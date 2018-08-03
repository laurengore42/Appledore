using System;
using System.Collections.Generic;

namespace HolmesMVC
{
    public partial class GoogleGeocode
    {
        public class ResponseResults
        {
            public List<AddressComponent> Address_Components;
            public String Formatted_Address;
            public Geometry Geometry;
            public String Place_ID;
            public List<string> Types;
        }
    }
}