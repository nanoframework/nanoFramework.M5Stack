// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Hardware.Esp32;

namespace nanoFramework.AtomLite
{
    /// <summary>
    /// The AtomLite Board.
    /// </summary>
    public static partial class AtomLite
    {
        private static RgbLed _rgbLed;

        /// <summary>
        /// RGB NeoPixel led.
        /// </summary>
        public static RgbLed NeoPixel
        {
            get
            {
                if (_rgbLed == null)
                {
                    _rgbLed = new();
                }

                return _rgbLed;
            }
        }

        static AtomLite()
        {
            // Setup first the I2C bus
            Configuration.SetPinFunction(32, DeviceFunction.I2C1_CLOCK);
            Configuration.SetPinFunction(26, DeviceFunction.I2C1_DATA);

            // Setup buttons
            _gpio = new();
        }
    }
}
