using System;
using System.Collections.Generic;
using HolmesMVC.GoogleGeocode;

namespace HolmesMVC.Models
{
    public partial class Actor
    {
        public Actor()
        {
            HolmesLinkActors = new List<HolmesLinkActor>();
            Appearances = new List<Appearance>();
            Renames = new List<Rename>();
        }

        public int ID { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public DateTime? Birthdate { get; set; }
        public DateTime? Deathdate { get; set; }
        public string Pic { get; set; }
        public string PicCredit { get; set; }
        public string Middlenames { get; set; }
        public int? SpeciesID { get; set; }
        public string IMDb { get; set; }
        public string IMDbName { get; set; }
        public string Wikipedia { get; set; }
        public string Birthplace { get; set; }
        public string SyncedBirthplace { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int BirthdatePrecision { get; set; }
        public int DeathdatePrecision { get; set; }
        public string UrlName { get; set; }
        public virtual Species Species { get; set; }
        public virtual ICollection<HolmesLinkActor> HolmesLinkActors { get; set; }
        public virtual ICollection<Appearance> Appearances { get; set; }
        public virtual ICollection<Rename> Renames { get; set; }

        public string SyncBirthplace()
        {
            if (Birthplace == SyncedBirthplace)
            {
                return "Success";
            }

            try
            {
                Latitude = 0;
                Longitude = 0;
                GeocodeResponse latlng = Geocoder.Geocode(System.Configuration.ConfigurationManager.AppSettings["GoogleMapsAPIKey"], Birthplace);
#pragma warning disable IDE0018 // Inline variable declaration
                double tempLat = 0;
                double tempLng = 0;
#pragma warning restore IDE0018 // Inline variable declaration
                if (latlng.ErrorCode == 0 && Double.TryParse(latlng.Position.Lat, out tempLat) && Double.TryParse(latlng.Position.Lng, out tempLng))
                {
                    Latitude = tempLat;
                    Longitude = tempLng;
                    SyncedBirthplace = Birthplace;
                }
                else
                {
                    return "Geocoding failed: " + latlng.ErrorMessage;
                }
            } catch (Exception ex) 
            {
                return ex.Message;
            }

            return "Success";
        }

        public string PicShow
        {
            get
            {
                if (Pic != null && Pic.IndexOf('.') > -1)
                {
                    return Pic.Replace("http:","https:");
                }
                else if (string.IsNullOrEmpty(Pic))
                {
                    return null;
                }
                else
                {
                    return "/Content/ActorPhotos/" + Pic + ".jpg";
                }
            }
        }
        public string PicCreditShow
        {
            get
            {
                if (!string.IsNullOrEmpty(PicCredit))
                {
                    return "&copy; " + PicCredit;
                }
                else if (Pic != null && Pic.IndexOf('.') == -1)
                {
                    return "image credit: " + "unknown - can you help?";
                }
                else
                {
                    return null;
                }
            }
        }

        public int? AgeInYears
        {
            get
            {
                var birth = Birthdate;
                if (birth == null)
                {
                    return null;
                }

                var bday = (DateTime)birth;
                var dday = Deathdate ?? DateTime.Now;
                var age = dday.Year - bday.Year;
                if (dday.DayOfYear < bday.DayOfYear)
                {
                    age--;
                }

                return age;
            }
        }

    }
}
