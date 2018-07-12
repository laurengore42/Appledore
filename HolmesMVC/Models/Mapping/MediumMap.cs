
using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class MediumMap : EntityTypeConfiguration<Medium>
    {
        public MediumMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("Media");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.Name).HasColumnName("Name");
        }
    }
}
