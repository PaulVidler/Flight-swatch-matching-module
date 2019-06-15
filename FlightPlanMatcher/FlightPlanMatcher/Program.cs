using System;
using Atlass.Riegl;
using System.Device.Location;

namespace FlightPlanMatcher
{
    class Program
    {
        static void Main(string[] args)
        {

            KMLParser parser = new KMLParser();

            PlannedFlightProject plannedFlightProject = parser.ParseKMLFile();

            ActualFlightProject actualFlightProject = RPPParser.AddSwathsFromRPP();
            

            foreach (var swath in actualFlightProject.ActualSwathList)
            {
                Console.WriteLine("Actual Order = " + swath.ActualOrder);
                Console.WriteLine("Planned order = " + swath.PlannedOrder);
                
            }

            GeoCoordinate newGeo = new GeoCoordinate(89.4455, 123.4455);
            GeoCoordinate newGeo1 = new GeoCoordinate(89.4454, 123.4454);

            Console.WriteLine("Distance between 1 and 2: " + newGeo.GetDistanceTo(newGeo1));

            Console.WriteLine("Total planned flights: " + plannedFlightProject.totalPlannedSwaths());

           

        }
    }
}
