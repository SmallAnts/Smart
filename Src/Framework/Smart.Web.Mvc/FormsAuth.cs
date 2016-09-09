using Smart.Core.Extensions;
using Newtonsoft.Json;
using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;

namespace Smart.Web.Mvc
{
    /// <summary>
    /// 身份验证类
    /// </summary>
    public class FormsAuth
    {
        //Cookie保存是时间
        private const int CookieSaveDays = 7;

        /// <summary>
        /// 用户登录成功时设置Cookie
        /// </summary>
        /// <typeparam name="TUserData"></typeparam>
        /// <param name="username"></param>
        /// <param name="userData"></param>
        /// <param name="rememberMe"></param>
        public static void SignIn<TUserData>(string username, TUserData userData, bool rememberMe) where TUserData : class, new()
        {
            userData.ThrowIfNull("userinfo", "Authorized user information can not be empty! ".T());

            //创建ticket
            var ticket = new FormsAuthenticationTicket(
                version: 2,
                name: username,
                issueDate: DateTime.Now,
                expiration: DateTime.Now.AddTicks(FormsAuthentication.Timeout.Ticks),
                isPersistent: rememberMe,
                userData: userData.ToJson());

            //加密ticket
            var cookieValue = FormsAuthentication.Encrypt(ticket);

            //创建Cookie
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieValue)
            {
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL,
                Domain = FormsAuthentication.CookieDomain,
                Path = FormsAuthentication.FormsCookiePath,
            };
            if (rememberMe) cookie.Expires = DateTime.Now.AddDays(CookieSaveDays);
            //写入Cookie
            HttpContext.Current.Response.Cookies.Remove(cookie.Name);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        /// <summary>
        /// 从浏览器删除 Forms 身份验证票证。
        /// </summary>
        public static void SignOut()
        {
            FormsAuthentication.SignOut();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userinfoType"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static IPrincipal GetUser(Type userinfoType, HttpRequestBase request)
        {
            try
            {
                var ticket = GetTicket(request);
                object userinfo = Activator.CreateInstance(userinfoType, ticket);
                return userinfo as IPrincipal;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TUserData"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public static UserInfo<TUserData> GetUser<TUserData>(HttpRequestBase request) where TUserData : class, new()
        {
            try
            {
                var ticket = GetTicket(request);
                return new UserInfo<TUserData>(ticket);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static FormsAuthenticationTicket GetTicket(HttpRequestBase request)
        {
            request.ThrowIfNull("request");
            var cookie = request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value)) return null;
            try
            {
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                return ticket;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TUserData"></typeparam>
        /// <param name="request"></param>
        /// <param name="userdata"></param>
        public static void UpdateUserData<TUserData>(HttpRequestBase request, TUserData userdata) where TUserData : class, new()
        {
            var ticket = GetTicket(request);
            SignIn(ticket.Name, userdata, ticket.IsPersistent);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TUserData"></typeparam>
    public class UserInfo<TUserData> : IPrincipal where TUserData : class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticket"></param>
        public UserInfo(FormsAuthenticationTicket ticket)
        {
            if (ticket != null)
            {
                this.Identity = new FormsIdentity(ticket);
                if (!ticket.UserData.IsEmpty())
                    this.UserData = JsonConvert.DeserializeObject<TUserData>(ticket.UserData);
            }
        }

        public TUserData UserData { get; private set; }

        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FormAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userType"></param>
        public FormAuthorizeAttribute(Type userType)
        {
            this.UserType = userType;
        }
        /// <summary>
        /// 
        /// </summary>
        public Type UserType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return base.AuthorizeCore(httpContext);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var userinfoType = typeof(UserInfo<>).MakeGenericType(UserType);
            var user = FormsAuth.GetUser(userinfoType, filterContext.HttpContext.Request);
            if (user != null && user.Identity != null) filterContext.HttpContext.User = user;
            base.OnAuthorization(filterContext);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/account/sigin");
        }
    }
}