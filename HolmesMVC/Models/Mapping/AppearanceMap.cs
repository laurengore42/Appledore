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
            this.Property(t => t.Actor).HasColumnName("Actor");
            this.Property(t => t.Character).HasColumnName("Character");
            this.Property(t => t.Episode).HasColumnName("Episode");
            this.Property(t => t.Pic).HasColumnName("Pic");

            // Relationships
            this.HasRequired(t => t.Actor1)
                .WithMany(t => t.Appearances)
                .HasForeignKey(d => d.Actor);
            this.HasRequired(t => t.Character1)
                .WithMany(t => t.Appearances)
                .HasForeignKey(d => d.Character);
            this.HasRequired(t => t.Episode1)
                .WithMany(t => t.Appearances)
                .HasForeignKey(d => d.Episode);

        }
    }
}
