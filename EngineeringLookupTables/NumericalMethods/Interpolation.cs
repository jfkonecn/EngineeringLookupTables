using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringLookupTables.NumericalMethods
{
    internal class Interpolation
    {

        /// <summary>
        /// Performs a linear interpolation between two reference points 
        /// </summary>
        /// <param name="x">Desired x coordinate</param>
        /// <param name="refPoint1">reference point one</param>
        /// <param name="refPoint2">reference point two</param>
        /// <returns>Desired y coordinate</returns>
        internal static double LinearInterpolation(double x, DataPoint refPoint1, DataPoint refPoint2)
        {
            return ((refPoint2.Y - refPoint1.Y) / (refPoint2.X - refPoint1.X)) * (x - refPoint1.X) + refPoint1.Y;
        }

    }
}
