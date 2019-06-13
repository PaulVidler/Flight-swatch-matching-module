using System;
using System.Collections.Generic;
using System.Text;
using Atlass.Riegl;

namespace FlightPlanMatcher
{
    class RPPParser
    {
        public static ActualFlightProject AddSwathsFromRPP()
        {

            string rppPath = @"C:\Users\Paul\Documents\GitHub\Flight-swatch-matching-module\Callide_VQ780_190226.rpp";

            RiProcessProjectXmlParser project = new RiProcessProjectXmlParser();

            var obj = project.Open(rppPath);

            ActualFlightProject newProject = new ActualFlightProject();

            newProject.ProjectName = obj.Name;

            foreach (var swath  in obj.LidarData)
            {
                ActualSwath actualSwath = new ActualSwath();
                
                actualSwath.StartLat = swath.StartLatitude;
                actualSwath.StartLong = swath.StartLongitude;
                actualSwath.EndLat = swath.EndLatitude;
                actualSwath.EndLong = swath.EndLongitude;
                actualSwath.sensor = swath.LaserConfig;
                actualSwath.ActualOrder = swath.OrderFlown;
                actualSwath.Altitude = swath.StartAltitude;
                actualSwath.PlannedOrder = swath.OrderPlanned;

                newProject.AddSwath(actualSwath);

            }

            return newProject;

        }
    }

}
