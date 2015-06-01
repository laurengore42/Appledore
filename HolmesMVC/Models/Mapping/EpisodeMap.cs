using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class EpisodeMap : EntityTypeConfiguration<Episode>
    {
        public EpisodeMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Story)
                .IsFixedLength()
                .HasMaxLength(4);

            // Table & Column Mappings
            this.ToTable("Episodes");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Season).HasColumnName("Season");
            this.Property(t => t.Story).HasColumnName("Story");
            this.Property(t => t.Airdate).HasColumnName("Airdate");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Translation).HasColumnName("Translation");
            this.Property(t => t.AirdatePrecision).HasColumnName("AirdatePrecision");

            // Relationships
            this.HasRequired(t => t.Season1)
                .WithMany(t => t.Episodes)
                .HasForeignKey(d => d.Season);
            this.HasOptional(t => t.Story1)
                .WithMany(t => t.Episodes)
                .HasForeignKey(d => d.Story);

        }
    }
}
