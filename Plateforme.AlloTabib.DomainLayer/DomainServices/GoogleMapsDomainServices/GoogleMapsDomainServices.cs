using System;
using System.Linq;
using System.Net;
using System.Xml.Linq;
//using Google.Api.Maps.Service.Geocoding;


namespace Plateforme.AlloTabib.DomainLayer.DomainServices.GoogleMapsDomainServices
{
    public class GoogleMapsDomainServices
    {
        private static string baseUri = "http://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&sensor=false";
        private string location = string.Empty;

        public static void RetrieveFormatedAddress(string lat, string lng)
        {
            string requestUri = string.Format(baseUri, lat, lng);

            using (WebClient wc = new WebClient())
            {
                string result = wc.DownloadString(requestUri);
                var xmlElm = XElement.Parse(result);
                var status = (from elm in xmlElm.Descendants()
                              where
                                  elm.Name == "status"
                              select elm).FirstOrDefault();
                if (status.Value.ToLower() == "ok")
                {
                    var res = (from elm in xmlElm.Descendants()
                               where
                                   elm.Name == "formatted_address"
                               select elm).FirstOrDefault();
                    requestUri = res.Value;
                }
            }
        }

        public static Coordinate GetCoordinates(string region)
        {
            using (var client = new WebClient())
            {

                string uri = "http://maps.google.com/maps/geo?q='" + region +
                  "'&output=csv&key=ABQIAAAAzr2EBOXUKnm_jVnk0OJI7xSosDVG8KKPE1" +
                  "-m51RBrvYughuyMxQ-i1QfUnH94QxWIa6N4U6MouMmBA";

                string[] geocodeInfo = client.DownloadString(uri).Split(',');

                return new Coordinate(Convert.ToDouble(geocodeInfo[2]),
                           Convert.ToDouble(geocodeInfo[3]));
            }
        }

        public struct Coordinate
        {
            private double lat;
            private double lng;

            public Coordinate(double latitude, double longitude)
            {
                lat = latitude;
                lng = longitude;

            }

            public double Latitude { get { return lat; } set { lat = value; } }
            public double Longitude { get { return lng; } set { lng = value; } }

        }

        public void Geo()
        {
            //var request = new GeocodingRequest();
            //request.Address = "1600 Amphitheatre Parkway, Mountain View, CA";
            //var response = GeocodingService.GetResponse(request);

            //var result = response.Results.First();

            //Console.WriteLine("Full Address: " + result.FormattedAddress);         // "1600 Amphitheatre Pkwy, Mountain View, CA 94043, USA"
            //Console.WriteLine("Latitude: " + result.Geometry.Location.Latitude);   // 37.4230180
            //Console.WriteLine("Longitude: " + result.Geometry.Location.Longitude); // -122.0818530
        }
    }


 
}
