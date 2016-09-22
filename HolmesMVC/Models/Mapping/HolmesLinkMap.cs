using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class HolmesLinkMap : EntityTypeConfiguration<HolmesLink>
    {
        public HolmesLinkMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Table & Column Mappings
            this.ToTable("HolmesLinks");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Airdate).HasColumnName("Airdate");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.AirdatePrecision).HasColumnName("AirdatePrecision");
        }
    }
}
