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
            this.ToTable("HolmesLinkAppearances");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.HolmesLinkActorID).HasColumnName("HolmesLinkActor");
            this.Property(t => t.HolmesLinkID).HasColumnName("HolmesLink");

            // Relationships
            this.HasRequired(t => t.HolmesLinkActor)
                .WithMany(t => t.HolmesLinkAppearances)
                .HasForeignKey(d => d.HolmesLinkActorID);
            this.HasRequired(t => t.HolmesLink)
                .WithMany(t => t.HolmesLinkAppearances)
                .HasForeignKey(d => d.HolmesLinkID);

        }
    }
}
