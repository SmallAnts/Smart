using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.Core.Extensions;

namespace Smart.Tests
{
    [TestClass]
    public class HttpCacheTest
    {
        [TestMethod]
        public void TestGet()
        {
            var cache = new Core.Caching.HttpCache();
            var test1 = cache.Get("test", () => new int[] { 1, 2, 3, 4, 5, 6 });
            var test2 = cache.Get<int[]>("test");
            Assert.IsTrue(test1 != null);
            Assert.IsTrue(test2 != null);
        }

        [TestMethod]
        public void Test()
        {
            IRepository<SysUser, IDbContext> repo = new Repository<SysUser, MyDbContext>(new MyDbContext());
        }

    }

    public class SysUser
    {
        public int SysUserId { get; set; }
    }
    public class DbContext
    {
    }
    public class MyDbContext : DbContext, IDbContext
    {

    }

    public interface IRepository<T>
    {
        T Get(object id);
    }
    public interface IRepository<TEntity, out TDbContext> : IRepository<TEntity> { }

    public interface IDbContext { }

    public class Repository<T> : IRepository<T> where T : class, new()
    {
        public DbContext _dbContext;
        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public T Get(object id)
        {
            return new T();
        }
    }

    public class Repository<TEntity, TDbContext> : IRepository<TEntity, TDbContext>
        where TEntity : class, new()
    {
        public DbContext _dbContext;
        public Repository(TDbContext dbContext)
        {
            _dbContext = dbContext as DbContext;
        }
        public TEntity Get(object id)
        {
            return new TEntity();
        }
    }
}
