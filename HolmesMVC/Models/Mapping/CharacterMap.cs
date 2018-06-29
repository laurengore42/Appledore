using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class CharacterMap : EntityTypeConfiguration<Character>
    {
        public CharacterMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Forename)
                .HasMaxLength(50);

            this.Property(t => t.Surname)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Wikipedia)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("Characters");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Forename).HasColumnName("Forename");
            this.Property(t => t.Surname).HasColumnName("Surname");
            this.Property(t => t.Gender).HasColumnName("Gender");
            this.Property(t => t.Species).HasColumnName("Species");
            this.Property(t => t.Honorific).HasColumnName("Honorific");
            this.Property(t => t.Wikipedia).HasColumnName("Wikipedia");

            // Relationships
            this.HasOptional(t => t.Gender1)
                .WithMany(t => t.Characters)
                .HasForeignKey(d => d.Gender);
            this.HasOptional(t => t.Honorific1)
                .WithMany(t => t.Characters)
                .HasForeignKey(d => d.Honorific);
            this.HasOptional(t => t.Species1)
                .WithMany(t => t.Characters)
                .HasForeignKey(d => d.Species);

        }
    }
}
