using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class ActorMap : EntityTypeConfiguration<Actor>
    {
        public ActorMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Forename)
                .HasMaxLength(50);

            this.Property(t => t.Surname)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Pic)
                .HasMaxLength(1000);

            this.Property(t => t.PicCredit)
                .HasMaxLength(1000);

            this.Property(t => t.Middlenames)
                .HasMaxLength(200);

            this.Property(t => t.IMDb)
                .HasMaxLength(1000);

            this.Property(t => t.Wikipedia)
                .HasMaxLength(1000);

            this.Property(t => t.Birthplace)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("Actors");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Forename).HasColumnName("Forename");
            this.Property(t => t.Surname).HasColumnName("Surname");
            this.Property(t => t.Birthdate).HasColumnName("Birthdate");
            this.Property(t => t.Deathdate).HasColumnName("Deathdate");
            this.Property(t => t.Pic).HasColumnName("Pic");
            this.Property(t => t.PicCredit).HasColumnName("PicCredit");
            this.Property(t => t.Middlenames).HasColumnName("Middlenames");
            this.Property(t => t.Gender).HasColumnName("Gender");
            this.Property(t => t.Species).HasColumnName("Species");
            this.Property(t => t.IMDb).HasColumnName("IMDb");
            this.Property(t => t.Wikipedia).HasColumnName("Wikipedia");
            this.Property(t => t.Birthplace).HasColumnName("Birthplace");
            this.Property(t => t.SyncedBirthplace).HasColumnName("SyncedBirthplace");
            this.Property(t => t.Latitude).HasColumnName("Latitude");
            this.Property(t => t.Longitude).HasColumnName("Longitude");
            this.Property(t => t.BirthdatePrecision).HasColumnName("BirthdatePrecision");
            this.Property(t => t.DeathdatePrecision).HasColumnName("DeathdatePrecision");

            //Ignore fluff properties
            this.Ignore(t => t.PicShow);
            this.Ignore(t => t.PicCreditShow);

            // Relationships
            this.HasOptional(t => t.Gender1)
                .WithMany(t => t.Actors)
                .HasForeignKey(d => d.Gender);
            this.HasOptional(t => t.Species1)
                .WithMany(t => t.Actors)
                .HasForeignKey(d => d.Species);

        }
    }
}
