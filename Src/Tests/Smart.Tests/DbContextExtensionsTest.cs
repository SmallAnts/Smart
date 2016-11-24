using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.Core.Extensions;
using Smart.Data.Extensions;
namespace Smart.Tests
{
    [TestClass]
    public class DbContextExtensionsTest : TestBase
    {
        public DbContextExtensionsTest() : base() { }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void Query()
        {
            var db = new Smart.Data.EF.EFDbContext();
            var users = db.Query<SysUser>("select * from SysUser where SysUserId=@0", 1).ToList();
        }

        [TestMethod]
        public void QueryDataTable()
        {
            var db = new Smart.Data.EF.EFDbContext();
            var users = db.QueryDataTable("select * from SysUser where SysUserId=@0", 1);
        }

        [TestMethod]
        public void ExecuteSql()
        {
            var db = new Smart.Data.EF.EFDbContext();
            db.ExecuteSql("update SysUser set UpdateTime=@0 where SysUserId=@1", 1, DateTime.Now);
        }

        [TestMethod]
        public void ExecuteScalar()
        {
            var db = new Smart.Data.EF.EFDbContext();
            var ret = (int)db.ExecuteScalar("select count(1) from SysUser");
        }

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
            var db = new Smart.Data.EF.EFDbContext();
            var inParams = new { userId = 1, roleId = 1 };
            var ret = db.ExecuteProc("HasRoleProc", inParams, setOutParam: o =>
            {
                o.ParameterName = "hasRole";
                o.DbType = System.Data.DbType.Int32;
            });
        }
    }
}
