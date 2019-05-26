using System;
using System.Collections.Generic;
using System.Text;

namespace Atlass.Riegl
{
    /// <summary>
    /// A swath, sometimes called "flight run".
    /// A single "line" in a flight, starting at a given point, ending at another point,
    /// and with collected data laser data stored in a referenced RXP file.
    /// </summary>
    public class Swath
    {
        /// <summary>
        /// E.g. 180313_231654_Scanner_1
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// E.g. Record001_Line1
        /// </summary>
        public string RecordName { get; set; }

        /// <summary>
        /// The order in which this swath was flown in the project, starting at 1.
        /// </summary>
        public int OrderFlown { get; set; }

        /// <summary>
        /// The order of this line in the original flight plan, starting at 1.
        /// </summary>
        public int? OrderPlanned { get; set; }

        /// <summary>
        /// Location of related RXP file.
        /// </summary>
        public string RxpFilePath { get; set; }

        /// <summary>
        /// Size of record, in bytes.
        /// Notes that this is typically LESS than the size of the RXP file, for reasons unclear (maybe file padding?).
        /// </summary>
        public long RecordSize { get; set; }

        public DateTime StartTime { get; set; }

        /// <summary>
        /// Ground speed at start, in m/s.
        /// </summary>
        public decimal StartGroundSpeed { get; set; }

        /// <summary>
        /// Starting latitude degrees.
        /// </summary>
        public decimal StartLatitude { get; set; }

        /// <summary>
        /// Starting longitude degrees.
        /// </summary>
        public decimal StartLongitude { get; set; }

        /// <summary>
        /// Altitude at start, in metres.
        /// </summary>
        public decimal StartAltitude { get; set; }

        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Ground speed at end, in m/s.
        /// </summary>
        public decimal? EndGroundSpeed { get; set; }

        /// <summary>
        /// End latitude degrees.
        /// </summary>
        public decimal? EndLatitude { get; set; }

        /// <summary>
        /// End longitude degrees.
        /// </summary>
        public decimal? EndLongitude { get; set; }

        /// <summary>
        /// Altitude at end, in metres.
        /// </summary>
        public decimal? EndAltitude { get; set; }

        /// <summary>
        /// The laser configuration used for this swath.
        /// </summary>
        public LaserConfiguration LaserConfig { get; set; }

        public override string ToString()
        {
            return $"{Name} {RecordName}";
        }
    }
}
