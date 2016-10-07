using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Data.Common;
using Smart.Data.Extensions;
using Smart.Core.Extensions;

namespace Smart.Tests
{
    [TestClass]
    public class DatabaseTest : TestBase
    {
        public DatabaseTest() : base() { }

        [TestMethod]
        public void TestMethod1()
        {
            var db = new Samples.Core.Context.SampleDbContext();
            var t = new Data.EF.EFRepository<Samples.Core.Entites.UserInfo>(db).Get("Id=@0", 12);
            var ts = new Data.EF.EFRepository<Samples.Core.Entites.UserInfo>(db).Query("Id=@0", 12);
            var userRepo = new Data.EF.EFRepository<Samples.Core.Entites.UserInfo>(db);
            var accountRepo = new Data.EF.EFRepository<Samples.Core.Entites.AccountInfo>(db);
            var query = from a in userRepo.Table
                        join b in accountRepo.Table on a.Name equals b.Name
                        select new { a, b };

        }

        [TestMethod]
        public void TestMethod2()
        {
            var db = new Samples.Core.Context.SampleDbContext();
            var rep = new Data.EF.EFRepository<Samples.Core.Entites.UserInfo>(db);
            var list = rep.GetPage(2, 2, "1=1");
        }

        [TestMethod]
        public void TestMethod3()
        {
            var db = new Samples.Core.Context.SampleDbContext();
            var rep = new Data.EF.EFRepository<Samples.Core.Entites.UserInfo>(db);
            var user = new Samples.Core.Entites.UserInfo { Name = "2341" };
            var ret = rep.Insert(user);
            var tb = db.QueryDataTable("select * from userinfo where name=@0", "2341");
            var ls = db.QueryDynamic("select * from userinfo where name=@0", "2341");
            var tbjson = tb.ToJson();
            var lsjson = ls.ToJson();
            var find = rep.TableNoTracking.ToList();
            //var entity = rep.GetById(1);
            //entity.Name = "fdsafdsafdsa";
            //rep.Update(entity);
            var ret2 = rep.Update(new Samples.Core.Entites.UserInfo { Id = 2, Name = "updat城e9999" });
        }
    }
}
