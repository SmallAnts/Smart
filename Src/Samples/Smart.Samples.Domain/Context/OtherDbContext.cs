using System.Data.Entity;
using System.Reflection;

namespace Smart.Samples.Domain.Context
{
    // 多数据库支持
    public interface IOtherDbContext { }

    internal class OtherDbContext : Data.EFDbContext, IOtherDbContext
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
        protected override void OnPreModelCreating(DbModelBuilder modelBuilder, Assembly assembly)
        {
            assembly = Assembly.GetExecutingAssembly();
            base.OnPreModelCreating(modelBuilder, assembly);
        }
    }
}
