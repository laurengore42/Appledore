using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class StoryMap : EntityTypeConfiguration<Story>
    {
        public StoryMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            Property(t => t.ID)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(4);

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            ToTable("Stories");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.Name).HasColumnName("Name");
        }
    }
}
