using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class DateMap : EntityTypeConfiguration<Date>
    {
        public DateMap()
        {
            // Primary Key
            this.HasKey(t => t.StoryID);

            // Properties
            this.Property(t => t.StoryID)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(4);

            // Table & Column Mappings
            this.ToTable("Dates");
            this.Property(t => t.StoryID).HasColumnName("Story");
            this.Property(t => t.BaringGouldStart).HasColumnName("BaringGouldStart");
            this.Property(t => t.BaringGouldEnd).HasColumnName("BaringGouldEnd");
            this.Property(t => t.Watson).HasColumnName("Watson");
            this.Property(t => t.BaringGouldPrecision).HasColumnName("BaringGouldPrecision");

            // Relationships
            this.HasRequired(t => t.Story)
                .WithOptional(t => t.Date);

        }
    }
}
