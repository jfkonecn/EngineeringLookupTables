using EngineeringLookupTables.Common;
using EngineeringLookupTables.NumericalMethods.FiniteDifferenceFormulas;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringLookupTables.NumericalMethods
{
    public static class NewtonsMethod
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startingX"></param>
        /// <param name="fx"></param>
        /// <param name="fxPrime"></param>
        /// <returns>first x value found where f(x) == 0 if max guesses reached then NaN is returned</returns>
        public static double Solve(Func<double, double> fx, Func<double, double> fxPrime, Range xRange)
        {
            double curX = xRange.MidPoint,
                fxResult = fx(curX),
                totalGuesses = 0;
            const double maxErr = 1e-6,
                maxGuesses = 1e4;
            while (Math.Abs(fxResult) > maxErr && totalGuesses < maxGuesses)
            {
                fxResult = fx(curX);
                curX = curX - fxResult / fxPrime(curX);
                curX = curX < xRange.Min ? xRange.Min : curX;
                curX = curX > xRange.Max ? xRange.Max : curX;
                totalGuesses++;
            }
            return curX;
        }
        /// <summary>
        /// Use finite difference formulas to calculate fxPrime
        /// </summary>
        /// <param name="fx"></param>
        /// <returns></returns>
        public static double Solve(Func<double, double> fx, Range xRange)
        {
            return Solve(fx, (x) => 
            {
                double step = xRange.RangeMagnitude / 1e3d;

                if (xRange.Min >= x - step || double.IsNaN(x - step))
                {
                    return FirstDerivative.ThreePointForward(x, fx, step);
                }
                if (xRange.Max <= x + step || double.IsNaN(x + step))
                {
                    return FirstDerivative.ThreePointBackward(x, fx, step);
                }
                return FirstDerivative.TwoPointCentral(x, fx, step);
            }, xRange);
        }
    }
}
