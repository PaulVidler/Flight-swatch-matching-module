using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace Testing1
{
    class Program
    {
        


        public static void Main()
        {
            PlannedSwath plannedSwath = new PlannedSwath();


            //Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.Load(@"C:\Users\Paul\Desktop\Program_for_Dean\Callide_1904_AMG_750.kml");

            XmlNodeList name = doc.GetElementsByTagName("name");
            XmlNodeList coords = doc.GetElementsByTagName("coordinates");

            Console.WriteLine(name.Count);
            Console.WriteLine(coords.Count);


            for (int i = 0; i < name.Count; i++)
            {
                if (name[i].InnerText.Length > 0)
                {
                    if (name[i].InnerText.Contains("Run"))
                    {
                        plannedSwath.PlannedOrder = name[i].FirstChild.InnerText;
                    }

                }
            }

            for (int i = 0; i < coords.Count; i++)
            {
                if (coords[i].InnerText.Length > 0)
                {
                    Console.WriteLine(i + ": " + coords[i].FirstChild.InnerText);

                }
            }

            string testCoords = coords[1].FirstChild.InnerText;

            Console.WriteLine("New coords: " + testCoords.ToString());

            string[] split = testCoords.Split(new Char[] { ',',' '},
                                 StringSplitOptions.RemoveEmptyEntries);


            Console.WriteLine("Lat: " + split[0]);
            Console.WriteLine("Long: " + split[1]);
            Console.WriteLine("Alt: " + split[2]);
            Console.WriteLine("EndLat: " + split[3]);
            Console.WriteLine("endLong: " + split[4]);
            Console.WriteLine("EndAlt: " + split[5]);


        }
    }


    class PlannedSwath
    {
        public float StartLat { get; set; }
        public float StartLong { get; set; }
        public float EndLat { get; set; }
        public float EndLong { get; set; }
        // flight run number according to KML flight plan
        public string PlannedOrder { get; set; }


    }

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
