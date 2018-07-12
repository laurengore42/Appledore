using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class HolmesLinkAppearanceMap : EntityTypeConfiguration<HolmesLinkAppearance>
    {
        public HolmesLinkAppearanceMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            ToTable("HolmesLinkAppearances");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.HolmesLinkActorID).HasColumnName("HolmesLinkActor");
            Property(t => t.HolmesLinkID).HasColumnName("HolmesLink");

            // Relationships
            HasRequired(t => t.HolmesLinkActor)
                .WithMany(t => t.HolmesLinkAppearances)
                .HasForeignKey(d => d.HolmesLinkActorID);
            HasRequired(t => t.HolmesLink)
                .WithMany(t => t.HolmesLinkAppearances)
                .HasForeignKey(d => d.HolmesLinkID);

        }
    }
}
