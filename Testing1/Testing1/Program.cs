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
            PlannedFlight flight = new PlannedFlight();

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

            Console.WriteLine(flight.totalPlannedSwaths());

        }
    }


    class PlannedSwath
    {
        public double StartLat { get; set; }
        public double StartLong { get; set; }
        public double EndLat { get; set; }
        public double EndLong { get; set; }
        // flight run number according to KML flight plan
        public string PlannedOrder { get; set; }
        public int PlannedAltitude { get; set; }

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
