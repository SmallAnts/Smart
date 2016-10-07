using Smart.Core;
using Smart.Web.Mvc;
using System.Web;
using System.Web.Mvc;
using Smart.Sample.Services;
using Smart.Core.Extensions;

namespace Smart.Sample.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ErrorFilterAttribute());
        }

    }

    public class ErrorFilterAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            filterContext.ExceptionHandled = true;
            var result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = new { error = filterContext.Exception.GetBaseException().Message };
            filterContext.Result = result;
            if (!(filterContext.Exception is ServiceException))
            {
                try
                {
                    var log = SmartContext.Current.Resolve<log4net.ILog>();
                    log.Error(filterContext.Exception.GetBaseException());
                }
                catch
                {
                }
            }
        }
    }

    public class FormAuthorizeAttribute : AuthorizeAttribute
    {
        public string OperationCode { get; set; }

        private AuthorizationFailureReason failureReason { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var result = base.AuthorizeCore(httpContext);
            if (result == false) return false;
            if (!OperationCode.IsEmpty())
            {
                var userService = SmartContext.Current.Resolve<Core.IServices.IUserService>();
                var user = FormsAuth.GetUser<Core.Entites.SysUser>(httpContext.Request);
                var operation = userService.GetUserPrivileges(user.UserData.SysUserId).Find(p => p.SysActionId == OperationCode);
                if (operation == null)
                {
                    failureReason = AuthorizationFailureReason.NoOperatingRights;
                    return false;
                }
            }
            return true;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = FormsAuth.GetUser<Core.Entites.SysUser>(filterContext.HttpContext.Request);
            if (user != null && user.Identity != null)
            {
                filterContext.HttpContext.User = user;
            }
            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            switch (failureReason)
            {
                case AuthorizationFailureReason.NoOperatingRights:
                    var jsonResult = new JsonResult();
                    jsonResult.Data = new { error = "Sorry, you do not have permission to perform this operation.".T() };
                    filterContext.Result = jsonResult;
                    break;
                default:
                    filterContext.Result = new RedirectResult("/sys/signin");
                    break;
            }
        }
    }

    internal enum AuthorizationFailureReason
    {
        NoLogin,
        NoOperatingRights,
    }
}
