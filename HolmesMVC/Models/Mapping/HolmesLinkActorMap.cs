using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class HolmesLinkActorMap : EntityTypeConfiguration<HolmesLinkActor>
    {
        public HolmesLinkActorMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("HolmesLinkActors");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ActorID).HasColumnName("Actor");
            this.Property(t => t.Name).HasColumnName("Name");

            // Relationships
            this.HasOptional(t => t.Actor)
                .WithMany(t => t.HolmesLinkActors)
                .HasForeignKey(d => d.ActorID);

        }
    }
}
