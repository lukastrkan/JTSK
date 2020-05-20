using System;
using System.Collections.Generic;
using System.Text;

namespace JTSK.Models
{
    public class Coordinate
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Display { get; set; }

        public Coordinate(double latitude, double longitude, string display)
        {            
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Display = display;
        }
    }
}
