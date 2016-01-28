using System.Data.Entity;
using System.Reflection;

namespace Smart.Samples.Domain.Context
{
    internal class SampleDbContext : Data.EFDbContext
    {
        static SampleDbContext()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SampleDbContext>());
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
