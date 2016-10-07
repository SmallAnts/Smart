using Smart.Core.Extensions; // 该命名空间下扩展了很多实例方法
using Smart.Data.Extensions;
using Smart.Sample.Core.Entites;
using System;
using System.Linq;
using System.Collections.Generic;
using Smart.Data;

namespace Smart.Sample.Services.Sys
{
    // 服务类使用 internal，避免被其它程序集直接实例化，应该使用依赖注入的方式得到服务实例
    internal class UserService : ServiceBase, Core.IServices.IUserService
    {
        // 登录验证
        public SysUser SignIn(string loginName, string password)
        {
            var pwd = EncryptPassword(password);
            // 根据用户名密码获取用户信息
            var user = this.db.SysUser.FirstOrDefault(u => u.LoginName == loginName && u.Password == pwd);
            if (user != null)
            {
                // 更新最后登录时间
                user.LastLoginTime = DateTime.Now;
                this.db.SaveChanges();
            }
            return user;
        }

        // 新增系统用户
        public int AddSysUser(SysUser user, params int[] roleIds)
        {
            if (user.LoginName.IsEmpty())
            {
                throw new ServiceException("Login name is required!".T());
            }
            if (this.db.SysUser.Any(u => u.LoginName == user.LoginName))
            {
                throw new ServiceException("Login name already exists.".T());
            }
            using (var ts = db.Database.BeginTransaction())
            {
                user.CreateTime = DateTime.Now;
                user.Password = EncryptPassword(user.Password);
                this.db.SysUser.Add(user);
                var ret = this.db.SaveChanges();
                if (roleIds != null)
                {
                    SetUserRoles(user.CreateUserId, user.SysUserId, roleIds);
                    if (ret > 0)
                    {
                        this.db.SaveChanges();
                    }
                }
                ts.Commit();
                return ret;
            }

        }
        public int DeleteSysUser(int userId)
        {
            var user = this.db.SysUser.Find(userId);
            if (user == null) throw new Exception("Account does not exist".T());
            user.IsDelete = true;
            user.UpdateTime = DateTime.Now;
            this.db.Update(user);
            return this.db.SaveChanges();
        }

        // 只能更新 UserName, LoginName, UserRoles
        public int UpdateSysUser(SysUser user, int[] roleIds)
        {
            if (roleIds != null)
            {
                SetUserRoles(user.UpdateUserId.Value, user.SysUserId, roleIds);
            }
            user.UpdateTime = DateTime.Now;
            this.db.Update(user, updateProperties: u => new
            {
                u.UserName,
                u.LoginName,
                u.EmployeeId,
                u.UpdateTime,
                u.UpdateUserId
            });
            return this.db.SaveChanges();
        }
        public Page<dynamic> GetSysUsers(int salonId, int pageIndex, int pageSize, bool IsDelete, string search)
        {
            var rolesQuery = from a in this.db.Role
                             join b in this.db.UserRole on a.RoleId equals b.RoleId
                             select new { a.RoleId, a.Name, b.UserId };

            var query = from u in this.db.SysUser
                        where u.IsDelete == IsDelete && u.BeautySalonId == salonId
                        select new
                        {
                            #region 用户信息字段
                            BeautySalonId = u.BeautySalonId,
                            CreateTime = u.CreateTime,
                            CreateUserId = u.CreateUserId,
                            EmployeeId = u.EmployeeId,
                            IsAdmin = u.IsAdmin,
                            IsDelete = u.IsDelete,
                            LastLoginTime = u.LastLoginTime,
                            LoginName = u.LoginName,
                            Password = u.Password,
                            Roles = rolesQuery.Where(r => r.UserId == u.SysUserId).Select(r => r.Name),
                            SysUserId = u.SysUserId,
                            UpdateTime = u.UpdateTime,
                            UpdateUserId = u.UpdateUserId,
                            UserName = u.UserName,
                            #endregion
                        };
            if (!search.IsEmpty()) query = query.Where(d => d.LoginName.Contains(search));
            var result = query.OrderBy(d => d.LoginName).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList<dynamic>();
            var page = new Page<dynamic>();
            page.TotalItems = query.Count();
            page.Items = result;
            return page;
        }
        public int ChangePassword(int userId, string oldpassword, string password)
        {
            if (password.IsEmpty()) throw new Exception("The new password can not be empty".T());
            var user = this.db.SysUser.Find(userId);
            if (user == null) throw new Exception("Account does not exist".T());
            if (user.Password != EncryptPassword(oldpassword)) throw new Exception("The old password is incorrect".T());

            user.UpdateTime = DateTime.Now;
            user.Password = EncryptPassword(password);
            this.db.Update(user);
            return this.db.SaveChanges();
        }
        public int ResetPassword(int userId, string password)
        {
            if (password.IsEmpty()) throw new Exception("The new password can not be empty".T());
            var user = this.db.SysUser.Find(userId);
            if (user == null) throw new Exception("Account does not exist".T());

            user.UpdateTime = DateTime.Now;
            user.Password = EncryptPassword(password);
            this.db.Update(user);
            return this.db.SaveChanges();
        }
        // 从缓存获取用户权限
        public List<SysAction> GetUserPrivileges(int userId)
        {
            var userPrivileges = cache.Get(CacheKeys.UserPrivileges, () =>
            {
                var query = from a in db.UserRole
                            join b in db.RoleSysAction on a.RoleId equals b.RoleId
                            join c in db.SysAction on b.SysActionId equals c.SysActionId
                            where a.UserId == userId
                            select c;
                return query.ToList();
            });
            return userPrivileges;
        }

