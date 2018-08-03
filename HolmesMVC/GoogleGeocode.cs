using System;
using System.Linq;
using System.Web;
using System.Net;
using Newtonsoft.Json;

namespace HolmesMVC
{
    public static partial class GoogleGeocode
    {

        public static GeocodeResponse Geocode(string Key, string Location)
        {

            GeocodeResponse response = new GeocodeResponse();
            try
            {
                string targetURI;

                if (string.IsNullOrEmpty(Key))
                {
                    throw new ArgumentException("Geocode stopped because no key was provided", nameof(Key));
                }
                if (string.IsNullOrEmpty(Location))
                {
                    throw new ArgumentException("Geocode stopped because no address was provided", nameof(Location));
                }

                //Build the url
                targetURI = "https://maps.googleapis.com/maps/api/geocode/json?";
                targetURI += "key=" + HttpUtility.UrlEncode(Key);
                targetURI += "&address=" + HttpUtility.UrlEncode(Location);

                //Get the response
                string jsonStr = "";
                HttpWebRequest fr = (HttpWebRequest)(WebRequest.Create(targetURI));
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