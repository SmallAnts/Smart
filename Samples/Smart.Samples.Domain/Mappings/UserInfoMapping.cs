using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart.Samples.Domain.Mappings
{
    class UserInfoMapping : EntityTypeConfiguration<Entites.UserInfo>
    {
        public UserInfoMapping()
        {
            this.ToTable("UserInfo");
            this.HasKey(u => u.Id).Property(u => u.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
