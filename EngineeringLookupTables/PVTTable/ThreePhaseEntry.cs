using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringLookupTables.PVTTable
{

    public class ThreePhaseEntry
    {
        public PVTEntry VaporEntry { get; set; }
        public PVTEntry LiquidEntry { get; set; }
        public PVTEntry SolidEntry { get; set; }
    }

}
