using System;
using System.Collections.Generic;
using System.Text;
using Atlass.Riegl;

namespace FlightPlanMatcher
{
    

    class FlightVQ780 : IActualFlight
    {
        public ICollection<RiProcessProject> Project { get; set; }
        public DateTime FlightDate { get; set; }
        public int TotalAreas { get; set; }
        public Sensor SensorUsed { get; set; } 

    }
}
