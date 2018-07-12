using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class EpisodeMap : EntityTypeConfiguration<Episode>
    {
        public EpisodeMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            Property(t => t.StoryID)
                .IsFixedLength()
                .HasMaxLength(4);

            // Table & Column Mappings
            ToTable("Episodes");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.SeasonID).HasColumnName("Season");
            Property(t => t.StoryID).HasColumnName("Story");
            Property(t => t.Airdate).HasColumnName("Airdate");
            Property(t => t.Title).HasColumnName("Title");
            Property(t => t.Translation).HasColumnName("Translation");
            Property(t => t.AirdatePrecision).HasColumnName("AirdatePrecision");

            // Relationships
            HasRequired(t => t.Season)
                .WithMany(t => t.Episodes)
                .HasForeignKey(d => d.SeasonID);
            HasOptional(t => t.Story)
                .WithMany(t => t.Episodes)
                .HasForeignKey(d => d.StoryID);

        }
    }
}
