﻿using EngineeringLookupTables.Common;
using EngineeringLookupTables.PVTTable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringLookupTables.Tests.Helpers
{
    public class PVTEntryHelper
    {        
        public PVTEntryHelper(PVTEntry expected)
        {
            Expected = expected;
        }

        public PVTEntry Expected { get; }

        /// <summary>
        /// 
        /// </summary>
        public void AssertValidFractions()
        {
            if(Expected.SolidMassFraction == 0 && 
                Expected.LiquidMassFraction == 0 && 
                Expected.VaporMassFraction == 0)
            {
                return;
            }


            Assert.That(Expected.VaporMassFraction >= 0 && Expected.VaporMassFraction <= 1, 
                "Vapor fraction is not between 0 and 1");
            Assert.That(Expected.LiquidMassFraction >= 0 && Expected.LiquidMassFraction <= 1,
                "Liquid fraction is not between 0 and 1");
            Assert.That(Expected.SolidMassFraction >= 0 && Expected.SolidMassFraction <= 1,
                "Solid fraction is not between 0 and 1");

            Assert.That(Expected.VaporMassFraction +
                Expected.LiquidMassFraction + Expected.SolidMassFraction, 
                Is.EqualTo(1).Within(0.001),
                "Mass fractions must add to 1");

            Assert.That(Region.OutOfBounds != Expected.Region, "Region should not be out of bounds");
        }

        public void AssertEqual(PVTEntry actual)
        {
            if(actual == Expected)
            {
                Assert.Pass();
            }
            else if(actual == null)
            {
                Assert.Fail();
            }
            PVTEntry actualValue = actual;
            Assert.That(actualValue.Region == Expected.Region, nameof(Expected.Region));
            Assert.That(actualValue.VaporMassFraction, Is.EqualTo(Expected.VaporMassFraction).Within(0.5).Percent, 
                nameof(Expected.VaporMassFraction));
            Assert.That(actualValue.LiquidMassFraction, Is.EqualTo(Expected.LiquidMassFraction).Within(0.5).Percent,
                nameof(Expected.LiquidMassFraction));
            Assert.That(actualValue.SolidMassFraction, Is.EqualTo(Expected.SolidMassFraction).Within(0.5).Percent,
                nameof(Expected.SolidMassFraction));
            Assert.That(actualValue.Temperature, Is.EqualTo(Expected.Temperature).Within(0.5).Percent,
                nameof(Expected.Temperature));
            Assert.That(actualValue.Pressure, Is.EqualTo(Expected.Pressure).Within(0.5).Percent,
                nameof(Expected.Pressure));
            Assert.That(actualValue.SpecificVolume, Is.EqualTo(Expected.SpecificVolume).Within(0.5).Percent,
                nameof(Expected.SpecificVolume));
            Assert.That(actualValue.InternalEnergy, Is.EqualTo(Expected.InternalEnergy).Within(0.5).Percent,
                nameof(Expected.InternalEnergy));
            Assert.That(actualValue.Enthalpy, Is.EqualTo(Expected.Enthalpy).Within(0.5).Percent,
                nameof(Expected.Enthalpy));
            Assert.That(actualValue.Entropy, Is.EqualTo(Expected.Entropy).Within(0.5).Percent,
                nameof(Expected.Entropy));
            Assert.That(actualValue.IsochoricHeatCapacity, Is.EqualTo(Expected.IsochoricHeatCapacity).Within(0.5).Percent,
                nameof(Expected.IsochoricHeatCapacity));
            Assert.That(actualValue.IsobaricHeatCapacity, Is.EqualTo(Expected.IsobaricHeatCapacity).Within(0.5).Percent,
                nameof(Expected.IsobaricHeatCapacity));
            Assert.That(actualValue.SpeedOfSound, Is.EqualTo(Expected.SpeedOfSound).Within(0.5).Percent,
                nameof(Expected.SpeedOfSound));
            Assert.That(actualValue.Density, Is.EqualTo(Expected.Density).Within(0.5).Percent,
                nameof(Expected.Density));
        }
    }
}
