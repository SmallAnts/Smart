using Smart.Data.EF;
using System.Data.Entity;
using System.Reflection;
using Smart.Data.Extensions;

namespace Smart.Samples.Domain.Context
{
    // 多数据库支持
    public interface IOtherDbContext { }

    internal class OtherDbContext : EFDbContext, IOtherDbContext
    {
        static OtherDbContext()
        {
#if DEBUG
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<OtherDbContext>());
#else
            Database.SetInitializer<OtherDbContext>(null);
#endif
        }

        public OtherDbContext() : base("name=OtherConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.AddMappings(Assembly.GetExecutingAssembly());
        }
    }
}
