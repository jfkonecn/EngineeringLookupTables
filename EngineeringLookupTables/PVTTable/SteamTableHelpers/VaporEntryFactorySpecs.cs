using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringLookupTables.PVTTable.SteamTableHelpers
{
    internal class VaporEntryFactorySpecs
    {
        public double TauShift { get; set; } = 0;
        public double Pressure { get; set; } = 0;
        public double Temperature { get; set; } = 0;
        public double CriticalTemperature { get; set; } = 0;
        public double CriticalPressure { get; set; } = 0;
        public double Pi { get; set; } = 0;
        public double Tau { get; set; } = 0;

    }
}
