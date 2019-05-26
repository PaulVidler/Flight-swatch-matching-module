using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace FlightPlanMatcher
{

    // will give data to PlannedFlight before being matched to a Project object

    class KMLParser
    {
        public static void ParseKMLFile(string kmlFile)
        {
            //Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.Load(kmlFile);

            PlannedSwath plannedSwath = new PlannedSwath();
            PlannedFlight plannedFlight = new PlannedFlight();


            XmlNodeList name = doc.GetElementsByTagName("name");
            for (int i = 0; i < name.Count; i++)
            {
                if (name[i].InnerText.Length > 0)
                {
                    if (name[i].InnerText.Contains("Run"))
                    {

                        Console.WriteLine(name[i].FirstChild.InnerText);
                    }

                }
            }

            XmlNodeList coords = doc.GetElementsByTagName("coordinates");
            for (int i = 0; i < coords.Count; i++)
            {
                if (coords[i].InnerText.Length > 0)
                {
                    Console.WriteLine(coords[i].FirstChild.InnerText);

                }
            }

            string testCoords = coords[1].FirstChild.InnerText;

            Console.WriteLine("New coords: " + testCoords.ToString());

            string[] split = testCoords.Split(new Char[] { ',', ' ' },
                                 StringSplitOptions.RemoveEmptyEntries);


            Console.WriteLine("Lat: " + split[0]);
            Console.WriteLine("Long: " + split[1]);
            Console.WriteLine("Alt: " + split[2]);
            Console.WriteLine("EndLat: " + split[3]);
            Console.WriteLine("endLong: " + split[4]);
            Console.WriteLine("EndAlt: " + split[5]);

        }
    }
}
