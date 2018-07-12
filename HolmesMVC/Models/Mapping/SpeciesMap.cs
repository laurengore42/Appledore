
using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class SpeciesMap : EntityTypeConfiguration<Species>
    {
        public SpeciesMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("Species");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.Name).HasColumnName("Name");
        }
    }
}
