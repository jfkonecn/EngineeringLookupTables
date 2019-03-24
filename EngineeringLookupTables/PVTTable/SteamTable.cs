using EngineeringLookupTables.NumericalMethods;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringLookupTables.PVTTable
{
    /// <summary>
    /// Base on IAPWS Industrial Formulation 1997
    /// </summary>
    public class SteamTable : IPVTTable
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
            if (temperature < MinTemperature || temperature > MaxTemperature ||
                pressure < MinPressure || pressure > MaxPressure ||
                (pressure > 50e6 && temperature > 1073.15))
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
            if (temperature < MinTemperature || temperature >= CriticalTemperature)
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
            if (pressure < MinPressure || pressure >= CriticalPressure)
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
            if (temperature > MaxTemperature || temperature < CriticalTemperature)
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
            if (pressure > MaxPressure || pressure < CriticalPressure)
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



        #region IPVTTableMembers

        public readonly double CriticalTemperature = 647.096;

        public readonly double CriticalPressure = 22.06e6;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enthalpy"></param>
        /// <param name="pressure"></param>
        /// <returns>null if an entry cannot be found</returns>
        public IThermoEntry GetThermoEntryAtEnthalpyAndPressure(double enthalpy, double pressure)
        {
            IThermoEntry liqEntry = GetThermoEntryAtSatPressure(pressure, SaturationRegion.Liquid),
                vapEntry = GetThermoEntryAtSatPressure(pressure, SaturationRegion.Vapor);
            if (vapEntry != null && liqEntry != null &&
                vapEntry.Enthalpy >= enthalpy && liqEntry.Enthalpy <= enthalpy)
            {
                return PVTEntry.BuildLiquidVaporEntry(vapEntry, liqEntry, (vapEntry.Enthalpy - enthalpy) / (vapEntry.Enthalpy - liqEntry.Enthalpy));
            }

            double fx(double x)
            {
                IThermoEntry thermoEntry = GetThermoEntryAtTemperatureAndPressure(x, pressure);
                if (thermoEntry == null)
                    return double.NaN;

                return thermoEntry.Enthalpy- enthalpy;
            }
            double temperature = NewtonsMethod.Solve(500, 1, fx, minX: MinTemperature, maxX: MaxTemperatureGivenPressure(pressure));
            if (double.IsNaN(temperature))
                return null;
            return GetThermoEntryAtTemperatureAndPressure(temperature, pressure);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entropy"></param>
        /// <param name="pressure"></param>
        /// <returns>null if an entry cannot be found</returns>
        public IThermoEntry GetThermoEntryAtEntropyAndPressure(double entropy, double pressure)
        {
            IThermoEntry liqEntry = GetThermoEntryAtSatPressure(pressure, SaturationRegion.Liquid),
                vapEntry = GetThermoEntryAtSatPressure(pressure, SaturationRegion.Vapor);
            if(vapEntry != null && liqEntry != null &&
                vapEntry.Entropy >= entropy && liqEntry.Entropy <= entropy)
            {
                return PVTEntry.BuildLiquidVaporEntry(vapEntry, liqEntry, (vapEntry.Entropy - entropy) /(vapEntry.Entropy - liqEntry.Entropy));
            }

            double fx(double x)
            {
                IThermoEntry thermoEntry = GetThermoEntryAtTemperatureAndPressure(x, pressure);
                if (thermoEntry == null)
                    return double.NaN;
                return thermoEntry.Entropy - entropy;
            }
            double temperature = NewtonsMethod.Solve(300, 0.0001, fx, minX: MinTemperature, maxX: MaxTemperatureGivenPressure(pressure));
            if (double.IsNaN(temperature))
                return null;
            return GetThermoEntryAtTemperatureAndPressure(temperature, pressure);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pressure">between 0 and CriticalPressure (in K)</param>
        /// <param name="phase">cannot be solid</param>
        /// <returns></returns>
        public IThermoEntry GetThermoEntryAtSatPressure(double satPressure, SaturationRegion phase)
        {
            if (!TryGetSatTemperatureUsingPressure(satPressure, out double satTemp))
            {
                return null;
            }
            switch (phase)
            {
                case SaturationRegion.Liquid:
                    return Region1Equation(satTemp, satPressure);
                case SaturationRegion.Vapor:
                    return Region2Equation(satTemp, satPressure);
                case SaturationRegion.Solid:
                default:
                    return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="satTemp">Must be between 273.15 and CriticalTemperature (in K)</param>
        /// <param name="phase"></param>
        /// <returns>null if out of range</returns>
        public IThermoEntry GetThermoEntryAtSatTemp(double satTemp, SaturationRegion phase)
        {
            if(!TryGetSatPressureUsingTemperature(satTemp, out double satPressure))
            {
                return null;
            }
            
            switch (phase)
            {
                case SaturationRegion.Liquid:
                    return Region1Equation(satTemp, satPressure);
                case SaturationRegion.Solid:
                    return Region2Equation(satTemp, satPressure);
                case SaturationRegion.Vapor:
                default:
                    return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="temperature"></param>
        /// <param name="pressure"></param>
        /// <returns>null if out of range</returns>
        public IThermoEntry GetThermoEntryAtTemperatureAndPressure(double temperature, double pressure)
        {
            SteamEquationRegion equationRegion = FindRegion(temperature, pressure);
            IThermoEntry thermoEntry;
            switch (equationRegion)
            {
                case SteamEquationRegion.Region1:
                case SteamEquationRegion.Region4:
                    thermoEntry = Region1Equation(temperature, pressure);
                    break;
                case SteamEquationRegion.Region2:
                    thermoEntry = Region2Equation(temperature, pressure);
                    break;
                case SteamEquationRegion.Region3:
                    thermoEntry = Region3Equation(temperature, pressure);
                    break;
                case SteamEquationRegion.Region5:
                    thermoEntry = Region5Equation(temperature, pressure);
                    break;
                case SteamEquationRegion.OutOfRange:
                default:
                    thermoEntry = null;
                    break;
            }
            return thermoEntry;
        }

        /// <summary>
        /// The model is undefined when the pressure is greater than 50 MPa AND the temperature is greater than 800
        /// </summary>
        /// <param name="pressure"></param>
        private double MaxTemperatureGivenPressure(double pressure)
        {
            if (pressure > 50e6)
                return 800 + 273.15;
            return 2273.15;
        }


        /// <summary>
        /// The model is undefined when the pressure is greater than 50 MPa AND the temperature is greater than 800
        /// </summary>
        /// <param name="pressure"></param>
        private double MinTemperatureGivenPressure(double pressure)
        {
            if (pressure > 50e6)
                return 0;
            return 800 + 273.15;
        }

        public double MaxPressure { get; } = 100e6;


        public double MinPressure { get; } = 0;

        #endregion
    }
}
