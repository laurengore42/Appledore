using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class CharacterMap : EntityTypeConfiguration<Character>
    {
        public CharacterMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            Property(t => t.Forename)
                .HasMaxLength(50);

            Property(t => t.Surname)
                .IsRequired()
                .HasMaxLength(50);

            Property(t => t.Wikipedia)
                .HasMaxLength(1000);

            Property(t => t.StoryID)
                .IsFixedLength()
                .HasMaxLength(4);

            // Table & Column Mappings
            ToTable("Characters");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.Forename).HasColumnName("Forename");
            Property(t => t.Surname).HasColumnName("Surname");
            Property(t => t.GenderID).HasColumnName("Gender");
            Property(t => t.SpeciesID).HasColumnName("Species");
            Property(t => t.HonorificID).HasColumnName("Honorific");
            Property(t => t.Wikipedia).HasColumnName("Wikipedia");
            Property(t => t.StoryID).HasColumnName("Story");

            // Relationships
            HasOptional(t => t.Gender)
                .WithMany(t => t.Characters)
                .HasForeignKey(d => d.GenderID);
            HasOptional(t => t.Honorific)
                .WithMany(t => t.Characters)
                .HasForeignKey(d => d.HonorificID);
            HasOptional(t => t.Species)
                .WithMany(t => t.Characters)
                .HasForeignKey(d => d.SpeciesID);
            HasOptional(t => t.Story)
                .WithMany(t => t.Characters)
                .HasForeignKey(d => d.StoryID);

        }
    }
}
