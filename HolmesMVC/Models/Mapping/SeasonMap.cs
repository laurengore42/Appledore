using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class SeasonMap : EntityTypeConfiguration<Season>
    {
        public SeasonMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            ToTable("Seasons");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.Translation).HasColumnName("Translation");
            Property(t => t.AdaptationID).HasColumnName("Adaptation");
            Property(t => t.AirOrder).HasColumnName("AirOrder");

            // Relationships
            HasRequired(t => t.Adaptation)
                .WithMany(t => t.Seasons)
                .HasForeignKey(d => d.AdaptationID);

        }
    }
}
