using System;
using System.Collections.Generic;

namespace Atlass.Riegl
{
    /// <summary>
    /// A RiProcess Project, also known as an "RPP File".
    /// </summary>
    public class RiProcessProject
    {
        /// <summary>
        /// Expected coordinate system for RPP files.
        /// If there is a different specified coordinate system in the file, an exception will be thrown.
        /// </summary>
        public const string EXPECTED_COORDINATE_SYSTEM = "WGS84";

        /// <summary>
        /// Name of the project, e.g. "Eyre_Peninsula_VQ780_180314"
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Format version of the project file. E.g. 1.4.2
        /// </summary>
        public string DocVersion { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Path to the project file.
        /// </summary>
        public string RppFilePath { get; set; }

        /// <summary>
        /// Drive that the project file came from, e.g. "C:\"
        /// </summary>
        public string Drive { get; set; }

        /// <summary>
        /// Label of the drive that the project file came from.
        /// </summary>
        public string TransitDriveReference { get; set; }

        /// <summary>
        /// Swaths, flight runs, laser data.
        /// </summary>
        public List<Swath> LidarData { get; set; } = new List<Swath>();

        /// <summary>
        /// All files referenced by the project.
        /// </summary>
        public List<RiProcessFile> Files { get; set; } = new List<RiProcessFile>();

        public override string ToString()
        {
            return Name;
        }
    }
}
