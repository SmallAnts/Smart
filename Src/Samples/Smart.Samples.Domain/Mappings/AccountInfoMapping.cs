using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart.Samples.Domain.Mappings
{
    class AccountInfoMapping : EntityTypeConfiguration<Entites.AccountInfo>
    {
        public AccountInfoMapping()
        {
            this.ToTable("AccountInfo");
            this.HasKey(u => u.Id).Property(u => u.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
