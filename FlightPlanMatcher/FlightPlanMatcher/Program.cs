using System;
using Atlass.Riegl;

namespace FlightPlanMatcher
{
    class Program
    {
        static void Main(string[] args)
        {

            //KMLParser parser = new KMLParser();

            //parser.ParseKMLFile();

            ActualFlightProject actualFlightProject = RPPParser.AddSwathsFromRPP();

            foreach (var swath in actualFlightProject.ActualSwathList)
            {
                Console.WriteLine("Actual Order = " + swath.ActualOrder);
                Console.WriteLine("Planned order: " + swath.PlannedOrder);
            }

        }
    }
}
