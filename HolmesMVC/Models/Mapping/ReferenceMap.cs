using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class ReferenceMap : EntityTypeConfiguration<Reference>
    {
        public ReferenceMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.StoryID)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(4);

            // Table & Column Mappings
            this.ToTable("References");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.StoryID).HasColumnName("Story");
            this.Property(t => t.EpisodeID).HasColumnName("Episode");

            // Relationships
            this.HasRequired(t => t.Episode)
                .WithMany(t => t.References)
                .HasForeignKey(d => d.EpisodeID);
            this.HasRequired(t => t.Story)
                .WithMany(t => t.References)
                .HasForeignKey(d => d.StoryID);

        }
    }
}
