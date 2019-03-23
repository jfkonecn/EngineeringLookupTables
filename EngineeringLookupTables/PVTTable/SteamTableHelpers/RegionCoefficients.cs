using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringLookupTables.PVTTable.SteamTableHelpers
{
    internal class RegionCoefficients
    {
        public RegionCoefficients(double i, double j, double n)
        {
            I = i;
            J = j;
            N = n;
        }

        public RegionCoefficients(double j, double n) : this(double.NaN, j, n)
        {
        }
        public double I { get; }
        public double J { get; }
        public double N { get; }
    }
}
