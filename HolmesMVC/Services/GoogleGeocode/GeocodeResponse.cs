﻿using System;

namespace HolmesMVC.Services.GoogleGeocode
{
    public class GeocodeResponse
    {
        public Int16 ErrorCode = 0;
        public String ErrorMessage;
        public LatLng Position;
    }
}