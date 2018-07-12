using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class webpages_RolesMap : EntityTypeConfiguration<webpages_Roles>
    {
        public webpages_RolesMap()
        {
            // Primary Key
            HasKey(t => t.RoleId);

            // Properties
            Property(t => t.RoleName)
                .IsRequired()
                .HasMaxLength(2561);

            // Table & Column Mappings
            ToTable("webpages_Roles");
            Property(t => t.RoleId).HasColumnName("RoleId");
            Property(t => t.RoleName).HasColumnName("RoleName");
        }
    }
}
