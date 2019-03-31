using EngineeringLookupTables.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringLookupTables.PVTTable
{
    internal class LiquidVaporEntryFactory : IPVTEntryFactory
    {
        public LiquidVaporEntryFactory(PVTEntry vapEntry, PVTEntry liqEntry, double liquidFraction)
        {
            _container = new ThreePhaseEntry()
            {
                VaporEntry = vapEntry ?? throw new ArgumentNullException(nameof(vapEntry)),
                LiquidEntry = liqEntry ?? throw new ArgumentNullException(nameof(liqEntry)),
                SolidEntry = null
            };

            if(liquidFraction < 0 || liquidFraction > 1)
                throw new ArgumentOutOfRangeException(nameof(liquidFraction), "Must be between 0 and 1");
            _liquidFraction = liquidFraction; 
        }
        private readonly ThreePhaseEntry _container;
        private readonly double _liquidFraction;
        public PVTEntry BuildThermoEntry()
        {
            return new PVTEntry(_container, 1.0 - _liquidFraction, _liquidFraction, 0.0);
        }
    }
}
