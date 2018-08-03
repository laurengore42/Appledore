using System;
using System.Collections.Generic;

namespace HolmesMVC
{
    public partial class GoogleGeocode
    {
        public class Response
        {
            public List<ResponseResults> Results;
            public String Status;
        }
    }
}