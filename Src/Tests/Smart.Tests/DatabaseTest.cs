using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Smart.Tests
{
    [TestClass]
    public class DatabaseTest : TestBase
    {
        [TestMethod]
        public void TestMethod1()
        {
            var db = new Samples.Domain.Context.SampleDbContext();
            var userRepo = new Data.EFRepository<Samples.Domain.Entites.UserInfo>(db);
            var accountRepo = new Data.EFRepository<Samples.Domain.Entites.AccountInfo>(db);
            var query = from a in userRepo.Table
                        join b in accountRepo.Table on a.Name equals b.Name
                        select new { a, b };
            
        }
    }
}
