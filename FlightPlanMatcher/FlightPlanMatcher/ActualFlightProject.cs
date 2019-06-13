using System;
using System.Collections.Generic;
using System.Text;

namespace FlightPlanMatcher
{
    class ActualFlightProject
    {
        public string ProjectName { get; set; }

        public List<ActualSwath> ActualSwathList = new List<ActualSwath>();


        public void AddSwath(ActualSwath actSwath)
        {
            ActualSwathList.Add(actSwath);
        }

        public int totalActualSwaths()
        {
            return ActualSwathList.Count;
        }

    }
}
