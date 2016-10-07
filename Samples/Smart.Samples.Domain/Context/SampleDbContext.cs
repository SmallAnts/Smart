using Smart.Data.EF;
using System.Data.Entity;
using System.Reflection;
using Smart.Data.Extensions;

namespace Smart.Samples.Domain.Context
{
    internal class SampleDbContext : EFDbContext
    {
        static SampleDbContext()
        {
#if DEBUG
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SampleDbContext>());
#else
            //Database.SetInitializer<SampleDbContext>(null);
#endif
        }

        public SampleDbContext() : base("name=DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.AddMappings(Assembly.GetExecutingAssembly());
        }
    }
}
