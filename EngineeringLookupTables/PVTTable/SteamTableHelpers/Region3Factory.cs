using EngineeringLookupTables.Common;
using EngineeringLookupTables.NumericalMethods;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringLookupTables.PVTTable.SteamTableHelpers
{
    internal class Region3Factory : IPVTEntryFactory
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="temperature">in K</param>
        /// <param name="pressure">in Pa</param>
        /// <param name="maxTemperature">in K</param>
        public Region3Factory(double temperature, double pressure, Range temperatureRange)
        {
            Props = new Region3Properties()
            {
                Pressure = pressure,
                Temperature = temperature
            };

            NewtonsMethod.Solve((x) => Region3EquationHelper(Props.Temperature, x), 
                temperatureRange);
            if (double.IsNaN(Props.Pressure))
                throw new ArgumentOutOfRangeException("Could find a point in region3 at the given temperature and pressure");
        }



        public PVTEntry BuildThermoEntry()
        {
            return new PVTEntry()
            {
                Region = Region.SupercriticalFluid,
                Pressure = Props.Pressure,
                Temperature = Props.Temperature,
                SpecificVolume = 1.0 / Props.Density,
                Density = Props.Density,
                InternalEnergy = Props.Tau * Props.PhiTau * UniversalConstants.WaterGasConstant * Props.Temperature,
                Enthalpy = (Props.Tau * Props.PhiTau + Props.Delta * Props.PhiDelta) * UniversalConstants.WaterGasConstant * Props.Temperature,
                Entropy = (Props.Tau * Props.PhiTau - Props.Phi) * UniversalConstants.WaterGasConstant,
                IsochoricHeatCapacity = -Math.Pow(Props.Tau, 2) * Props.PhiTauTau * UniversalConstants.WaterGasConstant,
                IsobaricHeatCapacity = (-Math.Pow(Props.Tau, 2) * Props.PhiTauTau
                    + Math.Pow(Props.Delta * Props.PhiDelta - Props.Delta * Props.Tau * Props.PhiDeltaTau, 2) 
                    / (2 * Props.Delta * Props.PhiDelta + Math.Pow(Props.Delta, 2) * Props.PhiDeltaDelta))
                    * UniversalConstants.WaterGasConstant,
                SpeedOfSound = Math.Sqrt((2 * Props.Delta * Props.PhiDelta + Math.Pow(Props.Delta, 2) * Props.PhiDeltaDelta -
                    Math.Pow(Props.Delta * Props.PhiDelta - Props.Delta * Props.Tau * Props.PhiDeltaTau, 2) 
                    / (Math.Pow(Props.Tau, 2) * Props.PhiTauTau)) * UniversalConstants.WaterGasConstant * Props.Temperature),
                SolidMassFraction = 0,
                LiquidMassFraction = 0,
                VaporMassFraction = 0
            };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="temperature">in K</param>
        /// <param name="density">in kg/m3</param>
        /// <returns></returns>
        private double Region3EquationHelper(double temperature, double density)
        {
            double n1 = Region3Coefficients[0].N;
            double tempDelta = density / 322;
            Props.Temperature = temperature;
            Props.Density = density;
            Props.Delta = tempDelta;
            Props.Tau = 647.096 / temperature;
            Props.Phi = n1 * Math.Log(tempDelta);
            Props.PhiDelta = n1 / tempDelta;
            Props.PhiDeltaDelta = -n1 / Math.Pow(tempDelta, 2);
            Props.PhiTau = 0;
            Props.PhiTauTau = 0;
            Props.PhiDeltaTau = 0;
            

            for (int i = 1; i < Region3Coefficients.Length; i++)
            {
                RegionCoefficients item = Region3Coefficients[i];
                Props.Phi += item.N * Math.Pow(Props.Delta, item.I) * Math.Pow(Props.Tau, item.J);
                Props.PhiDelta += item.N * item.I * Math.Pow(Props.Delta, item.I - 1) * Math.Pow(Props.Tau, item.J);
                Props.PhiDeltaDelta += item.N * item.I * (item.I - 1) * Math.Pow(Props.Delta, item.I - 2) * Math.Pow(Props.Tau, item.J);
                Props.PhiTau += item.N * Math.Pow(Props.Delta, item.I) * item.J * Math.Pow(Props.Tau, item.J - 1);
                Props.PhiTauTau += item.N * Math.Pow(Props.Delta, item.I) * item.J * (item.J - 1) * Math.Pow(Props.Tau, item.J - 2);
                Props.PhiDeltaTau += item.N * item.I * Math.Pow(Props.Delta, item.I - 1) * item.J * Math.Pow(Props.Tau, item.J - 1);
            }

            double pressure = Props.PhiDelta * Props.Delta * density * UniversalConstants.WaterGasConstant * Props.Temperature;
            return  Props.Pressure - pressure;
        }


        private Region3Properties Props { get; set; }

        private readonly RegionCoefficients[] Region3Coefficients = new RegionCoefficients[]
{
            new RegionCoefficients(double.NaN,   double.NaN,   1.0658070028513E+00),
            new RegionCoefficients(0,   0,   -1.5732845290239E+01),
            new RegionCoefficients(0,   1,   2.0944396974307E+01),
            new RegionCoefficients(0,   2,   -7.6867707878716E+00),
            new RegionCoefficients(0,   7,   2.6185947787954E+00),
            new RegionCoefficients(0,   10,  -2.8080781148620E+00),
            new RegionCoefficients(0,   12,  1.2053369696517E+00),
            new RegionCoefficients(0,   23,  -8.4566812812502E-03),
            new RegionCoefficients(1,   2,   -1.2654315477714E+00),
            new RegionCoefficients(1,   6,   -1.1524407806681E+00),
            new RegionCoefficients(1,   15,  8.8521043984318E-01),
            new RegionCoefficients(1,   17,  -6.4207765181607E-01),
            new RegionCoefficients(2,   0,   3.8493460186671E-01),
            new RegionCoefficients(2,   2,   -8.5214708824206E-01),
            new RegionCoefficients(2,   6,   4.8972281541877E+00),
            new RegionCoefficients(2,   7,   -3.0502617256965E+00),
            new RegionCoefficients(2,   22,  3.9420536879154E-02),
            new RegionCoefficients(2,   26,  1.2558408424308E-01),
            new RegionCoefficients(3,   0,   -2.7999329698710E-01),
            new RegionCoefficients(3,   2,   1.3899799569460E+00),
            new RegionCoefficients(3,   4,   -2.0189915023570E+00),
            new RegionCoefficients(3,   16,  -8.2147637173963E-03),
            new RegionCoefficients(3,   26,  -4.7596035734923E-01),
            new RegionCoefficients(4,   0,   4.3984074473500E-02),
            new RegionCoefficients(4,   2,   -4.4476435428739E-01),
            new RegionCoefficients(4,   4,   9.0572070719733E-01),
            new RegionCoefficients(4,   26,  7.0522450087967E-01),
            new RegionCoefficients(5,   1,   1.0770512626332E-01),
            new RegionCoefficients(5,   3,   -3.2913623258954E-01),
            new RegionCoefficients(5,   26,  -5.0871062041158E-01),
            new RegionCoefficients(6,   0,   -2.2175400873096E-02),
            new RegionCoefficients(6,   2,   9.4260751665092E-02),
            new RegionCoefficients(6,   26,  1.6436278447961E-01),
            new RegionCoefficients(7,   2,   -1.3503372241348E-02),
            new RegionCoefficients(8,   26,  -1.4834345352472E-02),
            new RegionCoefficients(9,   2,   5.7922953628084E-04),
            new RegionCoefficients(9,   26,  3.2308904703711E-03),
            new RegionCoefficients(10,  0,   8.0964802996215E-05),
            new RegionCoefficients(10,  1,   -1.6557679795037E-04),
            new RegionCoefficients(11,  26,  -4.4923899061815E-05)
};


    }
}
