using Smart.Web.Mvc;
using Smart.Core.Extensions;
using Smart.Sample.Core.Entites;
using System.IO;

namespace Smart.Sample.Web.Controllers
{
    public abstract class ControllerBase : BaseController
    {
        private SysUser _operator;
        /// <summary>
        /// 当前登录用户信息
        /// </summary>
        protected SysUser Operator
        {
            get
            {
                if (_operator == null)
                {
                    var userinfo = this.User as UserInfo<SysUser>;
                    if (userinfo == null)
                    {
                        userinfo = FormsAuth.GetUser<SysUser>(this.Request);
                    }
                    _operator = userinfo == null ? null : userinfo.UserData;
                }
                return _operator ?? new SysUser() { SysUserId = 1, BeautySalonId = 1 };
            }
        }

        public log4net.ILog Log
        {
            get { return Smart.Core.SmartContext.Current.Resolve<log4net.ILog>(); }
        }

        private SysParam _sysParams;
        public SysParam SysParams
        {
            get { return _sysParams ?? (_sysParams = GetSysParams()); }
        }

        private SysParam GetSysParams()
        {
            try
            {
                var filename = HttpContext.Server.MapPath("~/sys_params.json");
                if (!System.IO.File.Exists(filename))
                {
                    return new SysParam();
                }
                var json = string.Empty;
                using (var fs = new FileStream(filename, FileMode.Open))
                {
                    using (var sr = new StreamReader(fs))
                    {
                        json = sr.ReadToEnd();
                    }
                }
                var sysParams = json.JsonTo<SysParam>();
                return sysParams;
            }
            catch (System.Exception ex)
            {
                Log.Error(ex);
                return new SysParam();
            }

        }
    }
}