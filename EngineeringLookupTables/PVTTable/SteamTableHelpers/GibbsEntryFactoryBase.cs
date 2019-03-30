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
            double specVol = Props.Pi * (Props.GammaPi * UniversalConstants.WaterGasConstant * Props.Temperature) / Props.Pressure;
            EntryFracs fracs = CalculateFractions(Region);
            return new PVTEntry()
            {
                Region = Region,
                Pressure = Props.Pressure,
                Temperature = Props.Temperature,
                VaporMassFraction = fracs.VapFrac,
                LiquidMassFraction = fracs.LiqFrac,
                SolidMassFraction = fracs.SolFrac,
                Density = 1d / specVol,
                SpecificVolume = specVol,
                InternalEnergy = UniversalConstants.WaterGasConstant * Props.Temperature * (Props.Tau * Props.GammaTau - Props.Pi * Props.GammaPi),
                Enthalpy = UniversalConstants.WaterGasConstant * Props.Temperature * Props.Tau * Props.GammaTau,
                Entropy = UniversalConstants.WaterGasConstant * (Props.Tau * Props.GammaTau - Props.Gamma),
                IsochoricHeatCapacity = UniversalConstants.WaterGasConstant * (-Math.Pow(-Props.Tau, 2) * Props.GammaTauTau + Math.Pow(Props.GammaPi - Props.Tau * Props.GammaPiTau, 2) / Props.GammaPiPi),
                IsobaricHeatCapacity = UniversalConstants.WaterGasConstant * -Math.Pow(-Props.Tau, 2) * Props.GammaTauTau,
                SpeedOfSound = Math.Sqrt(UniversalConstants.WaterGasConstant * Props.Temperature *
                    ((Math.Pow(Props.GammaPi, 2)) / ((Math.Pow(Props.GammaPi - Props.Tau * Props.GammaPiTau, 2) / (Math.Pow(Props.Tau, 2) * Props.GammaTauTau)) - Props.GammaPiPi))),
            };
        }


        private EntryFracs CalculateFractions(Region region)
        {
            EntryFracs fracs = new EntryFracs()
            {
                VapFrac = 0,
                LiqFrac = 0,
                SolFrac = 0
            };
            switch (region)
            {
                case Region.Vapor:
                    fracs.VapFrac = 1;
                    break;
                case Region.Liquid:
                    fracs.LiqFrac = 1;
                    break;
                case Region.Solid:
                    fracs.SolFrac = 1;
                    break;
                default:
                    break;
            }

            return fracs;
        }

        private struct EntryFracs
        {
            public double VapFrac { get; set; }
            public double LiqFrac { get; set; }
            public double SolFrac { get; set; }
        }

        public Region Region { get; protected set; }
        internal StandardProperties Props { get; } = new StandardProperties();
    }
}
