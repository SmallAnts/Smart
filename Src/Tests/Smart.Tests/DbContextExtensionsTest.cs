using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.Core.Extensions;
using Smart.Data.Extensions;
using System.Data.Common;
using System.Collections.Generic;
using System.Data;

namespace Smart.Tests
{
    [TestClass]
    public class DbContextExtensionsTest : TestBase
    {
        public DbContextExtensionsTest() : base() { }
        // TODO:增加批量执行SQL方法
        /// <summary>
        /// Sql 查询测试
        /// </summary>
        [TestMethod]
        public void Query()
        {
            var db = new Smart.Data.EF.EFDbContext();
            var users = db.Query<SysUser>("select * from SysUser where SysUserId=@0", 1).ToList();
            var ps = new { Id = 1, Name = "admin" };
            var users2 = db.Query<SysUser>("select * from SysUser where SysUserId=@Id and Name=@Name", ps).ToList();
        }
        /// <summary>
        ///  Sql 查询测试,返回DataTable
        /// </summary>
        [TestMethod]
        public void QueryDataTable()
        {
            var db = new Smart.Data.EF.EFDbContext();
            var users = db.QueryDataTable("select * from SysUser where SysUserId=@0", 1);
        }
        /// <summary>
        /// 执行Sql，返回受影响行数
        /// </summary>
        [TestMethod]
        public void ExecuteSql()
        {
            var db = new Smart.Data.EF.EFDbContext();
            var sql = "update SysUser set UpdateTime=@1 where SysUserId=@0";
            db.ExecuteSql(sql, 1, DateTime.Now);
        }
        /// <summary>
        /// 执行Sql，返回第一行第一列的值
        /// </summary>
        [TestMethod]
        public void ExecuteScalar()
        {
            var db = new Smart.Data.EF.EFDbContext();
            var ret = (int)db.ExecuteScalar("select count(1) from SysUser");
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        [TestMethod]
        public void ExecuteProc()
        {
            #region 存储过程创建脚本
            //create proc HasRoleProc
            //@userId int,
            //@roleId int,
            //@hasRole int output --传出参数
            //as
            //if exists(select * from UserRole where UserId = @userId and RoleId = @roleId)
            //set @hasRole = 1
            //else
            //set @hasRole = 0
            //go
            #endregion
            //System.Data.SqlClient.SqlParameter 
            var db = new Smart.Data.EF.EFDbContext();
            var inParams = new { userId = 1, roleId = 1 };
            var hasRoleParam = db.CreateDbParameter("hasRole", DbType.Int32, ParameterDirection.Output);
            // TODO：增加多个输出参数的方法
            var ret = db.ExecuteProc("HasRoleProc", inParams, hasRoleParam);
        }
    }
}
