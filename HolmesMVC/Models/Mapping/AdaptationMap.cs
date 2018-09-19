
using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class AdaptationMap : EntityTypeConfiguration<Adaptation>
    {
        public AdaptationMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            Property(t => t.Name)
                .HasMaxLength(1000);

            Property(t => t.Company)
                .HasMaxLength(1000);

            Property(t => t.UrlName)
                .HasMaxLength(150);

            // Table & Column Mappings
            ToTable("Adaptations");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.Translation).HasColumnName("Translation");
            Property(t => t.MediumID).HasColumnName("Medium");
            Property(t => t.Company).HasColumnName("Company");
            Property(t => t.UrlName).HasColumnName("UrlName");

            // Relationships
            HasRequired(t => t.Medium)
                .WithMany(t => t.Adaptations)
                .HasForeignKey(d => d.MediumID);

        }
    }
}
