using Smart.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smart.Sample.Core.IServices
{
    /// <summary>
    /// 用户服务接口, 这是一个示例，把所有功能都放到这个接口里了，实际应该分的更细一点
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 系统登录验证
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        Entites.SysUser SignIn(string loginName, string password);

        /// <summary>
        /// 新增系统用户
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="roleIds">角色ID集体</param>
        /// <returns></returns>
        int AddSysUser(Entites.SysUser user, int[] roleIds);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int DeleteSysUser(int userId);
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        int UpdateSysUser(Entites.SysUser user, int[] roleIds);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="salonId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="IsDelete"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        Page<dynamic> GetSysUsers(int salonId, int pageIndex, int pageSize, bool IsDelete, string search);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldpassword"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        int ChangePassword(int userId, string oldpassword, string password);
        /// <summary>
        /// 获取用户有权限的功能列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        List<Entites.SysFunc> GetUserFuncs(int userId);

        /// <summary>
        /// 获取用户权限信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<Entites.SysAction> GetUserPrivileges(int userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        int AddRole(Entites.Role role);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        int DeleteRole(int roleId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        int UpdateRole(Entites.Role role);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        Page<Entites.Role> GetRoles(int pageIndex, int pageSize, string search);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<Entites.Role> GetAllRoles();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        List<string> GetRoleSysActions(int roleId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <param name="sysActionIds"></param>
        /// <returns></returns>
        int SetRoleSysActions(int userId, int roleId, string[] sysActionIds);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<DTOs.RightInfo> GetRights();
    }
}
