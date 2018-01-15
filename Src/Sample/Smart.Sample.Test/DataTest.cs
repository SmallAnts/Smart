using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using Smart.Sample.Core.Context;
using Smart.Sample.Core.Entites;
using Smart.Core.Extensions;

namespace Smart.Sample.Test
{
    [TestClass]
    public class DataTest : UnitTestBase
    {
        Services.Sys.UserService userService;

        public DataTest() : base()
        {
            userService = new Services.Sys.UserService();

        }

        [TestMethod]
        public void TransTest()
        {
            var db = new SampleDbContext();
            var entity = db.SysUser.FirstOrDefault();
            var entity2 = entity.Copy();
            entity.UpdateTime = DateTime.Now;

            var trans = db.Database.BeginTransaction();
            //db.Database.BeginTransaction();
            var db2 = new SampleDbContext();

            var t = db2.Database.Connection.Equals(db.Database.Connection);
            db2.Database.UseTransaction(trans.UnderlyingTransaction);
            //db2.Database.BeginTransaction();
            db.SysUser.Add(entity2);
            db.SaveChanges();
            trans.Rollback();
            entity2.LoginName = "test003";
            db.SaveChanges();

            trans.Commit();
        }

        // 初始化数据库
        [TestMethod]
        public void DatabaseInit()
        {
            var db = new SampleDbContext();
            db.Role.Where("");
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SampleDbContext>());
            if (db.Database.Exists())
            {
                db.Database.Delete();
            }
            db.Database.CreateIfNotExists();
            Init_Roles();
            Init_SysUser();
            Init_SysFuncs();
        }

        public void Init_SysUser()
        {
            userService.AddSysUser(new SysUser
            {
                LoginName = "admin",
                BeautySalonId = 1,
                Password = "admin",
                IsAdmin = true
            }, 1);
        }
        [TestMethod]
        public void Init_SysFuncs()
        {
            var funcs = new List<SysFunc>();
            var actions = new List<SysAction>();
         
            funcs.Add(new SysFunc { SysFuncId = "000000", Name = "主页", ParentId = "", Icon = "fa-home", Url = "/home/firstpage" });

            funcs.Add(new SysFunc { SysFuncId = "010000", Name = "系统设置", ParentId = "", Icon = "fa-cogs" });

            funcs.Add(new SysFunc { SysFuncId = "010100", Name = "角色维护", ParentId = "010000", Url = "/sys/roles" });
            actions.Add(new SysAction { Name = "新增", SysFuncId = "010100",SysActionId="01010002" });
            actions.Add(new SysAction { Name = "修改", SysFuncId = "010100",SysActionId="01010003" });
            actions.Add(new SysAction { Name = "删除", SysFuncId = "010100", SysActionId="01010004" });
            actions.Add(new SysAction { Name = "权限设置", SysFuncId = "010100", SysActionId="01010005" });
            
            funcs.Add(new SysFunc { SysFuncId = "010200", Name = "用户维护", ParentId = "010000", Url = "/sys/sysusers" });
            funcs.Add(new SysFunc { SysFuncId = "010300", Name = "参数设置", ParentId = "010000", Url = "/sys/settings" });

            funcs.Add(new SysFunc { SysFuncId = "020000", Name = "示例", ParentId = "", Icon = "fa-list" });
            funcs.Add(new SysFunc { SysFuncId = "020100", Name = "树控件", ParentId = "020000", Url = "/demo/ztree" });
            funcs.Add(new SysFunc { SysFuncId = "020200", Name = "下拉树控件", ParentId = "020000", Url = "/demo/combotree" });
            funcs.Add(new SysFunc { SysFuncId = "020300", Name = "下拉表格控件", ParentId = "020000", Url = "/demo/combogrid" });
            funcs.Add(new SysFunc { SysFuncId = "020400", Name = "下拉控件Chosen", ParentId = "020000", Url = "/demo/chosen" });
            funcs.Add(new SysFunc { SysFuncId = "020500", Name = "下拉控件Select2", ParentId = "020000", Url = "/demo/select2" });
            funcs.Add(new SysFunc { SysFuncId = "020600", Name = "下拉多选控件", ParentId = "020000", Url = "/demo/multiselect" });

            var roleActions = new List<RoleSysAction>();
            for (int i = 0; i < funcs.Count; i++)
            {
                funcs[i].DisplayIndex = i * 2;
                if (!funcs.Exists(f => f.ParentId == funcs[i].SysFuncId))
                {
                    actions.Add(new SysAction
                    {
                        SysFuncId = funcs[i].SysFuncId,
                        Name = "View",
                        SysActionId = funcs[i].SysFuncId + "01",
                    });
                }
            }
            userService.ClearSysFunctions();
            userService.AddSysfuncs(funcs);
            userService.AddSysActions(actions);
            var oids = userService.db.SysAction.Select(p => p.SysActionId).ToArray();
            userService.SetRoleSysActions(1, 1, oids);
        }

        public void Init_Roles()
        {
            userService.AddRole(new Role { Name = "超级管理员", Note = "系统超级管理员角色", IsSysRole = true });
        }

    }
}
