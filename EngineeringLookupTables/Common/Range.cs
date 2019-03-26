using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringLookupTables.Common
{
    public class Range
    {
        internal Range(double min, double max)
        {
            if(min > max)
            {
                throw new ArgumentException("min must be less than max");
            }

            Min = min;
            Max = max;
        }

        public double Min { get; }
        public double Max { get; }

        /// <summary>
        /// True if the passed number is within the range
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool IsWithin(double num)
        {
            return num >= Min && num <= Max;
        }
    }
}
