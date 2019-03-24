using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringLookupTables.PVTTable.SteamTableHelpers
{
    internal class StandardProperties
    {
        /// <summary>
        /// In K
        /// </summary>
        public double Temperature { get; set; } = 0;
        /// <summary>
        /// In Pa
        /// </summary>
        public double Pressure { get; set; } = 0;        
        /// <summary>
        /// Inverse reduced temperature, T*/T
        /// </summary>
        public double Tau { get; set; } = 0;
        /// <summary>
        /// Reduced pressure, p/p*
        /// </summary>
        public double Pi { get; set; } = 0;
        /// <summary>
        /// Dimensionless Gibbs free energy, g/(RT)
        /// </summary>
        public double Gamma { get; set; } = 0;
        /// <summary>
        /// Derivative of gamma with respect to pi
        /// </summary>
        public double GammaPi { get; set; } = 0;
        /// <summary>
        /// Derivative of gamma with respect to pi then pi
        /// </summary>
        public double GammaPiPi { get; set; } = 0;
        /// <summary>
        /// Derivative of gamma with respect to tau
        /// </summary>
        public double GammaTau { get; set; } = 0;
        /// <summary>
        /// Derivative of gamma with respect to tau then tau
        /// </summary>
        public double GammaTauTau { get; set; } = 0;
        /// <summary>
        /// Derivative of gamma with respect to pi then tau
        /// </summary>
        public double GammaPiTau { get; set; } = 0;
    }
}
