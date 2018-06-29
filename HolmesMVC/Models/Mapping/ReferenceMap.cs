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
            this.Property(t => t.Story)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(4);

            // Table & Column Mappings
            this.ToTable("References");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Story).HasColumnName("Story");
            this.Property(t => t.Episode).HasColumnName("Episode");

            // Relationships
            this.HasRequired(t => t.Episode1)
                .WithMany(t => t.References)
                .HasForeignKey(d => d.Episode);
            this.HasRequired(t => t.Story1)
                .WithMany(t => t.References)
                .HasForeignKey(d => d.Story);

        }
    }
}
