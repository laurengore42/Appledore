using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class AppearanceMap : EntityTypeConfiguration<Appearance>
    {
        public AppearanceMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            ToTable("Appearances");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.ActorID).HasColumnName("Actor");
            Property(t => t.CharacterID).HasColumnName("Character");
            Property(t => t.EpisodeID).HasColumnName("Episode");
            Property(t => t.Pic).HasColumnName("Pic");

            // Relationships
            HasRequired(t => t.Actor)
                .WithMany(t => t.Appearances)
                .HasForeignKey(d => d.ActorID);
            HasRequired(t => t.Character)
                .WithMany(t => t.Appearances)
                .HasForeignKey(d => d.CharacterID);
            HasRequired(t => t.Episode)
                .WithMany(t => t.Appearances)
                .HasForeignKey(d => d.EpisodeID);

        }
    }
}
