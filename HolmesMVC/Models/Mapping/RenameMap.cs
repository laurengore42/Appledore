using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class RenameMap : EntityTypeConfiguration<Rename>
    {
        public RenameMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Adaptation, t.Character, t.Actor });

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Adaptation)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Character)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Actor)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Forename)
                .HasMaxLength(50);

            this.Property(t => t.Surname)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Renames");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Adaptation).HasColumnName("Adaptation");
            this.Property(t => t.Character).HasColumnName("Character");
            this.Property(t => t.Actor).HasColumnName("Actor");
            this.Property(t => t.Honorific).HasColumnName("Honorific");
            this.Property(t => t.Forename).HasColumnName("Forename");
            this.Property(t => t.Surname).HasColumnName("Surname");

            // Relationships
            this.HasRequired(t => t.Actor1)
                .WithMany(t => t.Renames)
                .HasForeignKey(d => d.Actor);
            this.HasRequired(t => t.Adaptation1)
                .WithMany(t => t.Renames)
                .HasForeignKey(d => d.Adaptation);
            this.HasRequired(t => t.Character1)
                .WithMany(t => t.Renames)
                .HasForeignKey(d => d.Character);
            this.HasOptional(t => t.Honorific1)
                .WithMany(t => t.Renames)
                .HasForeignKey(d => d.Honorific);

        }
    }
}
