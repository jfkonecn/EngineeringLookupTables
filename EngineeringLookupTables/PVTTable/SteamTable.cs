using EngineeringLookupTables.Common;
using EngineeringLookupTables.NumericalMethods;
using EngineeringLookupTables.PVTTable.SteamTableHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringLookupTables.PVTTable
{
    /// <summary>
    /// Base on IAPWS Industrial Formulation 1997
    /// </summary>
    public class SteamTable : PVTTable
    {
        public readonly static SteamTable Table = new SteamTable();

        private SteamTable()
        {

        }

        private enum SteamEquationRegion
        {
            Region1,
            Region2,
            Region3,
            Region4,
            Region5,
            OutOfRange
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="temperature">in K</param>
        /// <param name="pressure">in Pa</param>
        /// <returns></returns>
        private SteamEquationRegion FindRegion(double temperature, double pressure)
        {
            Range tempRange = GetTemperatureRange(pressure);
            Range preRange = GetPressureRange(temperature);

            if (!tempRange.IsWithin(temperature) ||
                !preRange.IsWithin(pressure))
            {
                return SteamEquationRegion.OutOfRange;
            }

            if (temperature > (273.15 + 800))
            {
                return SteamEquationRegion.Region5;
            }
            else if(temperature > (600 + 273.15))
            {
                return SteamEquationRegion.Region2;
            }
            else if (TryGetSatPressureUsingTemperature(temperature, out double satPressure))
            {
                if (satPressure == pressure)
                {
                    return SteamEquationRegion.Region4;
                }
                else if (satPressure > pressure)
                {
                    return SteamEquationRegion.Region2;
                }
                return SteamEquationRegion.Region1;
            }
            else if (TryGetBoundary34PressureUsingTemperature(temperature, out double boundaryPressure))
            {
                if (boundaryPressure > pressure)
                {
                    return SteamEquationRegion.Region2;
                }
                return SteamEquationRegion.Region3;
            }
            return SteamEquationRegion.OutOfRange;
        }




        private bool TryGetSatPressureUsingTemperature(double temperature, out double pressure)
        {
            if (temperature >= CriticalTemperature)
            {
                pressure = double.NaN;
                return false;
            }
            double
                satTempRatio = temperature / 1,
                theta = satTempRatio + (nRegion4[8] / (satTempRatio - nRegion4[9])),
                A = Math.Pow(theta, 2) + nRegion4[0] * theta + nRegion4[1],
                B = nRegion4[2] * Math.Pow(theta, 2) + nRegion4[3] * theta + nRegion4[4],
                C = nRegion4[5] * Math.Pow(theta, 2) + nRegion4[6] * theta + nRegion4[7];
            pressure = Math.Pow((2 * C) / (-B + Math.Pow(Math.Pow(B, 2) - 4 * A * C, 0.5)), 4) * 1e6;
            return true;
        }

        private bool TryGetSatTemperatureUsingPressure(double pressure, out double temperature)
        {
            if (pressure >= CriticalPressure)
            {
                temperature = double.NaN;
                return false;
            }

            double beta = Math.Pow(pressure / 1e6, 0.25),
                E = Math.Pow(beta, 2) + nRegion4[2] * beta + nRegion4[5],
                F = nRegion4[0] * Math.Pow(beta, 2) + nRegion4[3] * beta + nRegion4[6],
                G = nRegion4[1] * Math.Pow(beta, 2) + nRegion4[4] * beta + nRegion4[7],
                D = (2 * G) / (-F - Math.Pow(Math.Pow(F, 2) - 4 * E * G, 0.5));
            temperature = (nRegion4[9] + D - Math.Pow(Math.Pow(nRegion4[9] + D, 2) - 4 * (nRegion4[8] + nRegion4[9] * D), 0.5)) / 2;
            return true;
        }


        private bool TryGetBoundary34PressureUsingTemperature(double temperature, out double pressure)
        {
            if (temperature < CriticalTemperature)
            {
                pressure = double.NaN;
                return false;
            }
            double theta = temperature / 1;
            pressure = (nBoundary34[0] + nBoundary34[1] * theta + nBoundary34[2] * Math.Pow(theta, 2)) * 1e6;
            return true;
        }

        private bool TryGetBoundary34TemperatureUsingPressure(double pressure, out double temperature)
        {
            if (pressure < CriticalPressure)
            {
                temperature = double.NaN;
                return false;
            }
            double pi = pressure / 1e6;
            temperature = (nBoundary34[3] + Math.Sqrt((pi - nBoundary34[4]) / nBoundary34[2]));
            return true;
        }








        private static readonly double[] nRegion4 = new double[]
        {
            1167.0521452767,
            -724213.16703206,
            -17.073846940092,
            12020.82470247,
            -3232555.0322333,
            14.91510861353,
            -4823.2657361591,
            405113.40542057,
            -0.23855557567849,
            650.17534844798
        };

        private static readonly double[] nBoundary34 = new double[]
        {
            348.05185628969,
            -1.1671859879975,
            0.0010192970039326,
            572.54459862746,
            13.91883977887
        };


      


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pressure">between 0 and CriticalPressure (in K)</param>
        /// <param name="phase">cannot be solid</param>
        /// <returns></returns>
        public override PVTEntry GetEntryAtSatPressure(double satPressure, SaturationRegion phase)
        {
            if (!TryGetSatTemperatureUsingPressure(satPressure, out double satTemp))
            {
                return null;
            }
            IPVTEntryFactory fac = null;
            switch (phase)
            {
                case SaturationRegion.Liquid:
                    fac = new Region1Factory(satTemp, satPressure);
                    break;
                case SaturationRegion.Vapor:
                    fac = new Region2Factory(satTemp, satPressure, CriticalTemperature, CriticalPressure);
                    break;
                case SaturationRegion.Solid:
                default:
                    return null;
            }
            return fac?.BuildThermoEntry();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="satTemp">Must be between 273.15 and CriticalTemperature (in K)</param>
        /// <param name="phase"></param>
        /// <returns>null if out of range</returns>
        public override PVTEntry GetEntryAtSatTemp(double satTemp, SaturationRegion phase)
        {
            if(!TryGetSatPressureUsingTemperature(satTemp, out double satPressure))
            {
                return null;
            }
            IPVTEntryFactory fac = null;
            switch (phase)
            {
                case SaturationRegion.Liquid:
                    fac = new Region1Factory(satTemp, satPressure);
                    break;
                case SaturationRegion.Solid:
                    fac = new Region2Factory(satTemp, satPressure, CriticalTemperature, CriticalPressure);
                    break;
                case SaturationRegion.Vapor:
                default:
                    fac = null;
                    break;
            }
            return fac?.BuildThermoEntry();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="temperature"></param>
        /// <param name="pressure"></param>
        /// <returns>null if out of range</returns>
        public override PVTEntry GetEntryAtTemperatureAndPressure(double temperature, double pressure)
        {
            SteamEquationRegion equationRegion = FindRegion(temperature, pressure);
            IPVTEntryFactory fac = null;
            switch (equationRegion)
            {
                case SteamEquationRegion.Region1:
                    fac = new Region1Factory(temperature, pressure);
                    break;
                case SteamEquationRegion.Region2:
                    fac = new Region2Factory(temperature, pressure, CriticalTemperature, CriticalPressure);
                    break;
                case SteamEquationRegion.Region3:
                    Range tempRange = GetTemperatureRange(pressure);
                    fac = new Region3Factory(temperature, pressure, tempRange.Min, tempRange.Max);
                    break;
                case SteamEquationRegion.Region4:
                    fac = new Region4Factory(temperature, pressure);
                    break;
                case SteamEquationRegion.Region5:
                    fac = new Region4Factory(temperature, pressure);
                    break;
                case SteamEquationRegion.OutOfRange:
                default:
                    fac = null;
                    break;
            }
            return fac?.BuildThermoEntry();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pressure">in Pa</param>
        /// <returns></returns>
        public override Range GetTemperatureRange(double pressure)
        {
            if (pressure > 50e6)
                return new Range(273.15, 800 + 273.15);
            return new Range(800 + 273.15, 2000 + 273.15);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="temperature">in K</param>
        /// <returns></returns>
        public override Range GetPressureRange(double temperature)
        {
            if (temperature > 800)
                return new Range(0, 10e6);
            return new Range(0, 100e6);
        }

        public override double CriticalTemperature => 647.096;

        public override double CriticalPressure => 22.06e6;
    }
}
