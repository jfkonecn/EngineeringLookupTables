using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringLookupTables.PVTTable.SteamTableHelpers
{
    internal class Region5Factory : VaporEntryFactoryBase
    {
        public Region5Factory(double temperature, double pressure, double criticalTemperature, double criticalPressure) 
            : base(0, temperature, pressure, criticalTemperature, criticalPressure)
        {
            Props.Pi = pressure / 1.0e6;
            Props.Tau = 1000 / temperature;
        }

        protected override RegionCoefficients[] BuildIdealCoefficients()
        {           

            return new RegionCoefficients[]
            {
                new RegionCoefficients(0,   -1.3179983674201E+01),
                new RegionCoefficients(1,   6.8540841634434E+00),
                new RegionCoefficients(-3,  -2.4805148933466E-02),
                new RegionCoefficients(-2,  3.6901534980333E-01),
                new RegionCoefficients(-1,  -3.1161318213925E+00),
                new RegionCoefficients(2,   -3.2961626538917E-01)
            };
        }

        protected override RegionCoefficients[] BuildResidualCoefficients()
        {
            return new RegionCoefficients[]
            {
                new RegionCoefficients(1,   1,   1.5736404855259E-03),
                new RegionCoefficients(1,   2,   9.0153761673944E-04),
                new RegionCoefficients(1,   3,   -5.0270077677648E-03),
                new RegionCoefficients(2,   3,   2.2440037409485E-06),
                new RegionCoefficients(2,   9,   -4.1163275453471E-06),
                new RegionCoefficients(3,   7,   3.7919454822955E-08)
            };
        }
    }
}
