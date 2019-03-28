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
        public PVTEntryHelper(PVTEntry entry)
        {
            Entry = entry;
        }

        public PVTEntry Entry { get; }

        /// <summary>
        /// 
        /// </summary>
        public void AssertValidFractions()
        {
            Assert.That(Entry.VaporMassFraction >= 0 && Entry.VaporMassFraction <= 1, 
                "Vapor fraction is not between 0 and 1");
            Assert.That(Entry.LiquidMassFraction >= 0 && Entry.LiquidMassFraction <= 1,
                "Liquid fraction is not between 0 and 1");
            Assert.That(Entry.SolidMassFraction >= 0 && Entry.SolidMassFraction <= 1,
                "Solid fraction is not between 0 and 1");

            Assert.That(Entry.VaporMassFraction +
                Entry.LiquidMassFraction + Entry.SolidMassFraction == 1, 
                "Mass fractions must add to 1");

            Assert.That(Region.OutOfBounds != Entry.Region, "Region should not be out of bounds");
        }
    }
}
