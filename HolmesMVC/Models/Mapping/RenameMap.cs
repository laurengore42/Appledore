using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class RenameMap : EntityTypeConfiguration<Rename>
    {
        public RenameMap()
        {
            // Primary Key
            this.HasKey(t => new { t.AdaptationID, t.CharacterID, t.ActorID });

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.AdaptationID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CharacterID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ActorID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Forename)
                .HasMaxLength(50);

            this.Property(t => t.Surname)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Renames");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.AdaptationID).HasColumnName("Adaptation");
            this.Property(t => t.CharacterID).HasColumnName("Character");
            this.Property(t => t.ActorID).HasColumnName("Actor");
            this.Property(t => t.HonorificID).HasColumnName("Honorific");
            this.Property(t => t.Forename).HasColumnName("Forename");
            this.Property(t => t.Surname).HasColumnName("Surname");

            // Relationships
            this.HasRequired(t => t.Actor)
                .WithMany(t => t.Renames)
                .HasForeignKey(d => d.ActorID);
            this.HasRequired(t => t.Adaptation)
                .WithMany(t => t.Renames)
                .HasForeignKey(d => d.AdaptationID);
            this.HasRequired(t => t.Character)
                .WithMany(t => t.Renames)
                .HasForeignKey(d => d.CharacterID);
            this.HasOptional(t => t.Honorific)
                .WithMany(t => t.Renames)
                .HasForeignKey(d => d.HonorificID);

        }
    }
}
