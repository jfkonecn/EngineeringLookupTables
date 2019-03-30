using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringLookupTables.PVTTable.SteamTableHelpers
{
    internal class Region2Factory : VaporEntryFactoryBase
    {
        public Region2Factory(double temperature, double pressure, 
            double criticalTemperature, double criticalPressure) : 
            base(
                new VaporEntryFactorySpecs()
                {
                    TauShift = 0.5,
                    CriticalPressure = criticalPressure,
                    CriticalTemperature = criticalTemperature,
                    Pi = pressure / 1.0e6,
                    Tau = 540.0 / temperature,
                    Pressure = pressure,
                    Temperature = temperature
                })
        {

        }


        protected override RegionCoefficients[] BuildIdealCoefficients()
        {
            return new RegionCoefficients[]
            {
                new RegionCoefficients(0,   -9.6927686500217E+00),
                new RegionCoefficients(1,   1.0086655968018E+01),
                new RegionCoefficients(-5,  -5.6087911283020E-03),
                new RegionCoefficients(-4,  7.1452738081455E-02),
                new RegionCoefficients(-3,  -4.0710498223928E-01),
                new RegionCoefficients(-2,  1.4240819171444E+00),
                new RegionCoefficients(-1,  -4.3839511319450E+00),
                new RegionCoefficients(2,  -2.8408632460772E-01),
                new RegionCoefficients(3,   2.1268463753307E-02)
            };
        }

        protected override RegionCoefficients[] BuildResidualCoefficients()
        {
            return new RegionCoefficients[]
            {
                new RegionCoefficients(1,   0,   -1.7731742473213E-03),
                new RegionCoefficients(1,   1,   -1.7834862292358E-02),
                new RegionCoefficients(1,   2,   -4.5996013696365E-02),
                new RegionCoefficients(1,   3,   -5.7581259083432E-02),
                new RegionCoefficients(1,   6,   -5.0325278727930E-02),
                new RegionCoefficients(2,   1,   -3.3032641670203E-05),
                new RegionCoefficients(2,   2,   -1.8948987516315E-04),
                new RegionCoefficients(2,   4,   -3.9392777243355E-03),
                new RegionCoefficients(2,   7,   -4.3797295650573E-02),
                new RegionCoefficients(2,   36,  -2.6674547914087E-05),
                new RegionCoefficients(3,   0,   2.0481737692309E-08),
                new RegionCoefficients(3,   1,   4.3870667284435E-07),
                new RegionCoefficients(3,   3,   -3.2277677238570E-05),
                new RegionCoefficients(3,   6,   -1.5033924542148E-03),
                new RegionCoefficients(3,   35,  -4.0668253562649E-02),
                new RegionCoefficients(4,   1,   -7.8847309559367E-10),
                new RegionCoefficients(4,   2,   1.2790717852285E-08),
                new RegionCoefficients(4,   3,   4.8225372718507E-07),
                new RegionCoefficients(5,   7,   2.2922076337661E-06),
                new RegionCoefficients(6,   3,   -1.6714766451061E-11),
                new RegionCoefficients(6,   16,  -2.1171472321355E-03),
                new RegionCoefficients(6,   35,  -2.3895741934104E+01),
                new RegionCoefficients(7,   0,   -5.9059564324270E-18),
                new RegionCoefficients(7,   11,  -1.2621808899101E-06),
                new RegionCoefficients(7,   25,  -3.8946842435739E-02),
                new RegionCoefficients(8,   8,   1.1256211360459E-11),
                new RegionCoefficients(8,   36,  -8.2311340897998E+00),
                new RegionCoefficients(9,   13,  1.9809712802088E-08),
                new RegionCoefficients(10,  4,   1.0406965210174E-19),
                new RegionCoefficients(10,  10,  -1.0234747095929E-13),
                new RegionCoefficients(10,  14,  -1.0018179379511E-09),
                new RegionCoefficients(16,  29,  -8.0882908646985E-11),
                new RegionCoefficients(16,  50,  1.0693031879409E-01),
                new RegionCoefficients(18,  57,  -3.3662250574171E-01),
                new RegionCoefficients(20,  20,  8.9185845355421E-25),
                new RegionCoefficients(20,  35,  3.0629316876232E-13),
                new RegionCoefficients(20,  48,  -4.2002467698208E-06),
                new RegionCoefficients(21,  21,  -5.9056029685639E-26),
                new RegionCoefficients(22,  53,  3.7826947613457E-06),
                new RegionCoefficients(23,  39,  -1.2768608934681E-15),
                new RegionCoefficients(24,  26,  7.3087610595061E-29),
                new RegionCoefficients(24,  40,  5.5414715350778E-17),
                new RegionCoefficients(24,  58,  -9.4369707241210E-07)
            };
        }
    }
}
