using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EngineeringLookupTables.PVTTable
{
    public class PVTEntry
    {
        public PVTEntry()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="vaporMassFraction"></param>
        /// <param name="liquidMassFraction"></param>
        /// <param name="solidMassFraction"></param>
        public PVTEntry(ThreePhaseEntry container, 
            double vaporMassFraction, double liquidMassFraction, double solidMassFraction)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));
            if (container.VaporEntry != null && container.VaporEntry.Region != Region.Vapor)
                throw new ArgumentOutOfRangeException(nameof(container.VaporEntry), "Must be vapor!");
            if (container.LiquidEntry != null && container.LiquidEntry.Region != Region.Liquid)
                throw new ArgumentOutOfRangeException(nameof(container.LiquidEntry), "Must be liquid!");
            if (container.SolidEntry != null && container.SolidEntry.Region != Region.Solid)
                throw new ArgumentOutOfRangeException(nameof(container.SolidEntry), "Must be solid!");
            if (vaporMassFraction + liquidMassFraction + solidMassFraction != 1)
                throw new ArgumentException("Fractions do not up to 1!");

            VaporMassFraction = vaporMassFraction;
            LiquidMassFraction = liquidMassFraction;
            SolidMassFraction = solidMassFraction;


            InterpolateProperties((x) => x.Temperature, (x) => Temperature = x, container);
            InterpolateProperties((x) => x.Pressure, (x) => Pressure = x, container);
            InterpolateProperties((x) => x.SpecificVolume, (x) => SpecificVolume = x, container);
            InterpolateProperties((x) => x.InternalEnergy, (x) => InternalEnergy = x, container);
            InterpolateProperties((x) => x.Enthalpy, (x) => Enthalpy = x, container);
            InterpolateProperties((x) => x.Entropy, (x) => Entropy = x, container);
            InterpolateProperties((x) => x.IsochoricHeatCapacity, (x) => IsochoricHeatCapacity = x, container);
            InterpolateProperties((x) => x.IsobaricHeatCapacity, (x) => IsobaricHeatCapacity = x, container);
            InterpolateProperties((x) => x.SpeedOfSound, (x) => SpeedOfSound = x, container);
        }

        private void InterpolateProperties(
            Func<PVTEntry, double> getter, Action<double> setter, ThreePhaseEntry container)
        {
            double num = 0;
            if(container.VaporEntry != null)
                num += getter(container.VaporEntry) * VaporMassFraction;
            if (container.LiquidEntry != null)
                num += getter(container.LiquidEntry) * LiquidMassFraction;
            if (container.SolidEntry != null)
                num += getter(container.SolidEntry) * SolidMassFraction;
            setter(num);
        }






        public Region Region
        {
            get
            {
                Region temp = Region.OutOfBounds;
                if (VaporMassFraction != 0 && LiquidMassFraction != 0 && SolidMassFraction != 0)
                {
                    temp = Region.SolidLiquidVapor;
                }
                else if (VaporMassFraction != 0 && LiquidMassFraction != 0 && SolidMassFraction == 0)
                {
                    temp = Region.LiquidVapor;
                }
                else if (VaporMassFraction != 0 && LiquidMassFraction == 0 && SolidMassFraction != 0)
                {
                    temp = Region.SolidVapor;
                }
                else if (VaporMassFraction != 0 && LiquidMassFraction == 0 && SolidMassFraction == 0)
                {
                    temp = Region.Vapor;
                }
                else if (VaporMassFraction == 0 && LiquidMassFraction != 0 && SolidMassFraction != 0)
                {
                    temp = Region.SolidLiquid;
                }
                else if (VaporMassFraction == 0 && LiquidMassFraction != 0 && SolidMassFraction == 0)
                {
                    temp = Region.Liquid;
                }
                else if (VaporMassFraction == 0 && LiquidMassFraction == 0 && SolidMassFraction != 0)
                {
                    temp = Region.Solid;
                }
                return temp;
            }
        }
        /// <summary>
        /// between 0 and 1
        /// </summary>
        public double VaporMassFraction { get; set; }
        /// <summary>
        /// between 0 and 1
        /// </summary>
        public double LiquidMassFraction { get; set; }
        /// <summary>
        /// between 0 and 1
        /// </summary>
        public double SolidMassFraction { get; set; }
        /// <summary>
        /// In Kelvin
        /// </summary>
        public double Temperature { get; set; }
        /// <summary>
        /// In Pa
        /// </summary>
        public double Pressure { get; set; }
        /// <summary>
        /// m3/kg
        /// </summary>
        public double SpecificVolume { get; set; }
        /// <summary>
        /// In J/kg
        /// </summary>
        public double InternalEnergy { get; set; }
        /// <summary>
        /// In J/kg
        /// </summary>
        public double Enthalpy { get; set; }
        /// <summary>
        /// J/(kg*K)
        /// </summary>
        public double Entropy { get; set; }
        /// <summary>
        /// Cv, Heat Capacity at constant volume (J/(kg*K))
        /// </summary>
        public double IsochoricHeatCapacity { get; set; }
        /// <summary>
        /// Cp, Heat Capacity at constant pressure (J/(kg*K))
        /// </summary>
        public double IsobaricHeatCapacity { get; set; }
        /// <summary>
        /// m/s
        /// </summary>
        public double SpeedOfSound { get; set; }
        /// <summary>
        /// In kg/m3
        /// </summary>
        public double Density { get; set; }
    }
}
