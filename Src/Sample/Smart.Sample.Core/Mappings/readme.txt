数据表映射配置类，如：
只要添加该类即可自动添加映射

using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart.Sample.Core.Mappings
{
    class UserInfoMapping : EntityTypeConfiguration<Entites.PersonBase>
    {
        public UserInfoMapping()
        {
            this.ToTable("UserInfo");
            this.HasKey(u => u.Id).Property(u => u.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
