using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class GenderMap : EntityTypeConfiguration<Gender>
    {
        public GenderMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("Genders");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.Name).HasColumnName("Name");
        }
    }
}
