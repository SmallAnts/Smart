using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Data.Common;

namespace Smart.Tests
{
    [TestClass]
    public class DatabaseTest : TestBase
    {
        public DatabaseTest() : base() { }

        [TestMethod]
        public void TestMethod1()
        {
            var db = new Samples.Domain.Context.SampleDbContext();
            var t = new Data.EF.EFRepository<Samples.Domain.Entites.UserInfo>(db).Get("Id=@0", 12);
            var ts = new Data.EF.EFRepository<Samples.Domain.Entites.UserInfo>(db).Query("Id=@0", 12);
            var userRepo = new Data.EF.EFRepository<Samples.Domain.Entites.UserInfo>(db);
            var accountRepo = new Data.EF.EFRepository<Samples.Domain.Entites.AccountInfo>(db);
            var query = from a in userRepo.Table
                        join b in accountRepo.Table on a.Name equals b.Name
                        select new { a, b };

        }
    }
}
