using Smart.Web.Mvc;
using Smart.Web.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Smart.Core.Extensions;
using Smart.Web.Mvc.UI.JqGrid;
using System.IO;
using System.Web;
using System.Reflection;
using System.Collections;
using Smart.Core.Caching;
using Newtonsoft.Json;

namespace Smart.Sample.Web.Controllers
{
    public class SysController : ControllerBase
    {
        Core.IServices.IUserService userService;

        public SysController()
        {
            userService = this.GetService<Core.IServices.IUserService>();
        }
        #region 参数设置
        public ActionResult Settings()
        {
            return View(this.SysParams);
        }
        [HttpPost]
        //[FormAuthorize(OperationCode = "010301")]
        public JsonResult SaveSysParams(Core.Entites.SysParam sysParams)
        {
            var filename = HttpContext.Server.MapPath("~/sys_params.json");
            var json = sysParams.ToJson(Formatting.Indented);
            using (var fs = new FileStream(filename, FileMode.Create))
            {
                using (var sw = new StreamWriter(fs))
                {
                    sw.Write(json);
                }
            }
            return Json(new { value = "Save success!".T() });
        }
        #endregion

        #region 用户管理
        public ActionResult SysUsers()
        {
            ViewBag.Roles = userService.GetAllRoles();
            return View();
        }
        [HttpPost]
        public JsonResult GetSysUsers(BindArgs args, int? state, string search)
        {
            var page = userService.GetSysUsers(Operator.BeautySalonId, args.PageIndex, args.PageSize, state == 1, search);
            return Json(new GridData
            {
                Page = 1,
                Records = page.TotalItems,
                Total = page.TotalPages,
                Rows = page.Items,
            });
        }
        [HttpPost]
        public JsonResult SaveSysUser(Core.Entites.SysUser user, string roleIds)
        {
            var ret = 0;
            var roles = GetRoleIdArray(roleIds);
            if (user.SysUserId <= 0)
            {
                user.CreateUserId = Operator.SysUserId;
                user.BeautySalonId = Operator.BeautySalonId;
                ret = userService.AddSysUser(user, roles);
            }
            else
            {
                user.UpdateUserId = Operator.SysUserId;
                ret = userService.UpdateSysUser(user, roles);
            }
            return ret > 0 ? Json(new { value = user }) : Json(new { error = "Save failed!".T() });
        }
        [HttpPost]
        public JsonResult DeleteSysUser(Core.Entites.SysUser user)
        {
            var ret = userService.DeleteSysUser(user.SysUserId);
            return Json(new { value = ret });
        }
        #endregion

        #region 角色管理
        public ActionResult Roles()
        {
            var rights = userService.GetRights();
            var funcNodes = new List<object>();
            foreach (var item in rights)
            {
                funcNodes.Add(new { id = item.Id, pId = item.ParentId, name = item.Name });
            }
            ViewBag.funcNodes = funcNodes.ToJson();
            return View();
        }
        [HttpPost]
        public JsonResult GetRoles(BindArgs args, string search)
        {
            var page = userService.GetRoles(args.PageIndex, args.PageSize, search);
            return Json(new GridData
            {
                Page = 1,
                Records = page.TotalItems,
                Total = page.TotalPages,
                Rows = page.Items,
            });
        }
        [HttpPost]
        public JsonResult SaveRole(Core.Entites.Role role)
        {
            var ret = 0;
            if (role.RoleId <= 0)
            {
                role.CreateUserId = Operator.SysUserId;
                role.BeautySalonId = Operator.BeautySalonId;
                ret = userService.AddRole(role);
            }
            else
            {
                role.UpdateUserId = Operator.SysUserId;
                ret = userService.UpdateRole(role);
            }
            return ret > 0 ? Json(new { value = role }) : Json(new { error = "Save failed!".T() });
        }
        [HttpPost]
        public JsonResult DeleteRole(Core.Entites.Role role)
        {
            var ret = userService.DeleteRole(role.RoleId);
            return Json(new { value = ret });
        }
        [HttpPost]
        public JsonResult GetRolePrivileges(int roleId)
        {
            var ret = userService.GetRoleSysActions(roleId);
            return Json(new { value = ret });
        }
        [HttpPost]
        public JsonResult SetRolePrivileges(int roleId, string[] operations)
        {
            var ret = userService.SetRoleSysActions(Operator.SysUserId, roleId, operations);
            return Json(new { value = ret });
        }

        // 逗号隔开的字符串转换为数组
        private int[] GetRoleIdArray(string roleIds)
        {
            if (roleIds.IsEmpty()) return null;
            var ids = roleIds.Split(',');
            var ret = new List<int>();
            foreach (var item in ids)
            {
                var id = item.AsInt();
                if (id > 0) ret.Add(id);
            }
            return ret.ToArray();
        }
        #endregion

        #region 登录注册

