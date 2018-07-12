using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class HolmesLinkMap : EntityTypeConfiguration<HolmesLink>
    {
        public HolmesLinkMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Table & Column Mappings
            ToTable("HolmesLinks");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.Airdate).HasColumnName("Airdate");
            Property(t => t.Title).HasColumnName("Title");
            Property(t => t.EpisodeID).HasColumnName("Episode");
            Property(t => t.AirdatePrecision).HasColumnName("AirdatePrecision");

            // Relationships
            HasOptional(t => t.Episode)
                .WithMany(t => t.HolmesLinks)
                .HasForeignKey(d => d.EpisodeID);
        }
    }
}
