using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class HolmesLinkActorMap : EntityTypeConfiguration<HolmesLinkActor>
    {
        public HolmesLinkActorMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            ToTable("HolmesLinkActors");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.ActorID).HasColumnName("Actor");
            Property(t => t.Name).HasColumnName("Name");

            // Relationships
            HasOptional(t => t.Actor)
                .WithMany(t => t.HolmesLinkActors)
                .HasForeignKey(d => d.ActorID);

        }
    }
}
