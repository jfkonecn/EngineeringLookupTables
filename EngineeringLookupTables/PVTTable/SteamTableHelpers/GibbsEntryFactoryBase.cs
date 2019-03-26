using System;
using System.Collections.Generic;
using System.Text;


namespace EngineeringLookupTables.PVTTable.SteamTableHelpers
{
    /// <summary>
    /// Uses gibbs free energy to build an entry
    /// </summary>
    internal abstract class GibbsEntryFactoryBase : IPVTEntryFactory
    {

        protected GibbsEntryFactoryBase(Region region, double temperature, double pressure)
        {
            Region = region;
            Props.Temperature = temperature;
            Props.Pressure = pressure;
        }

        public PVTEntry BuildThermoEntry()
        {
            return new PVTEntry()
            {
                SpecificVolume = Props.Pi * (Props.GammaPi * UniversalConstants.WaterGasConstant * Props.Temperature) / Props.Pressure,
                InternalEnergy = UniversalConstants.WaterGasConstant * Props.Temperature * (Props.Tau * Props.GammaTau - Props.Pi * Props.GammaPi),
                Enthalpy = UniversalConstants.WaterGasConstant * Props.Temperature * Props.Tau * Props.GammaTau,
                Entropy = UniversalConstants.WaterGasConstant * (Props.Tau * Props.GammaTau - Props.Gamma),
                IsochoricHeatCapacity = UniversalConstants.WaterGasConstant * (-Math.Pow(-Props.Tau, 2) * Props.GammaTauTau + Math.Pow(Props.GammaPi - Props.Tau * Props.GammaPiTau, 2) / Props.GammaPiPi),
                IsobaricHeatCapacity = UniversalConstants.WaterGasConstant * -Math.Pow(-Props.Tau, 2) * Props.GammaTauTau,
                SpeedOfSound = Math.Sqrt(UniversalConstants.WaterGasConstant * Props.Temperature *
                    ((Math.Pow(Props.GammaPi, 2)) / ((Math.Pow(Props.GammaPi - Props.Tau * Props.GammaPiTau, 2) / (Math.Pow(Props.Tau, 2) * Props.GammaTauTau)) - Props.GammaPiPi))),
            };
        }

        public Region Region { get; protected set; }
        internal StandardProperties Props { get; } = new StandardProperties();
    }
}