        // 从缓存获取用户功能信息
        public List<SysFunc> GetUserFuncs(int userId)
        {
            var userFuncs = cache.Get(CacheKeys.UserFuncs, () =>
            {
                // 获取所有功能
                var allFuncs = this.GetSysFuncs();

                // 获取用户有权限的功能信息
                var query = from a in db.UserRole
                            join b in db.RoleSysAction on a.RoleId equals b.RoleId
                            join c in db.SysAction on b.SysActionId equals c.SysActionId
                            join d in db.SysFunc on c.SysFuncId equals d.SysFuncId
                            where a.UserId == userId
                            orderby d.DisplayIndex
                            select d;
                var user_funcs = query.ToList();

                //递归添加上级功能信息
                var funcPids = user_funcs.Where(f => !f.ParentId.IsEmpty()).Select(f => f.ParentId).Distinct().ToArray();
                foreach (var pid in funcPids)
                {
                    AddParentFunc(allFuncs, user_funcs, pid);
                }
                return user_funcs;
            });
            return userFuncs;
        }

        // 获取所有系统功能信息
        public List<SysFunc> GetSysFuncs()
        {
            return this.db.SysFunc.AsNoTracking().ToList();
        }


        public int AddRole(Role role)
        {
            role.CreateTime = DateTime.Now;
            this.db.Role.Add(role);
            return this.db.SaveChanges();
        }
        public int DeleteRole(int roleId)
        {
            this.db.Remove(new Role { RoleId = roleId });
            // 清除缓存
            cache.Remove(CacheKeys.UserPrivileges);
            cache.Remove(CacheKeys.UserFuncs);
            return this.db.SaveChanges();
        }
        public int UpdateRole(Role role)
        {
            role.UpdateTime = DateTime.Now;
            this.db.Update(role, updateProperties: r => new { r.Name, r.Note, r.UpdateTime, r.UpdateUserId });
            return this.db.SaveChanges();
        }
        public List<Role> GetAllRoles()
        {
            return this.db.Role.AsNoTracking()
                .OrderBy(d => d.Name)
                .ToList();
        }
        public Page<Role> GetRoles(int pageIndex, int pageSize, string search)
        {
            IQueryable<Role> query = this.db.Role.AsNoTracking();
            if (!search.IsEmpty()) query = query
                    .Where(d => d.Name.Contains(search));
            var page = new Page<Role>();
            page.TotalItems = query.Count();
            page.Items = query
                .OrderBy(d => new { d.IsSysRole, d.Name })
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return page;
        }
        public List<string> GetRoleSysActions(int roleId)
        {
            return this.db.RoleSysAction
                .Where(p => p.RoleId == roleId)
                .Select(p => p.SysActionId)
                .ToList();
        }
        public int SetRoleSysActions(int userId, int roleId, string[] sysActionIds)
        {
            var ret = 0;
            using (var ts = db.Database.BeginTransaction())
            {
                ret += this.db.Database.ExecuteSqlCommand("DELETE RoleSysAction WHERE RoleId=" + roleId);
                foreach (var sysAction in sysActionIds)
                {
                    this.db.RoleSysAction.Add(new RoleSysAction
                    {
                        RoleId = roleId,
                        SysActionId = sysAction,
                        CreateTime = DateTime.Now,
                        CreateUserId = userId
                    });
                }
                this.db.SaveChanges();
                ts.Commit();
            }
            // 清除缓存
            cache.Remove(CacheKeys.UserPrivileges);
            cache.Remove(CacheKeys.UserFuncs);
            return ret;
        }

        public List<Core.DTOs.RightInfo> GetRights()
        {
            var rights = this.db.SysFunc.AsNoTracking()
                .OrderBy(f => f.DisplayIndex)
                .Select(f => new Core.DTOs.RightInfo
                {
                    Id = f.SysFuncId,
                    ParentId = f.ParentId,
                    Name = f.Name
                }).ToList();
            var actions = this.db.SysAction.AsNoTracking()
                .OrderBy(f => f.SysActionId)
                .Select(f => new Core.DTOs.RightInfo
                {
                    Id = f.SysActionId,
                    ParentId = f.SysFuncId,
                    Name = f.Name
                }).ToList();
            rights.AddRange(actions);
            return rights;
        }
        // 递归添加上级功能信息
        private void AddParentFunc(List<SysFunc> funcs, List<SysFunc> userFuncs, string pid)
        {
            var parentFunc = funcs.Find(f => f.SysFuncId == pid);
            if (parentFunc != null)
            {
                if (!userFuncs.Exists(f => f.SysFuncId == pid)) userFuncs.Add(parentFunc);
                if (!parentFunc.ParentId.IsEmpty())
                {
                    AddParentFunc(funcs, userFuncs, parentFunc.ParentId);
                }
            }
        }
        private void SetUserRoles(int operatorId, int userId, int[] roleIds)
        {
            var ret = 0;
            ret += this.db.Database.ExecuteSqlCommand("DELETE UserRole WHERE UserId=" + userId);
            foreach (var item in roleIds)
            {
                this.db.UserRole.Add(new UserRole
                {
                    UserId = userId,
                    RoleId = item,
                    CreateTime = DateTime.Now,
                    CreateUserId = operatorId
                });
            }
            // 清除缓存
            cache.Remove(CacheKeys.UserFuncs);
            cache.Remove(CacheKeys.UserPrivileges);
        }
        // 加密
        internal static string EncryptPassword(string password)
        {
            return ("smart" + password).MD5Encrypt();
        }

        internal void AddSysfuncs(List<SysFunc> funcs)
        {
            this.db.SysFunc.AddRange(funcs);
            this.db.SaveChanges();
        }

        internal void AddSysActions(List<SysAction> actions)
        {
            this.db.SysAction.AddRange(actions);
            this.db.SaveChanges();
        }
    }
}
