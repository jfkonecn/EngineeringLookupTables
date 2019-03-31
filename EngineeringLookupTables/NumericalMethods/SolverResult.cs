using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringLookupTables.NumericalMethods
{
    public class SolverResult
    {
        public double Value { get; internal set; }
        public bool ReachedMaxGuesses { get; internal set; }
    }
}
