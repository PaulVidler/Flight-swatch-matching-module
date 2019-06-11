using System;
using System.Collections.Generic;
using System.Text;

namespace FlightPlanMatcher
{
    class ActualFlightProject
    {
        public string ProjectName { get; set; }

        public List<IActualFlight> ActualSwathList = new List<IActualFlight>();


        public void AddSwath(ActualSwath swath)
        {
            PlannedSwathList.Add(swath);
        }

        public int totalPlannedSwaths()
        {
            return PlannedSwathList.Count;
        }

    }
}
