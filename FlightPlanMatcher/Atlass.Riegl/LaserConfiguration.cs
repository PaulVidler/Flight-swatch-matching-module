using System;
using System.Collections.Generic;
using System.Text;

namespace Atlass.Riegl
{
    public class LaserConfiguration
    {
        /// <summary>
        /// Name of the laser configuration. E.g.
        /// Ground Test
        /// or
        /// 1100m 145kn 700 kHz (100%) 165lps Scanner 1
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Range, in metres.
        /// </summary>
        public decimal Range { get; set; }

        /// <summary>
        /// Speed, in metres per second.
        /// </summary>
        public decimal Speed { get; set; }

        /// <summary>
        /// Laser device, e.g. VQ-780i
        /// </summary>
        public string DeviceType { get; set; }

        /// <summary>
        /// Serial number of laser device.
        /// </summary>
        public string DeviceSerialNumber { get; set; }
    }
}
