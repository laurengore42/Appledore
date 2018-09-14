
using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class ActorMap : EntityTypeConfiguration<Actor>
    {
        public ActorMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            Property(t => t.Forename)
                .HasMaxLength(50);

            Property(t => t.Surname)
                .IsRequired()
                .HasMaxLength(50);

            Property(t => t.Pic)
                .HasMaxLength(1000);

            Property(t => t.PicCredit)
                .HasMaxLength(1000);

            Property(t => t.Middlenames)
                .HasMaxLength(200);

            Property(t => t.IMDb)
                .HasMaxLength(1000);

            Property(t => t.Wikipedia)
                .HasMaxLength(1000);

            Property(t => t.Birthplace)
                .HasMaxLength(200);

            // Table & Column Mappings
            ToTable("Actors");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.Forename).HasColumnName("Forename");
            Property(t => t.Surname).HasColumnName("Surname");
            Property(t => t.Birthdate).HasColumnName("Birthdate");
            Property(t => t.Deathdate).HasColumnName("Deathdate");
            Property(t => t.Pic).HasColumnName("Pic");
            Property(t => t.PicCredit).HasColumnName("PicCredit");
            Property(t => t.Middlenames).HasColumnName("Middlenames");
            Property(t => t.SpeciesID).HasColumnName("Species");
            Property(t => t.IMDb).HasColumnName("IMDb");
            Property(t => t.Wikipedia).HasColumnName("Wikipedia");
            Property(t => t.Birthplace).HasColumnName("Birthplace");
            Property(t => t.SyncedBirthplace).HasColumnName("SyncedBirthplace");
            Property(t => t.Latitude).HasColumnName("Latitude");
            Property(t => t.Longitude).HasColumnName("Longitude");
            Property(t => t.BirthdatePrecision).HasColumnName("BirthdatePrecision");
            Property(t => t.DeathdatePrecision).HasColumnName("DeathdatePrecision");
            Property(t => t.UrlName).HasColumnName("UrlName");

            //Ignore fluff properties
            Ignore(t => t.PicShow);
            Ignore(t => t.PicCreditShow);

            // Relationships
            HasOptional(t => t.Species)
                .WithMany(t => t.Actors)
                .HasForeignKey(d => d.SpeciesID);

        }
    }
}
