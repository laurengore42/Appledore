using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class ReferenceMap : EntityTypeConfiguration<Reference>
    {
        public ReferenceMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            Property(t => t.StoryID)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(4);

            // Table & Column Mappings
            ToTable("References");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.Description).HasColumnName("Description");
            Property(t => t.StoryID).HasColumnName("Story");
            Property(t => t.EpisodeID).HasColumnName("Episode");

            // Relationships
            HasRequired(t => t.Episode)
                .WithMany(t => t.References)
                .HasForeignKey(d => d.EpisodeID);
            HasRequired(t => t.Story)
                .WithMany(t => t.References)
                .HasForeignKey(d => d.StoryID);

        }
    }
}
