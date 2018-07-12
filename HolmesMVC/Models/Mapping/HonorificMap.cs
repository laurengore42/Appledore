using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class HonorificMap : EntityTypeConfiguration<Honorific>
    {
        public HonorificMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            Property(t => t.Name)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("Honorifics");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.Name).HasColumnName("Name");
        }
    }
}
