using System.Data.Entity;
using System.Reflection;

namespace Smart.Samples.Domain.Context
{
    internal class SampleDbContext : Data.EFDbContext
    {
        static SampleDbContext()
        {
#if DEBUG
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SampleDbContext>());
#else
            Database.SetInitializer<SampleDbContext>(null);
#endif
        }
        public SampleDbContext() : base("name=DefaultConnection")
        {
        }
        protected override void OnPreModelCreating(DbModelBuilder modelBuilder, Assembly assembly)
        {
            assembly = Assembly.GetExecutingAssembly();
            base.OnPreModelCreating(modelBuilder, assembly);
        }
    }
}
