using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringLookupTables.PVTTable.SteamTableHelpers
{
    internal class Region3Properties
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
        /// Dimensionless Helmholtz free energy, f/(RT)
        /// </summary>
        public double Phi { get; set; } = 0;
        /// <summary>
        /// Reduced density, rho/rho*
        /// </summary>
        public double Delta { get; set; } = 0;
        /// <summary>
        /// Derivative of phi with respect to delta
        /// </summary>
        public double PhiDelta { get; set; } = 0;
        /// <summary>
        /// Derivative of phi with respect to delta then delta
        /// </summary>
        public double PhiDeltaDelta { get; set; } = 0;
        /// <summary>
        /// Derivative of phi with respect to tau
        /// </summary>
        public double PhiTau { get; set; } = 0;
        /// <summary>
        /// Derivative of phi with respect to tau then tau
        /// </summary>
        public double PhiTauTau { get; set; } = 0;
        /// <summary>
        /// Derivative of phi with respect to delta then tau
        /// </summary>
        public double PhiDeltaTau { get; set; } = 0;
        public double Density { get; set; } = 0;
    }
}
