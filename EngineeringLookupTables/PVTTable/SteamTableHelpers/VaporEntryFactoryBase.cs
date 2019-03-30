using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringLookupTables.PVTTable.SteamTableHelpers
{
    internal abstract class VaporEntryFactoryBase : GibbsEntryFactoryBase
    {
        protected VaporEntryFactoryBase(VaporEntryFactorySpecs specs) :
            base(Region.OutOfBounds, specs.Temperature, specs.Pressure)
        {
            Props.Tau = specs.Tau;
            Props.Pi = specs.Pi;
            TauShift = specs.TauShift;
            CriticalTemperature = specs.CriticalTemperature;
            CriticalPressure = specs.CriticalPressure;
            IdealCoefficients = BuildIdealCoefficients();
            ResidualCoefficients = BuildResidualCoefficients();
            FinishPropsSetup();
        }



        private void FinishPropsSetup()
        {
            Props.Gamma = Math.Log(Props.Pi);
            Props.GammaPi = 1 / Props.Pi;
            Props.GammaPiPi = -1 / Math.Pow(Props.Pi, 2);
            Props.GammaTau = 0;
            Props.GammaTauTau = 0;
            Props.GammaPiTau = 0;

            foreach (RegionCoefficients item in IdealCoefficients)
            {
                Props.Gamma += item.N * Math.Pow(Props.Tau, item.J);
                Props.GammaTau += item.N * item.J * Math.Pow(Props.Tau, item.J - 1);
                Props.GammaTauTau += item.N * item.J * (item.J - 1) * Math.Pow(Props.Tau, item.J - 2);
            }
            foreach (RegionCoefficients item in ResidualCoefficients)
            {
                Props.Gamma += item.N * Math.Pow(Props.Pi, item.I) * Math.Pow(Props.Tau - TauShift, item.J);
                Props.GammaPi += item.N * item.I * Math.Pow(Props.Pi, item.I - 1) * Math.Pow(Props.Tau - TauShift, item.J);
                Props.GammaPiPi += item.N * item.I * (item.I - 1) * Math.Pow(Props.Pi, item.I - 2) * Math.Pow(Props.Tau - TauShift, item.J);
                Props.GammaTau += item.N * Math.Pow(Props.Pi, item.I) * item.J * Math.Pow(Props.Tau - TauShift, item.J - 1);
                Props.GammaTauTau += item.N * Math.Pow(Props.Pi, item.I) * item.J * (item.J - 1) * Math.Pow(Props.Tau - TauShift, item.J - 2);
                Props.GammaPiTau += item.N * item.I * Math.Pow(Props.Pi, item.I - 1) * item.J * Math.Pow(Props.Tau - TauShift, item.J - 1);
            }

            Region = Region.Vapor;
            if (Props.Temperature > CriticalTemperature)
            {
                if (Props.Pressure > CriticalPressure)
                    Region = Region.SupercriticalFluid;
                else
                    Region = Region.Gas;
            }
        }


        private double TauShift { get; }

        private RegionCoefficients[] ResidualCoefficients { get; set; }
        private RegionCoefficients[] IdealCoefficients { get; set; }
        public double CriticalTemperature { get; }

        public double CriticalPressure { get; }

        protected abstract RegionCoefficients[] BuildResidualCoefficients();
        protected abstract RegionCoefficients[] BuildIdealCoefficients();
    }
}
