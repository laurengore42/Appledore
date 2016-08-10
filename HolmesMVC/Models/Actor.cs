using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HolmesMVC.Models
{
    public partial class Actor
    {
        public Actor()
        {
            this.Appearances = new List<Appearance>();
            this.Renames = new List<Rename>();
        }

        public int ID { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public Nullable<System.DateTime> Birthdate { get; set; }
        public Nullable<System.DateTime> Deathdate { get; set; }
        public string Pic { get; set; }
        public string PicCredit { get; set; }
        public string Middlenames { get; set; }
        public Nullable<int> Gender { get; set; }
        public Nullable<int> Species { get; set; }
        public string IMDb { get; set; }
        public string IMDbName { get; set; }
        public string Wikipedia { get; set; }
        public string Birthplace { get; set; }
        public string SyncedBirthplace { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int BirthdatePrecision { get; set; }
        public int DeathdatePrecision { get; set; }
        public virtual Gender Gender1 { get; set; }
        public virtual Species Species1 { get; set; }
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
                GoogleGeocode.GeocodeResponse latlng = GoogleGeocode.Geocode(System.Configuration.ConfigurationManager.AppSettings["GoogleMapsAPIKey"], Birthplace);
                double tempLat = 0;
                double tempLng = 0;
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
                    return Pic;
                }
                else if (Pic == null || Pic == "")
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
                if (PicCredit != null && PicCredit != "")
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
    }
}
