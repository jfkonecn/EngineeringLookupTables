using EngineeringLookupTables.Common;
using EngineeringLookupTables.NumericalMethods;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringLookupTables.PVTTable
{
    public abstract class PVTTable
    {
        /// <summary>
        /// Gets entry at passed pressure and passed temperature. Null when no entry found.
        /// </summary>
        /// <param name="temperature">Desired Temperture (K)</param>
        /// <param name="pressure">Desired Pressure (Pa)</param>
        /// <returns></returns>
        public abstract PVTEntry GetEntryAtTemperatureAndPressure(double temperature, double pressure);

        /// <summary>
        /// Gets entry for saturated liquid or vapor at passed pressure. Null when no entry found.
        /// </summary>
        /// <param name="pressure">Desired Pressure (Pa)</param>
        /// <param name="phase"></param>
        /// <returns></returns>
        public abstract PVTEntry GetEntryAtSatPressure(double pressure, SaturationRegion phase);

        /// <summary>
        /// Gets entry and Pressure for saturated liquid or vapor at passed satTemp. Null when no entry found.
        /// </summary>
        /// <param name="satTemp">Desired saturation temperature</param>
        /// <param name="phase"></param>
        /// <returns></returns>
        public abstract PVTEntry GetEntryAtSatTemp(double satTemp, SaturationRegion phase);

        /// <summary>
        /// Gets the entry which matches the entropy and pressure passed
        /// </summary>
        /// <param name="entropy">J/(kg*K)</param>
        /// <param name="pressure">Pa</param>
        /// <returns></returns>
        public PVTEntry GetEntryAtEntropyAndPressure(double entropy, double pressure)
        {
            PVTEntry liqEntry = GetEntryAtSatPressure(pressure, SaturationRegion.Liquid);
            PVTEntry vapEntry = GetEntryAtSatPressure(pressure, SaturationRegion.Vapor);
            if (vapEntry != null && liqEntry != null &&
                vapEntry.Entropy >= entropy && liqEntry.Entropy <= entropy)
            {
                double liqFac = (vapEntry.Entropy - entropy) / (vapEntry.Entropy - liqEntry.Entropy);
                LiquidVaporEntryFactory fac = new LiquidVaporEntryFactory(vapEntry, liqEntry, liqFac);
                return fac.BuildThermoEntry();
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
        /// Gets the entry which matches the entropy and pressure passed
        /// </summary>
        /// <param name="entropy">kJ/(kg*K)</param>
        /// <param name="pressure">Pa</param>
        /// <returns></returns>
        public PVTEntry GetEntryAtEnthalpyAndPressure(double enthalpy, double pressure)
        {
            PVTEntry liqEntry = GetEntryAtSatPressure(pressure, SaturationRegion.Liquid);
            PVTEntry vapEntry = GetEntryAtSatPressure(pressure, SaturationRegion.Vapor);
            if (vapEntry != null && liqEntry != null &&
                vapEntry.Enthalpy >= enthalpy && liqEntry.Enthalpy <= enthalpy)
            {
                double liqFac = (vapEntry.Enthalpy - enthalpy) / (vapEntry.Enthalpy - liqEntry.Enthalpy);
                LiquidVaporEntryFactory fac = new LiquidVaporEntryFactory(vapEntry, liqEntry, liqFac);
                return fac.BuildThermoEntry();
            }

            double fx(double x)
            {
                PVTEntry entry = GetEntryAtTemperatureAndPressure(x, pressure);
                if (entry == null)
                    return double.NaN;

                return entry.Enthalpy - enthalpy;
            }
            Range tempRange = GetTemperatureRange(pressure);
            double temperature = NewtonsMethod.Solve(500, 1, fx, minX: tempRange.Min, maxX: tempRange.Max);
            if (double.IsNaN(temperature))
                return null;
            return GetEntryAtTemperatureAndPressure(temperature, pressure);
        }
        /// <summary>
        /// In K
        /// </summary>
        public abstract double CriticalTemperature { get; }
        /// <summary>
        /// In Pa
        /// </summary>
        public abstract double CriticalPressure { get; }
        /// <summary>
        /// Find temperature range given pressure
        /// </summary>
        /// <param name="pressure">in Pa</param>
        /// <returns></returns>
        public abstract Range GetTemperatureRange(double pressure);

        /// <summary>
        /// Find pressure range given temperature
        /// </summary>
        /// <param name="temperature">in K</param>
        /// <returns></returns>
        public abstract Range GetPressureRange(double temperature);

    }

    public enum SaturationRegion
    {
        /// <summary>
        /// Pressure is less than both the sublimation and vaporization curve and is below the critical temperature
        /// </summary>
        Vapor = Region.Vapor,
        /// <summary>
        /// Pressure is above the vaporization curve and the temperature is greater than the fusion curve and less than the critical temperature
        /// </summary>
        Liquid = Region.Liquid,
        /// <summary>
        /// Pressure is above the sublimation curve and temperature is less than the fusion curve
        /// </summary>
        Solid = Region.Solid
    }
}
