using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EngineeringMath.Resources.PVTTables
{
    public class ThermoEntry
    {
        public ThermoEntry()
        {
            ValidateFractions(region, vaporMassFraction, liquidMassFraction, solidMassFraction);
        }

        private ThermoEntry(IThermoEntry vapEntry, IThermoEntry liqEntry, IThermoEntry solidEntry, 
            double vaporMassFraction, double liquidMassFraction, double solidMassFraction)
        {
            if (vapEntry != null && vapEntry.Region != Region.Vapor)
                throw new ArgumentOutOfRangeException(nameof(vapEntry), "Must be vapor!");
            if (liqEntry != null && liqEntry.Region != Region.Liquid)
                throw new ArgumentOutOfRangeException(nameof(liqEntry), "Must be liquid!");
            if (solidEntry != null && solidEntry.Region != Region.Solid)
                throw new ArgumentOutOfRangeException(nameof(solidEntry), "Must be solid!");
            if (vaporMassFraction + liquidMassFraction + solidMassFraction != 1)
                throw new ArgumentException("Fractions do not up to 1!");

            VaporMassFraction = vaporMassFraction;
            LiquidMassFraction = liquidMassFraction;
            SolidMassFraction = solidMassFraction;

            if (vapEntry != null && liqEntry != null && solidEntry != null)
            {
                Region = Region.SolidLiquidVapor;
            }
            else if (vapEntry != null && liqEntry != null)
            {
                Region = Region.LiquidVapor;
            }
            else if (liqEntry != null && solidEntry != null)
            {
                Region = Region.SolidLiquid;
            }
            else if (vapEntry != null && solidEntry != null)
            {
                Region = Region.SolidVapor;
            }
            else if (vapEntry != null)
            {
                Region = Region.Vapor;
            }
            else if (liqEntry != null)
            {
                Region = Region.Liquid;
            }
            else if (solidEntry != null)
            {
                Region = Region.Solid;
            }
            else
            {
                throw new ArgumentException("All entries are null!");
            }
            InterpolateProperties(nameof(IThermoEntry.Temperature), vapEntry, liqEntry, solidEntry);
            InterpolateProperties(nameof(IThermoEntry.Pressure), vapEntry, liqEntry, solidEntry);
            InterpolateProperties(nameof(IThermoEntry.SpecificVolume), vapEntry, liqEntry, solidEntry);
            InterpolateProperties(nameof(IThermoEntry.InternalEnergy), vapEntry, liqEntry, solidEntry);
            InterpolateProperties(nameof(IThermoEntry.Enthalpy), vapEntry, liqEntry, solidEntry);
            InterpolateProperties(nameof(IThermoEntry.Entropy), vapEntry, liqEntry, solidEntry);
            InterpolateProperties(nameof(IThermoEntry.IsochoricHeatCapacity), vapEntry, liqEntry, solidEntry);
            InterpolateProperties(nameof(IThermoEntry.IsobaricHeatCapacity), vapEntry, liqEntry, solidEntry);
            InterpolateProperties(nameof(IThermoEntry.SpeedOfSound), vapEntry, liqEntry, solidEntry);
        }

        private void InterpolateProperties(string propName,
            IThermoEntry vapEntry, IThermoEntry liqEntry, IThermoEntry solidEntry)
        {
            PropertyInfo propInfo = typeof(ThermoEntry).GetProperty(propName);
            double num = 0;
            if(vapEntry != null)
                num += (double)propInfo.GetValue(vapEntry) * VaporMassFraction;
            if (liqEntry != null)
                num += (double)propInfo.GetValue(liqEntry) * LiquidMassFraction;
            if (solidEntry != null)
                num += (double)propInfo.GetValue(solidEntry) * SolidMassFraction;
            propInfo.SetValue(this, num);
        }


        public static IThermoEntry BuildLiquidVaporEntry(IThermoEntry vapEntry, IThermoEntry liqEntry, double liquidFraction)
        {
            return new ThermoEntry(vapEntry, liqEntry, null, 1.0 - liquidFraction, liquidFraction, 0.0);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="BadPhaseFractionComposition"></exception>
        private void ValidateFractions(Region region, double vaporMassFraction, double liquidMassFraction, double solidMassFraction)
        {
            if (vaporMassFraction < 0 || vaporMassFraction > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(vaporMassFraction));
            }
            else if (liquidMassFraction < 0 || liquidMassFraction > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(liquidMassFraction));
            }
            else if (solidMassFraction < 0 || solidMassFraction > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(solidMassFraction));
            }

            double sumFraction = vaporMassFraction + liquidMassFraction + solidMassFraction;
            if (sumFraction != 0 && sumFraction != 1)
            {
                throw new BadPhaseFractionComposition();
            }

            Region temp = region;


            if (vaporMassFraction != 0 && liquidMassFraction != 0 && solidMassFraction != 0)
            {
                temp = Region.SolidLiquidVapor;
            }
            else if (vaporMassFraction != 0 && liquidMassFraction != 0 && solidMassFraction == 0)
            {
                temp = Region.LiquidVapor;
            }
            else if (vaporMassFraction != 0 && liquidMassFraction == 0 && solidMassFraction != 0)
            {
                temp = Region.SolidVapor;
            }
            else if (vaporMassFraction != 0 && liquidMassFraction == 0 && solidMassFraction == 0)
            {
                temp = Region.Vapor;
            }
            else if (vaporMassFraction == 0 && liquidMassFraction != 0 && solidMassFraction != 0)
            {
                temp = Region.SolidLiquid;
            }
            else if (vaporMassFraction == 0 && liquidMassFraction != 0 && solidMassFraction == 0)
            {
                temp = Region.Liquid;
            }
            else if (vaporMassFraction == 0 && liquidMassFraction == 0 && solidMassFraction != 0)
            {
                temp = Region.Solid;
            }
            if (temp != region)
            {
                throw new BadPhaseFractionComposition();
            }
        }

        Region Region { get; }
        /// <summary>
        /// between 0 and 1
        /// </summary>
        public double VaporMassFraction { get; }
        /// <summary>
        /// between 0 and 1
        /// </summary>
        public double LiquidMassFraction { get; }
        /// <summary>
        /// between 0 and 1
        /// </summary>
        public double SolidMassFraction { get; }
        /// <summary>
        /// In Kelvin
        /// </summary>
        public double Temperature { get; }
        /// <summary>
        /// In Pa
        /// </summary>
        public double Pressure { get; }
        /// <summary>
        /// m3/kg
        /// </summary>
        public double SpecificVolume { get; }
        /// <summary>
        /// In J/kg
        /// </summary>
        public double InternalEnergy { get; }
        /// <summary>
        /// In J/kg
        /// </summary>
        public double Enthalpy { get; }
        /// <summary>
        /// J/(kg*K)
        /// </summary>
        public double Entropy { get; }
        /// <summary>
        /// Cv, Heat Capacity at constant volume (J/(kg*K))
        /// </summary>
        public double IsochoricHeatCapacity { get; }
        /// <summary>
        /// Cp, Heat Capacity at constant pressure (J/(kg*K))
        /// </summary>
        public double IsobaricHeatCapacity { get; }
        /// <summary>
        /// m/s
        /// </summary>
        public double SpeedOfSound { get; }
        /// <summary>
        /// In kg/m3
        /// </summary>
        public double Density { get; }
    }
}
