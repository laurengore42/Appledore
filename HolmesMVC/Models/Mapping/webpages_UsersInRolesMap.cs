using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class Webpages_UsersInRolesMap : EntityTypeConfiguration<Webpages_UsersInRoles>
    {
        public Webpages_UsersInRolesMap()
        {
            // Primary Key
            HasKey(t => new { t.UserId, t.RoleId });

            // Properties
            Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.RoleId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            ToTable("webpages_UsersInRoles");
            Property(t => t.UserId).HasColumnName("UserId");
            Property(t => t.RoleId).HasColumnName("RoleId");
        }
    }
}
