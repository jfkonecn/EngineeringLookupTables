using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringLookupTables.PVTTable.SteamTableHelpers
{
    internal class Region1Factory : GibbsEntryFactoryBase
    {
        public Region1Factory(double temperature, double pressure) : base(Region.Liquid)
        {
            Props.Pi = pressure / 16.53e6;
            Props.Tau = 1386.0 / temperature;

            foreach (RegionCoefficients item in Region1Coefficients)
            {
                Props.Gamma += item.N * Math.Pow(7.1 - Props.Pi, item.I) * Math.Pow(Props.Tau - 1.222, item.J);
                Props.GammaPi += -item.N * item.I * Math.Pow(7.1 - Props.Pi, item.I - 1) * Math.Pow(Props.Tau - 1.222, item.J);
                Props.GammaPiPi += item.N * item.I * (item.I - 1) * Math.Pow(7.1 - Props.Pi, item.I - 2) * Math.Pow(Props.Tau - 1.222, item.J);
                Props.GammaTau += item.N * item.J * Math.Pow(7.1 - Props.Pi, item.I) * Math.Pow(Props.Tau - 1.222, item.J - 1);
                Props.GammaTauTau += item.N * item.J * (item.J - 1) * Math.Pow(7.1 - Props.Pi, item.I) * Math.Pow(Props.Tau - 1.222, item.J - 2);
                Props.GammaPiTau += -item.N * item.I * item.J * Math.Pow(7.1 - Props.Pi, item.I - 1) * Math.Pow(Props.Tau - 1.222, item.J - 1);
            }
        }

        private readonly RegionCoefficients[] Region1Coefficients = new RegionCoefficients[]
        {
            new RegionCoefficients(0, -2, 1.4632971213167E-01),
            new RegionCoefficients(0, -1, -8.4548187169114E-01),
            new RegionCoefficients(0, 0, -3.756360367204),
            new RegionCoefficients(0, 1, 3.3855169168385E+00),
            new RegionCoefficients(0, 2, -9.5791963387872E-01),
            new RegionCoefficients(0, 3, 1.5772038513228E-01),
            new RegionCoefficients(0, 4, -1.6616417199501E-02),
            new RegionCoefficients(0, 5, 8.1214629983568E-04),
            new RegionCoefficients(1, -9, 2.8319080123804E-04),
            new RegionCoefficients(1, -7, -6.0706301565874E-04),
            new RegionCoefficients(1, -1, -1.8990068218419E-02),
            new RegionCoefficients(1, 0, -3.2529748770505E-02),
            new RegionCoefficients(1, 1, -2.1841717175414E-02),
            new RegionCoefficients(1, 3, -5.2838357969930E-05),
            new RegionCoefficients(2, -3, -4.7184321073267E-04),
            new RegionCoefficients(2, 0, -3.0001780793026E-04),
            new RegionCoefficients(2, 1, 4.7661393906987E-05),
            new RegionCoefficients(2, 3, -4.4141845330846E-06),
            new RegionCoefficients(2, 17, -7.2694996297594E-16),
            new RegionCoefficients(3, -4, -3.1679644845054E-05),
            new RegionCoefficients(3, 0, -2.8270797985312E-06),
            new RegionCoefficients(3, 6, -8.5205128120103E-10),
            new RegionCoefficients(4, -5, -2.2425281908000E-06),
            new RegionCoefficients(4, -2, -6.5171222895601E-07),
            new RegionCoefficients(4, 10, -1.4341729937924E-13),
            new RegionCoefficients(5, -8, -4.0516996860117E-07),
            new RegionCoefficients(8, -11, -1.2734301741641E-09),
            new RegionCoefficients(8, -6, -1.7424871230634E-10),
            new RegionCoefficients(21, -29, -6.8762131295531E-19),
            new RegionCoefficients(23, -31, 1.4478307828521E-20),
            new RegionCoefficients(29, -38, 2.6335781662795E-23),
            new RegionCoefficients(30, -39, -1.1947622640071E-23),
            new RegionCoefficients(31, -40, 1.8228094581404E-24),
            new RegionCoefficients(32, -41, -9.3537087292458E-26)
        };
    }
}
