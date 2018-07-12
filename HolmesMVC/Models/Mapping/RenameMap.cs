using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class RenameMap : EntityTypeConfiguration<Rename>
    {
        public RenameMap()
        {
            // Primary Key
            HasKey(t => new { t.AdaptationID, t.CharacterID, t.ActorID });

            // Properties
            Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.AdaptationID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.CharacterID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.ActorID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Forename)
                .HasMaxLength(50);

            Property(t => t.Surname)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("Renames");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.AdaptationID).HasColumnName("Adaptation");
            Property(t => t.CharacterID).HasColumnName("Character");
            Property(t => t.ActorID).HasColumnName("Actor");
            Property(t => t.HonorificID).HasColumnName("Honorific");
            Property(t => t.Forename).HasColumnName("Forename");
            Property(t => t.Surname).HasColumnName("Surname");

            // Relationships
            HasRequired(t => t.Actor)
                .WithMany(t => t.Renames)
                .HasForeignKey(d => d.ActorID);
            HasRequired(t => t.Adaptation)
                .WithMany(t => t.Renames)
                .HasForeignKey(d => d.AdaptationID);
            HasRequired(t => t.Character)
                .WithMany(t => t.Renames)
                .HasForeignKey(d => d.CharacterID);
            HasOptional(t => t.Honorific)
                .WithMany(t => t.Renames)
                .HasForeignKey(d => d.HonorificID);

        }
    }
}
