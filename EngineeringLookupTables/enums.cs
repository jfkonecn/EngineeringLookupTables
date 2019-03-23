using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringLookupTables
{
    public enum Region
    {
        /// <summary>
        /// Temperature and pressure is above the critical point
        /// </summary>
        SupercriticalFluid,
        /// <summary>
        /// Above critical temperature, but is below the critical pressure
        /// </summary>
        Gas,
        /// <summary>
        /// Pressure is less than both the sublimation and vaporization curve and is below the critical temperature
        /// </summary>
        Vapor,
        /// <summary>
        /// Pressure is above the vaporization curve and the temperature is greater than the fusion curve and less than the critical temperature
        /// </summary>
        Liquid,
        /// <summary>
        /// Pressure is above the sublimation curve and temperature is less than the fusion curve
        /// </summary>
        Solid,
        SolidLiquid,
        LiquidVapor,
        SolidVapor,
        SolidLiquidVapor
    }
}
