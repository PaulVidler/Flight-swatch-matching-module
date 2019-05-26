using System;
using System.Collections.Generic;
using System.Text;
using Atlass.Riegl;

namespace FlightPlanMatcher
{
    public enum Sensor { VQ780H, VQ780S, H68 }

    interface IActualFlight
    {

        // collection/list of swaths
        ICollection<RiProcessProject> Project { get; set; }
        DateTime FlightDate { get; set; }
        // amount of areas in flight
        int TotalAreas { get; set; }

        
    }
}
