using EngineeringLookupTables.PVTTable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringLookupTables.Tests
{
    [TestFixture]
    public class PVTEntryTests
    {
        [Test]
        public void PVTEntryShouldBeEqual()
        {
            PVTEntry lhs = new PVTEntry()
            {
                Density = 1.0,
                Enthalpy = 50,
                Entropy = 143,
                InternalEnergy = 431,
                IsobaricHeatCapacity = 14,
                SolidMassFraction = 4356,
                SpecificVolume = 414,
                SpeedOfSound = 494,
                IsochoricHeatCapacity = 144,
                LiquidMassFraction = 9595,
                Pressure = 104,
                VaporMassFraction = 421,
                Region = Region.Gas,
                Temperature = 103
            };
            PVTEntry rhs = new PVTEntry()
            {
                Density = 1.0,
                Enthalpy = 50,
                Entropy = 143,
                InternalEnergy = 431,
                IsobaricHeatCapacity = 14,
                SolidMassFraction = 4356,
                SpecificVolume = 414,
                SpeedOfSound = 494,
                IsochoricHeatCapacity = 144,
                LiquidMassFraction = 9595,
                Pressure = 104,
                VaporMassFraction = 421,
                Region = Region.Gas,
                Temperature = 103
            };

            Assert.That(Equals(lhs, rhs));
            Assert.That(lhs == rhs);
            Assert.That(!(lhs != rhs));
            rhs.Density += lhs.Density;
            Assert.That(!Equals(lhs, rhs));
            Assert.That(!(lhs == rhs));
            Assert.That(lhs != rhs);
        }
    }
}
