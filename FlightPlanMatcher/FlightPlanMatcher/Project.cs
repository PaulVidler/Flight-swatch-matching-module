using System;
using System.Collections.Generic;
using System.Text;

namespace FlightPlanMatcher
{
    
    // will watch actual flight data with planned flight data

    class Project
    {
        // method to match actual flight objects to planned flight lines

        public void MatchPairs(ActualFlightProject actualFlightProject, PlannedFlightProject plannedFlightProject)
        {
            // need to recieve and compare an "ActualFlightProject" and "PlannedFlightProject" objects. Loop through the swaths in each and
            // compare based on start/finish lat and long of each swath. Some swaths may be flown in the wrong direction, some swaths may
            // be imcomplete or even in 2 halves. Try to be careful how this one is approached. 
            // You're a surveyor, geometry is your wheel house, you've got this.....

        }

    }
}
