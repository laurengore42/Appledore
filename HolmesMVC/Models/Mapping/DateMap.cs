using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class DateMap : EntityTypeConfiguration<Date>
    {
        public DateMap()
        {
            // Primary Key
            this.HasKey(t => t.Story);

            // Properties
            this.Property(t => t.Story)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(4);

            // Table & Column Mappings
            this.ToTable("Dates");
            this.Property(t => t.Story).HasColumnName("Story");
            this.Property(t => t.BaringGouldStart).HasColumnName("BaringGouldStart");
            this.Property(t => t.BaringGouldEnd).HasColumnName("BaringGouldEnd");
            this.Property(t => t.Watson).HasColumnName("Watson");
            this.Property(t => t.BaringGouldPrecision).HasColumnName("BaringGouldPrecision");

            // Relationships
            this.HasRequired(t => t.Story1)
                .WithOptional(t => t.Date);

        }
    }
}
