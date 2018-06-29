using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class SeasonMap : EntityTypeConfiguration<Season>
    {
        public SeasonMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Seasons");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Translation).HasColumnName("Translation");
            this.Property(t => t.AdaptationID).HasColumnName("Adaptation");
            this.Property(t => t.AirOrder).HasColumnName("AirOrder");

            // Relationships
            this.HasRequired(t => t.Adaptation)
                .WithMany(t => t.Seasons)
                .HasForeignKey(d => d.AdaptationID);

        }
    }
}