        [AllowAnonymous]
        public ActionResult SignIn(string returnUrl = null)
        {
            this.ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult SignIn(Models.SignInModel model)
        {
            var user = userService.SignIn(model.LoginName, model.Password);
            if (user == null)
            {
                return Json(new { error = "Incorrect username or password！".T() });
            }
            else
            {
                FormsAuth.SignIn<Core.Entites.SysUser>(model.LoginName, user, model.RememberMe);
                return Json(new { value = user });
            }
        }


        [HttpPost]
        [AllowAnonymous]
        public JsonResult RetrievePassword()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public JsonResult ChangePassword(int userId, string oldPassword, string newPassword, string confirmPassword)
        {
            var ret = userService.ChangePassword(userId, oldPassword, newPassword);
            return Json(new { value = ret });
        }

        #endregion


        #region 缓存管理

        public ActionResult Cache()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Cache(BindArgs args)
        {
            var data = LoadCaches();
            return Json(new GridData
            {
                Page = args.PageIndex,
                Records = data.Count,
                Total = data.Count,
                Rows = data,
            });
        }

        [HttpPost]
        public JsonResult ClearCache(string key)
        {
            var cm = this.GetService<ICache>("ServiceCache");
            cm.Remove(key);
            return Json(new { value = true });
        }
        List<cacheInfo> LoadCaches()
        {
            var data = new List<cacheInfo>();
            var _cache = HttpRuntime.Cache;
            var caches = _cache.GetEnumerator();
            Assembly assembly = Assembly.GetAssembly(typeof(System.Web.Caching.Cache));
            // System.Web.Caching.CacheInternal 
            var nonpubFlag = BindingFlags.NonPublic | BindingFlags.Instance;
            var cacheField = typeof(System.Web.Caching.Cache).GetField("_cacheInternal", nonpubFlag);
            var _cacheInternal = cacheField.GetValue(_cache);
            var _getCacheSingle = _cacheInternal.GetType().GetMethod("GetCacheSingle", nonpubFlag);
            var cacheEntryType = assembly.GetType("System.Web.Caching.CacheEntry");
            while (caches.MoveNext())
            {
                var ci = new cacheInfo();
                var type = caches.Value.GetType();
                if (caches.Key.ToString().StartsWith("__")) continue;
                data.Add(ci);
                ci.Key = caches.Key.ToString();
                ci.Type = type.Name;
                ci.Value = caches.Value.ToString();

                if (type.IsGenericType)
                {
                    var typeDef = type.GetGenericTypeDefinition();
                    if (typeDef == typeof(List<>) || typeDef == typeof(Dictionary<,>))
                    {
                        ci.Count = type.GetProperty("Count").GetValue(caches.Value, null).ToString();
                    }
                }
                else if (type.IsArray)
                {
                    ci.Count = type.GetProperty("Length").GetValue(caches.Value, null).ToString();
                }

                var _cacheMultiple = _getCacheSingle.Invoke(_cacheInternal, new object[] { ci.Key.GetHashCode() });
                var entryField = _cacheMultiple.GetType().GetField("_entries", nonpubFlag);
                var _entries = entryField.GetValue(_cacheMultiple) as Hashtable;
                var _keyPI = cacheEntryType.GetProperty("Key", nonpubFlag);
                var _utcCreated = cacheEntryType.GetField("_utcCreated", nonpubFlag);
                var _utcExpires = cacheEntryType.GetField("_utcExpires", nonpubFlag);
                var _slidingExpiration = cacheEntryType.GetField("_slidingExpiration", nonpubFlag);
                var _utcLastUpdate = cacheEntryType.GetField("_utcLastUpdate", nonpubFlag);
                var _dependency = cacheEntryType.GetField("_dependency", nonpubFlag);
                foreach (DictionaryEntry item in _entries)
                {
                    if (_keyPI.GetValue(item.Value, null).ToString() == ci.Key)
                    {
                        var dt = _utcCreated.GetValue(item.Value).AsString().AsDateTime();
                        ci.Created = dt.AddHours(8).ToString();

                        dt = _utcLastUpdate.GetValue(item.Value).AsString().AsDateTime();
                        ci.LastUpdate = dt.Year == 1 ? ci.Created : dt.AddHours(8).ToString();

                        dt = _utcExpires.GetValue(item.Value).AsString().AsDateTime();
                        ci.Expires = dt.AddHours(8).ToString();

                        dt = _slidingExpiration.GetValue(item.Value).AsString().AsDateTime();
                        var ts = (dt - DateTime.Today);
                        ci.SlidingExpiration = ts.TotalSeconds == 0 ? null : ts.ToString();

                        ci.Dependency = _dependency.GetValue(item.Value).AsString();
                        break;
                    }
                }
            }
            return data;
        }
        #endregion

    }

    class cacheInfo
    {
        protected static Dictionary<string, string> dicKeys;
        static cacheInfo()
        {
            dicKeys = new Dictionary<string, string>();
        }
        public string Name
        {
            get
            {
                string name;
                if (dicKeys.TryGetValue(this.Key, out name))
                {
                    return name;
                }
                return string.Empty;
            }
        }
        public string Key { get; set; }
        public string Type { get; set; }
        public string Count { get; set; }
        public string Value { get; set; }
        public string Created { get; set; }
        public string LastUpdate { get; set; }
        public string Expires { get; set; }
        public string SlidingExpiration { get; set; }
        public string Dependency { get; set; }
    }
}