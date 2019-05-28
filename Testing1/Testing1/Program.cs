using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Collections;

namespace Testing1
{
    class Program
    {
        


        public static void Main()
        {
            
            //Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.Load(@"C:\Users\Paul\Desktop\Program_for_Dean\Callide_1904_AMG_750.kml");

            XmlNodeList name = doc.GetElementsByTagName("name");
            XmlNodeList coords = doc.GetElementsByTagName("coordinates");

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

            }

            // method to split coord strings into individual arrays
            string[] splitToArray(string arrayString)
            {
                split = arrayString.Split(new Char[] { ',', ' ' },
                                 StringSplitOptions.RemoveEmptyEntries);
                return split;

            }

            // now data is available to add to a swath, then loop through "swaths" in a container to prepare a "project"
            
            
            
            
            
            
            
            
            //string testCoords = coords[1].FirstChild.InnerText;

            //Console.WriteLine("New coords: " + testCoords.ToString());

            // string[] split = testCoords.Split(new Char[] { ',',' '},
                                 //StringSplitOptions.RemoveEmptyEntries);


            //Console.WriteLine("Lat: " + split[0]);
            //Console.WriteLine("Long: " + split[1]);
            //Console.WriteLine("Alt: " + split[2]);
            //Console.WriteLine("EndLat: " + split[3]);
            //Console.WriteLine("endLong: " + split[4]);
            //Console.WriteLine("EndAlt: " + split[5]);


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
