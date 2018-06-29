using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class AppearanceMap : EntityTypeConfiguration<Appearance>
    {
        public AppearanceMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Appearances");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ActorID).HasColumnName("Actor");
            this.Property(t => t.CharacterID).HasColumnName("Character");
            this.Property(t => t.EpisodeID).HasColumnName("Episode");
            this.Property(t => t.Pic).HasColumnName("Pic");

            // Relationships
            this.HasRequired(t => t.Actor)
                .WithMany(t => t.Appearances)
                .HasForeignKey(d => d.ActorID);
            this.HasRequired(t => t.Character)
                .WithMany(t => t.Appearances)
                .HasForeignKey(d => d.CharacterID);
            this.HasRequired(t => t.Episode)
                .WithMany(t => t.Appearances)
                .HasForeignKey(d => d.EpisodeID);

        }
    }
}
