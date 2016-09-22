using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class HolmesLinkAppearanceMap : EntityTypeConfiguration<HolmesLinkAppearance>
    {
        public HolmesLinkAppearanceMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Appearances");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Actor).HasColumnName("Actor");
            this.Property(t => t.HolmesLink).HasColumnName("HolmesLink");

            // Relationships
            this.HasRequired(t => t.Actor1)
                .WithMany(t => t.HolmesLinkAppearances)
                .HasForeignKey(d => d.Actor);
            this.HasRequired(t => t.HolmesLink1)
                .WithMany(t => t.HolmesLinkAppearances)
                .HasForeignKey(d => d.HolmesLink);

        }
    }
}
