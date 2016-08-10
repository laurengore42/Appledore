using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using Newtonsoft.Json;

namespace HolmesMVC
{
    public class GoogleGeocode
    {

        public class Response
        {
            public List<ResponseResults> Results;
            public String Status;
        }

        public class ResponseResults
        {
            public List<AddressComponent> Address_Components;
            public String Formatted_Address;
            public Geometry Geometry;
            public String Place_ID;
            public List<string> Types;
        }

        public class AddressComponent
        {
            public String Long_Name;
            public String Short_Name;
            public List<string> Types;
        }

        public class Geometry
        {
            public Bounds Bounds;
            public LatLng Location;
            public String Location_Type;
            public Bounds Viewport;
        }

        public class Bounds
        {
            public LatLng Northeast;
            public LatLng Southwest;
        }

        public class LatLng
        {
            public String Lat;
            public String Lng;
        }

        public class GeocodeResponse
        {
            public Int16 ErrorCode = 0;
            public String ErrorMessage;
            public LatLng Position;
        }

        public static GeocodeResponse Geocode(string Key, string Location)
        {

            GeocodeResponse response = new GeocodeResponse();
            try
            {
                string targetURI;

                if (string.IsNullOrEmpty(Key))
                {
                    throw new ArgumentException("Geocode stopped because no key was provided", "Key");
                }
                if (string.IsNullOrEmpty(Location))
                {
                    throw new ArgumentException("Geocode stopped because no address was provided", "Location");
                }

                //Build the url
                targetURI = "https://maps.googleapis.com/maps/api/geocode/json?";
                targetURI += "key=" + HttpUtility.UrlEncode(Key);
                targetURI += "&address=" + HttpUtility.UrlEncode(Location);

                //Get the response
                string jsonStr = "";
                HttpWebRequest fr = (HttpWebRequest)(HttpWebRequest.Create(targetURI));
                HttpWebResponse respJson = (HttpWebResponse)fr.GetResponse();
                if (respJson.StatusCode.ToString() == "OK")
                {
                    using (System.IO.StreamReader str = new System.IO.StreamReader(respJson.GetResponseStream()))
                    {
                        jsonStr = str.ReadToEnd();
                    }
                }
                else
                {
                    throw new Exception("Geocode request returned StatusCode " + respJson.StatusCode.ToString());
                }

                //Read the JSON
                Response respObject = new Response();
                respObject = (Response)JsonConvert.DeserializeObject(jsonStr, respObject.GetType());

                //Return the LatLng
                if (respObject.Results != null && respObject.Results.Any())
                {
                    response.Position = respObject.Results.First().Geometry.Location;
                }
                else
                {
                    throw new Exception("Geocode response contained no Results objects");
                }
            }
            catch (Exception ex)
            {
                response.ErrorCode = 1;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
    }
}