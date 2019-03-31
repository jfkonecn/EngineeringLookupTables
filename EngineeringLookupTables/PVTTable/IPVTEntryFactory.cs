using EngineeringLookupTables.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringLookupTables.PVTTable
{
    internal interface IPVTEntryFactory
    {
        PVTEntry BuildThermoEntry();
    }
}
