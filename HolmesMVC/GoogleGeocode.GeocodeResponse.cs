using System;

namespace HolmesMVC
{
    public partial class GoogleGeocode
    {
        public class GeocodeResponse
        {
            public Int16 ErrorCode = 0;
            public String ErrorMessage;
            public LatLng Position;
        }
    }
}