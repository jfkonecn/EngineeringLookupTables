using System;
using System.Collections.Generic;
using System.Text;
using EngineeringLookupTables;
using EngineeringLookupTables.Common;
using EngineeringLookupTables.PVTTable;
using EngineeringLookupTables.Tests.Helpers;
using NUnit.Framework;

namespace EngineeringLookupTables.Tests
{
    [TestFixture]
    public class SteamTableMethods
    {
        private static SteamTable _steamTable;


        [OneTimeSetUp]
        public void SetUpTest()
        {
            _steamTable = SteamTable.Table;
        }


        [Test]
        [TestCase(Region.SupercriticalFluid, 750, 78.309563916917e6, 1.0 / 500, 2102.069317626429e3, 2258.688445460262e3,
                4.469719056217e3, 2.71701677121e3, 6.341653594791e3, 760.696040876798, 0, 0, 0)]
        [TestCase(Region.Liquid, 473.15, 40e6, 0.001122406088, 825.228016170348e3,
                870.124259682489e3, 2.275752861241e3, 3.292858637199e3, 4.315767590903e3, 1457.418351596083, 0, 1, 0)]
        [TestCase(Region.SupercriticalFluid, 2000, 30e6, 0.03113852187, 5637.070382521894e3, 6571.226038618478e3,
                8.536405231138e3, 2.395894362358e3, 2.885698818781e3, 1067.369478777425, 0, 0, 0)]
        [TestCase(Region.Gas, 823.15, 14e6, 0.024763222774, 3114.302136294585e3, 3460.987255128561e3,
                6.564768889364e3, 1.892708832325e3, 2.666558503968e3, 666.050616844223, 0, 0, 0)]
        public void PressureAndTemperature(Region region, double temperature,
            double pressure, double specificVolume, double internalEnergy, double enthalpy,
            double entropy, double isochoricHeatCapacity, double isobaricHeatCapacity,
            double speedOfSound,
            double vaporFraction, double liquidFraction, double solidFraction)
        {
            PVTEntry expected = new PVTEntry()
            {
                Region = region,
                Temperature = temperature,
                Pressure = pressure,
                SpecificVolume = specificVolume,
                InternalEnergy = internalEnergy,
                Enthalpy = enthalpy,
                Entropy = entropy,
                IsochoricHeatCapacity = isochoricHeatCapacity,
                IsobaricHeatCapacity = isobaricHeatCapacity,
                SpeedOfSound = speedOfSound,
                VaporMassFraction = vaporFraction,
                LiquidMassFraction = liquidFraction,
                SolidMassFraction = solidFraction,
                Density = 1 / specificVolume
            };

            PVTEntry actual = _steamTable.GetEntryAtTemperatureAndPressure(temperature, pressure);
            ValidateEntry(expected, actual);
        }

        [Test]
        [TestCase(SaturationRegion.Liquid, 393.361545936488,
            0.2e6, 0.00106051840643552, 504471.741847973, 504683.84552926,
            1530.0982011075, 3666.99397284121, 4246.73524917536,
            1520.69128792808,
            0, 1, 0)]
        [TestCase(SaturationRegion.Vapor, 393.361545936488,
            0.2e6, 0.885735065081644, 2529094.32835793, 2706241.34137425,
            7126.8563914686, 1615.96336473298, 2175.22318865273,
            481.883535821489,
            1, 0, 0)]
        public void SatPressure(SaturationRegion region, double temperature,
            double pressure, double specificVolume, double internalEnergy, double enthalpy,
            double entropy, double isochoricHeatCapacity, double isobaricHeatCapacity,
            double speedOfSound,
            double vaporFraction, double liquidFraction, double solidFraction)
        {
            PVTEntry expected = new PVTEntry()
            {
                Region = (Region)region,
                Temperature = temperature,
                Pressure = pressure,
                SpecificVolume = specificVolume,
                InternalEnergy = internalEnergy,
                Enthalpy = enthalpy,
                Entropy = entropy,
                IsochoricHeatCapacity = isochoricHeatCapacity,
                IsobaricHeatCapacity = isobaricHeatCapacity,
                SpeedOfSound = speedOfSound,
                VaporMassFraction = vaporFraction,
                LiquidMassFraction = liquidFraction,
                SolidMassFraction = solidFraction,
                Density = 1 / specificVolume
            };

            PVTEntry actual = _steamTable.GetEntryAtSatPressure(pressure, region);
            ValidateEntry(expected, actual);
        }



        [Test]
        [TestCase(SaturationRegion.Liquid, 393.361545936488,
            0.2e6, 0.00106051840643552, 504471.741847973, 504683.84552926,
            1530.0982011075, 3666.99397284121, 4246.73524917536,
            1520.69128792808,
            0, 1, 0)]
        [TestCase(SaturationRegion.Vapor, 393.361545936488,
            0.2e6, 0.885735065081644, 2529094.32835793, 2706241.34137425,
            7126.8563914686, 1615.96336473298, 2175.22318865273,
            481.883535821489,
            1, 0, 0)]
        public void SatTemperature(SaturationRegion region, double temperature,
            double pressure, double specificVolume, double internalEnergy, double enthalpy,
            double entropy, double isochoricHeatCapacity, double isobaricHeatCapacity,
            double speedOfSound,
            double vaporFraction, double liquidFraction, double solidFraction)
        {
            PVTEntry expected = new PVTEntry()
            {
                Region = (Region)region,
                Temperature = temperature,
                Pressure = pressure,
                SpecificVolume = specificVolume,
                InternalEnergy = internalEnergy,
                Enthalpy = enthalpy,
                Entropy = entropy,
                IsochoricHeatCapacity = isochoricHeatCapacity,
                IsobaricHeatCapacity = isobaricHeatCapacity,
                SpeedOfSound = speedOfSound,
                VaporMassFraction = vaporFraction,
                LiquidMassFraction = liquidFraction,
                SolidMassFraction = solidFraction,
                Density = 1 / specificVolume
            };
            PVTEntry actual = _steamTable.GetEntryAtSatTemp(temperature, region);
            ValidateEntry(expected, actual);
        }


