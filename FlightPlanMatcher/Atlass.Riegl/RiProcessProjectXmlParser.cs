using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Atlass.Riegl
{
    /// <summary>
    /// Parse / convert RPP files in XML format, into a RiProcessProject object.
    /// </summary>
    public class RiProcessProjectXmlParser
    {
        public RiProcessProject Open(string path)
        {
            var riProject = new RiProcessProject();

            riProject.RppFilePath = path;

            string pathRoot = Path.GetPathRoot(path);
            var drives = DriveInfo.GetDrives();
            var driveForPath = drives.FirstOrDefault(drv => drv.RootDirectory.Name.ToLower() == pathRoot.ToLower());

            if (driveForPath == null)
            {
                throw new Exception($"Could not find drive info for path: {path}");
            }

            riProject.Drive = driveForPath.Name;
            riProject.TransitDriveReference = driveForPath.VolumeLabel;


            var document = XDocument.Load(path);

            var docHeader = document.XPathSelectElement("document/header");
            riProject.DocVersion = docHeader.XPathSelectElement("docinfo/version").Attribute("data").Value;
            riProject.CreatedDate = GetDateValue(docHeader, "docinfo/created");
            riProject.ModifiedDate = GetDateValue(docHeader, "docinfo/modified");

            var docContent = document.XPathSelectElement("document/content");

            var projects = docContent.Elements("object").Where(e => e.Attribute("kind").Value == "project");

            if (projects.Count() == 0)
            {
                throw new Exception($"Could not find any project entries in file: {path}");
            }
            else if (projects.Count() > 1)
            {
                throw new Exception($"Found multiple project entries ({projects.Count()}) in file: {path}");
            }

            var project = projects.First();

            riProject.Name = project.Attribute("name").Value;

            var coordSystem = project.XPathSelectElement("fields/field[@name='coordinate-system-internal']");
            if (coordSystem != null)
            {
                var coordSystemValue = coordSystem.Attribute("data").Value;

                if (!coordSystemValue.Equals(RiProcessProject.EXPECTED_COORDINATE_SYSTEM, StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception($"Unexpected coordinate system in project. Expected '{RiProcessProject.EXPECTED_COORDINATE_SYSTEM}', found '{coordSystemValue}'. File: {path}");
                }
            }

            riProject.Files = ReadFiles(project);
            riProject.LidarData = ReadFlightRuns(project, path);

            return riProject;
        }

        private List<RiProcessFile> ReadFiles(XElement project)
        {
            var fileReferences = project.XPathSelectElements("//files/file");
            var files = new List<RiProcessFile>();

            foreach (var file in fileReferences)
            {
                files.Add(new RiProcessFile()
                {
                    Id = file.Attribute("id").Value,
                    Path = TranslateFilePath(file, project)
                });
            }

            return files;
        }

        private string TranslateFilePath(XElement file, XElement project)
        {
            var filePath = file.Attribute("path").Value + file.Attribute("name").Value + file.Attribute("ext").Value;

            if (filePath.StartsWith(".\\"))
            {
                filePath = filePath.Substring(2);
            }

            if (Path.IsPathRooted(filePath))
            {
                // Usually, file references are relative to the RPP file, but we've seen the odd instance of
                // them being rooted / fully qualified. E.g. G:\Tenthill_VQ780_180512-2\02_INS-GPS_RAW\01_MON\INS-GPS_1\180512_032307_INS-GPS_1.mon.igs
                // In that case, we need to try translate back to a relative path.

                var lastPathElement = project.XPathSelectElement("//field[@name='last-path']");

                if (lastPathElement == null)
                {
                    throw new Exception($"Failed to read RPP file. Found a fully qualified file path when expecting a relative one: {filePath}");
                }

                var lastPath = lastPathElement.Attribute("data").Value;

                if (filePath.StartsWith(lastPath, StringComparison.OrdinalIgnoreCase))
                {
                    filePath = filePath.Substring(lastPath.Length);
                    if (filePath.StartsWith("\\"))
                    {
                        filePath = filePath.Substring(1);
                    }
                }
                else
                {
                    throw new Exception($"Failed to read RPP file. Found a fully qualified file path when expecting a relative one: {filePath}");
                }
            }

            return filePath;
        }

        private List<Swath> ReadFlightRuns(XElement project, string path)
        {
            var swaths = new List<Swath>();
            int orderFlown = 1;

            var flightRunObjects = project.XPathSelectElements("objects/object[@kind='RECORDS']/objects/object");

            foreach (var flightRunElement in flightRunObjects)
            {
                // E.g. Record001_Line8
                var recordName = flightRunElement.Attribute("name").Value;

                var lasDataElement = flightRunElement.XPathSelectElement("objects/object[@kind='lasdata']");

                if (lasDataElement == null)
                {
                    // We're not interested (for now) in flight runs without laser data
                    continue;
                }

                var lasDeviceLink = lasDataElement.XPathSelectElement("links/link[@name='lasdevice']");

                int? lineId = null;
                var fileSourceElement = lasDataElement.XPathSelectElement("fields/field[@name='file-source-id']");
                if (fileSourceElement != null)
                {
                    lineId = int.Parse(fileSourceElement.Attribute("data").Value);
                }

                var rxpFileElements = lasDataElement.XPathSelectElements("objects/object[@kind='rxp-file']");

                if (rxpFileElements.Count() == 0)
                {
                    throw new Exception($"Project file has no RXP records. {path}");
                }

                // Wouldn't expect more than one, but the schema makes it look possible, so we 'foreach'
                foreach (var rxpFileElement in rxpFileElements)
                {
                    var flightRun = new Swath();

                    flightRun.Name = lasDataElement.Attribute("name").Value;
                    flightRun.RecordName = recordName;
                    flightRun.OrderPlanned = lineId;
                    flightRun.OrderFlown = orderFlown++;

                    var lasConfigLink = rxpFileElement.XPathSelectElement("links/link[@name='lasconfig']");

                    if (lasConfigLink != null)
                    {
                        var lasNodeLink = NodeLink.Parse(lasConfigLink.Attribute("node").Value);

                        var lasConfigElement = lasNodeLink.GetElement(project);

                        if (lasConfigElement != null)
                        {
                            var lasConfig = new LaserConfiguration();

                            if (lasConfigElement.Attribute("kind").Value != "lasconfig")
                            {
                                throw new Exception($"Unepxected laser configuration object kind. Expected 'lasconfig', received '{lasConfigElement.Attribute("kind").Value}'.");
                            }

                            var lasConfigRange = lasConfigElement.XPathSelectElement("fields/field[@name='range']");
                            var lasConfigUnit = lasConfigRange.Attribute("unit")?.Value;
                            if (lasConfigUnit != null && lasConfigUnit != "m")
                            {
                                throw new Exception($"Unexpected laser config range unit. Expected 'm', received '{lasConfigUnit}'.");
                            }

                            lasConfig.Name = lasConfigElement.Attribute("name").Value;
                            lasConfig.Range = decimal.Parse(lasConfigRange.Attribute("data").Value);
                            lasConfig.Speed = decimal.Parse(lasConfigElement.XPathSelectElement("fields/field[@name='speed']").Attribute("data").Value);
                            lasConfig.DeviceType = lasConfigElement.XPathSelectElement("fields/field[@name='device-type']").Attribute("data").Value;
                            lasConfig.DeviceSerialNumber = lasConfigElement.XPathSelectElement("fields/field[@name='device-serial']").Attribute("data").Value;

                            flightRun.LaserConfig = lasConfig;
                        }
                    }

                    if (flightRun.LaserConfig == null)
                    {
                        throw new Exception($"Could not find laser config data for flight line {flightRun.Name}");
                    }


                    flightRun.RecordSize = long.Parse(rxpFileElement.XPathSelectElement("fields/field[@name='size']").Attribute("data").Value);

                    flightRun.StartTime = GetDateValue(rxpFileElement, "fields/field[@name='time-start']");
                    flightRun.StartGroundSpeed = decimal.Parse(rxpFileElement.XPathSelectElement("fields/field[@name='ground-speed-start']").Attribute("data").Value);
                    flightRun.StartLatitude = decimal.Parse(rxpFileElement.XPathSelectElement("fields/field[@name='latitude-start']").Attribute("data").Value);
                    flightRun.StartLongitude = decimal.Parse(rxpFileElement.XPathSelectElement("fields/field[@name='longitude-start']").Attribute("data").Value);
                    flightRun.StartAltitude = decimal.Parse(rxpFileElement.XPathSelectElement("fields/field[@name='altitude-start']").Attribute("data").Value);

                    // We've seen some examples of lasdata that doesn't have all 'end' details.
                    // We're not sure what causes this, maybe an error in acquisition?

                    var endTimeElement = rxpFileElement.XPathSelectElement("fields/field[@name='time-end']");
                    var endGroundSpeedElement = rxpFileElement.XPathSelectElement("fields/field[@name='ground-speed-end']");
                    var endLatitudeElement = rxpFileElement.XPathSelectElement("fields/field[@name='latitude-end']");
                    var endLongitudeElement = rxpFileElement.XPathSelectElement("fields/field[@name='longitude-end']");
                    var endAltitudeElement = rxpFileElement.XPathSelectElement("fields/field[@name='altitude-end']");

                    if (endTimeElement != null)
                    {
                        flightRun.EndTime = ParseDateValue(endTimeElement.Attribute("data").Value);
                    }

                    if (endGroundSpeedElement != null)
                    {
                        flightRun.EndGroundSpeed = decimal.Parse(endGroundSpeedElement.Attribute("data").Value);
                    }

                    if (endLatitudeElement != null)
                    {
                        flightRun.EndLatitude = decimal.Parse(endLatitudeElement.Attribute("data").Value);
                    }

                    if (endLongitudeElement != null)
                    {
                        flightRun.EndLongitude = decimal.Parse(endLongitudeElement.Attribute("data").Value);
                    }

                    if (endAltitudeElement != null)
                    {
                        flightRun.EndAltitude = decimal.Parse(endAltitudeElement.Attribute("data").Value);
                    }

                    var rxpFileNameElement = rxpFileElement.XPathSelectElement("files/file[@item='rxp-file']");

                    if (rxpFileNameElement == null)
                    {
                        throw new Exception($"Could not find detail of RXP file name for record {recordName}");
                    }

                    flightRun.RxpFilePath = TranslateFilePath(rxpFileNameElement, project);

                    swaths.Add(flightRun);
                }
            }

            return swaths;
        }

        private DateTime GetDateValue(XElement element, string xPathExpression)
        {
            string dateValue = element.XPathSelectElement(xPathExpression).Attribute("data").Value;
            return ParseDateValue(dateValue);
        }

        private DateTime ParseDateValue(string dateValue)
        {
            // Some RPP files have an unusual date format, e.g.
            // 2018-05-12 13:20:48+399
            // The '+399' in this case isn't an offset, maybe it's milliseconds? We don't know what it is, so, we ignore it.

            if (dateValue.Contains("+"))
            {
                return DateTime.Parse(dateValue.Substring(0, dateValue.IndexOf('+')));
            }
            else
            {
                return DateTime.Parse(dateValue);
            }
        }
    }
}
