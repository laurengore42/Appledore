using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class AdaptationMap : EntityTypeConfiguration<Adaptation>
    {
        public AdaptationMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(1000);

            this.Property(t => t.Company)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("Adaptations");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Translation).HasColumnName("Translation");
            this.Property(t => t.Medium).HasColumnName("Medium");
            this.Property(t => t.Company).HasColumnName("Company");

            // Relationships
            this.HasRequired(t => t.Medium1)
                .WithMany(t => t.Adaptations)
                .HasForeignKey(d => d.Medium);

        }
    }
}
