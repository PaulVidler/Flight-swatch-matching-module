using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Collections;

namespace FlightPlanMatcher
{

    // will give data to PlannedFlight before being matched to a Project object

    class KMLParser
    {
        //public static void ParseKMLFile(string kmlFile)
        public PlannedFlightProject ParseKMLFile()
        {

            //Create the XmlDocument.
            XmlDocument doc = new XmlDocument();

            // KML variable to be passed into method
            //doc.Load(kmlFile);


            // hard coded KML location
            doc.Load(@"C:\Users\Paul\Documents\GitHub\Flight-swatch-matching-module\Callide_Bhill_1902_AMG_1000.kml");

            XmlNodeList name = doc.GetElementsByTagName("name");
            XmlNodeList coords = doc.GetElementsByTagName("coordinates");

            // yeah yeah, shouldn't have used an array list, I know....
            ArrayList coordStrings = new ArrayList();
            ArrayList runNumber = new ArrayList();
            ArrayList startLat = new ArrayList();
            ArrayList startLong = new ArrayList();
            ArrayList endLat = new ArrayList();
            ArrayList endLong = new ArrayList();
            ArrayList altitude = new ArrayList();

            string[] split;


            // loop through XML list of "runs" and add to ordered list
            for (int i = 0; i < name.Count; i++)
            {
                if (name[i].InnerText.Length > 0)
                {
                    if (name[i].InnerText.Contains("Run"))
                    {
                        runNumber.Add(name[i].FirstChild.InnerText);
                    }

                }
            }

            // loop through XML list of "coords" and add to ordered list
            for (int i = 0; i < coords.Count; i++)
            {
                if (coords[i].InnerText.Length > 0)
                {
                    coordStrings.Add(coords[i].FirstChild.InnerText);

                }
            }

            // loop through coords string and split to an array. Returns the array "split" of coords in particular order.
            foreach (var item in coordStrings)
            {
                splitToArray(item.ToString());
                startLong.Add(split[0]);
                startLat.Add(split[1]);
                altitude.Add(split[2]);
                endLong.Add(split[3]);
                endLat.Add(split[4]);

            }

            // method to split coord strings into individual arrays (Used in loop that splits coord string into individual coords etc)
            string[] splitToArray(string arrayString)
            {
                split = arrayString.Split(new Char[] { ',', ' ' },
                                 StringSplitOptions.RemoveEmptyEntries);

                return split;

            }


            //new planned flight object used to add swaths to.
            PlannedFlightProject flight = new PlannedFlightProject();

            // counter for loop to add details to swaths
            int counter = 0;

            foreach (var i in runNumber)
            {
                PlannedSwath swath = new PlannedSwath();

                swath.StartLat = Convert.ToDouble(startLat[counter]);
                swath.StartLong = Convert.ToDouble(startLong[counter]);
                swath.EndLat = Convert.ToDouble(endLat[counter]);
                swath.EndLong = Convert.ToDouble(endLong[counter]);
                swath.PlannedOrder = (string)i;
                swath.PlannedAltitude = Convert.ToInt32(altitude[counter]);

                flight.AddSwath(swath);

                counter++;
            }

            return flight;

        }




    }
    
}
