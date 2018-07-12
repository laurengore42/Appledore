using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class DateMap : EntityTypeConfiguration<Date>
    {
        public DateMap()
        {
            // Primary Key
            HasKey(t => t.StoryID);

            // Properties
            Property(t => t.StoryID)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(4);

            // Table & Column Mappings
            ToTable("Dates");
            Property(t => t.StoryID).HasColumnName("Story");
            Property(t => t.BaringGouldStart).HasColumnName("BaringGouldStart");
            Property(t => t.BaringGouldEnd).HasColumnName("BaringGouldEnd");
            Property(t => t.Watson).HasColumnName("Watson");
            Property(t => t.BaringGouldPrecision).HasColumnName("BaringGouldPrecision");

            // Relationships
            HasRequired(t => t.Story)
                .WithOptional(t => t.Date);

        }
    }
}
