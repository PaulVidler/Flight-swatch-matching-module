using System;
using System.Collections.Generic;
using System.Text;

namespace FlightPlanMatcher
{
    
    class ActualSwath
    {

        public decimal StartLat { get; set; }
        public decimal StartLong { get; set; }
        public decimal? EndLat { get; set; }
        public decimal? EndLong { get; set; }

        public decimal Altitude { get; set; }
        // flight run number according to the order it was actually flown - Comes from Riegl module
        public int ActualOrder { get; set; }
        public int? PlannedOrder { get; set; }
        public Atlass.Riegl.LaserConfiguration sensor { get; set; }

    }
}
