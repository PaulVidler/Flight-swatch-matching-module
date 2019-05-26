using System;
using System.Collections.Generic;
using System.Text;

namespace FlightPlanMatcher
{
    
    class PlannedFlight
    {

        public string ProjectName { get; set; }
        public List<PlannedSwath> PlannedSwathList = new List<PlannedSwath>();


        public void AddSwath(PlannedSwath swath)
        {
            PlannedSwathList.Add(swath);
        }

        public int totalPlannedSwaths()
        {
            return PlannedSwathList.Count;
        }

    }

}