        [Test]
        [TestCase(Region.SupercriticalFluid, 750, 78.309563916917e6, 1.0 / 500, 2102.069317626429e3, 2258.688445460262e3,
    4.469719056217e3, 2.71701677121e3, 6.341653594791e3, 760.696040876798, 0, 0, 0)]
        [TestCase(Region.Liquid, 473.15, 40e6, 0.001122406088, 825.228016170348e3,
                870.124259682489e3, 2.275752861241e3, 3.292858637199e3, 4.315767590903e3, 1457.418351596083, 0, 1, 0)]
        [TestCase(Region.SupercriticalFluid, 2000, 30e6, 0.03113852187, 5637.070382521894e3, 6571.226038618478e3,
                8.536405231138e3, 2.395894362358e3, 2.885698818781e3, 1067.369478777425, 0, 0, 0)]
        [TestCase(Region.Gas, 823.15, 14e6, 0.024763222774, 3114.302136294585e3, 3460.987255128561e3,
                6.564768889364e3, 1.892708832325e3, 2.666558503968e3, 666.050616844223, 0, 0, 0)]
        [TestCase(Region.LiquidVapor, 318.957548207023, 10e3, 11.8087122249855, 1999135.82661328,
                2117222.94886314, 6.6858e3, 1966.28009225455, 2377.86300751001, 655.005141924186,
                0.804912447078132, 0.195087552921867, 0)]
        public void EnthalpyAndPressure(Region region, double temperature,
            double pressure, double specificVolume, double internalEnergy, double enthalpy,
            double entropy, double isochoricHeatCapacity, double isobaricHeatCapacity,
            double speedOfSound,
            double vaporFraction, double liquidFraction, double solidFraction)
        {
            PVTEntry expected = new PVTEntry()
            {
                Region = region,
                Temperature = temperature,
                Pressure = pressure,
                SpecificVolume = specificVolume,
                InternalEnergy = internalEnergy,
                Enthalpy = enthalpy,
                Entropy = entropy,
                IsochoricHeatCapacity = isochoricHeatCapacity,
                IsobaricHeatCapacity = isobaricHeatCapacity,
                SpeedOfSound = speedOfSound,
                VaporMassFraction = vaporFraction,
                LiquidMassFraction = liquidFraction,
                SolidMassFraction = solidFraction,
                Density = 1 / specificVolume
            };

            PVTEntry actual = _steamTable.GetEntryAtEnthalpyAndPressure(enthalpy, pressure);
            ValidateEntry(expected, actual);
        }


        [Test]
        [TestCase(Region.SupercriticalFluid, 750, 78.309563916917e6, 1.0 / 500, 2102.069317626429e3, 2258.688445460262e3,
4.469719056217e3, 2.71701677121e3, 6.341653594791e3, 760.696040876798, 0, 0, 0)]
        [TestCase(Region.Liquid, 473.15, 40e6, 0.001122406088, 825.228016170348e3,
                870.124259682489e3, 2.275752861241e3, 3.292858637199e3, 4.315767590903e3, 1457.418351596083, 0, 1, 0)]
        [TestCase(Region.SupercriticalFluid, 2000, 30e6, 0.03113852187, 5637.070382521894e3, 6571.226038618478e3,
                8.536405231138e3, 2.395894362358e3, 2.885698818781e3, 1067.369478777425, 0, 0, 0)]
        [TestCase(Region.Gas, 823.15, 14e6, 0.024763222774, 3114.302136294585e3, 3460.987255128561e3,
                6.564768889364e3, 1.892708832325e3, 2.666558503968e3, 666.050616844223, 0, 0, 0)]
        [TestCase(Region.LiquidVapor, 318.957548207023, 10e3, 11.8087122249855, 1999135.82661328,
                2117222.94886314, 6.6858e3, 1966.28009225455, 2377.86300751001, 655.005141924186,
                0.804912447078132, 0.195087552921867, 0)]
        public void EntropyAndPressure(Region region, double temperature,
            double pressure, double specificVolume, double internalEnergy, double enthalpy,
            double entropy, double isochoricHeatCapacity, double isobaricHeatCapacity,
            double speedOfSound,
            double vaporFraction, double liquidFraction, double solidFraction)
        {
            PVTEntry expected = new PVTEntry()
            {
                Region = region,
                Temperature = temperature,
                Pressure = pressure,
                SpecificVolume = specificVolume,
                InternalEnergy = internalEnergy,
                Enthalpy = enthalpy,
                Entropy = entropy,
                IsochoricHeatCapacity = isochoricHeatCapacity,
                IsobaricHeatCapacity = isobaricHeatCapacity,
                SpeedOfSound = speedOfSound,
                VaporMassFraction = vaporFraction,
                LiquidMassFraction = liquidFraction,
                SolidMassFraction = solidFraction,
                Density = 1 / specificVolume
            };

            PVTEntry actual = _steamTable.GetEntryAtEntropyAndPressure(entropy, pressure);
            ValidateEntry(expected, actual);
        }

        private void ValidateEntry(PVTEntry expected, PVTEntry actual)
        {
            PVTEntryHelper helper = new PVTEntryHelper(expected);
            helper.AssertValidFractions();
            helper.AssertEqual(actual);
        }
    }
}
