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
        public abstract Maybe<PVTEntry> GetEntryAtTemperatureAndPressure(double temperature, double pressure);

        /// <summary>
        /// Gets entry for saturated liquid or vapor at passed pressure. Null when no entry found.
        /// </summary>
        /// <param name="pressure">Desired Pressure (Pa)</param>
        /// <param name="phase"></param>
        /// <returns></returns>
        public abstract Maybe<PVTEntry> GetEntryAtSatPressure(double pressure, SaturationRegion phase);

        /// <summary>
        /// Gets entry and Pressure for saturated liquid or vapor at passed satTemp. Null when no entry found.
        /// </summary>
        /// <param name="satTemp">Desired saturation temperature</param>
        /// <param name="phase"></param>
        /// <returns></returns>
        public abstract Maybe<PVTEntry> GetEntryAtSatTemp(double satTemp, SaturationRegion phase);

        /// <summary>
        /// Gets the entry which matches the entropy and pressure passed
        /// </summary>
        /// <param name="entropy">J/(kg*K)</param>
        /// <param name="pressure">Pa</param>
        /// <returns></returns>
        public Maybe<PVTEntry> GetEntryAtEntropyAndPressure(double entropy, double pressure)
        {
            return GetEntryAtPressureAndProperty(pressure, entropy, (x) => x.Entropy);
        }

        /// <summary>
        /// Gets the entry which matches the entropy and pressure passed
        /// </summary>
        /// <param name="entropy">kJ/(kg*K)</param>
        /// <param name="pressure">Pa</param>
        /// <returns></returns>
        public Maybe<PVTEntry> GetEntryAtEnthalpyAndPressure(double enthalpy, double pressure)
        {
            return GetEntryAtPressureAndProperty(pressure, enthalpy, (x) => x.Enthalpy);
        }




        private Maybe<PVTEntry> GetEntryAtPressureAndProperty(
            double pressure, double targetPropVal, Func<PVTEntry, double> propGetter)
        {
            Maybe<PVTEntry> liqEntry = GetEntryAtSatPressure(pressure, SaturationRegion.Liquid);
            Maybe<PVTEntry> vapEntry = GetEntryAtSatPressure(pressure, SaturationRegion.Vapor);
            if (vapEntry.HasValue && liqEntry.HasValue &&
                propGetter(vapEntry.Value) >= targetPropVal && propGetter(liqEntry.Value) <= targetPropVal)
            {
                double liqFac = (propGetter(vapEntry.Value) - targetPropVal) / 
                    (propGetter(vapEntry.Value) - propGetter(liqEntry.Value));
                LiquidVaporEntryFactory fac = new LiquidVaporEntryFactory(vapEntry.Value, liqEntry.Value, liqFac);
                return Maybe<PVTEntry>.Some(fac.BuildThermoEntry());
            }

            double fx(double x)
            {
                Maybe<PVTEntry> entry = GetEntryAtTemperatureAndPressure(x, pressure);
                return entry.HasValue ? propGetter(entry.Value) - targetPropVal : double.NaN;
            }
            Range tempRange = GetTemperatureRange(pressure);
            SolverResult temperature = NewtonsMethod.Solve(fx, tempRange);

            return temperature.ReachedMaxGuesses ? 
                Maybe<PVTEntry>.None : 
                GetEntryAtTemperatureAndPressure(temperature.Value, pressure);
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


}
