// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Iot.Device.Mpu6886;
using Iot.Device.Ws28xx.Esp32;
using nanoFramework.Hardware.Esp32;
using System.Device.I2c;

namespace nanoFramework.AtomMatrix
{
    /// <summary>
    /// The AtomMatrix Board.
    /// </summary>
    public static partial class AtomMatrix
    {
        /// <summary>
        /// GPIO number of the neo pixel LED (from the datasheet).
        /// </summary>
        private const int _rgbLedGpio = 27;
        private static Ws2812c _rgbLed;
        private static Mpu6886AccelerometerGyroscope _mpu6886;

        /// <summary>
        /// Gets the Accelerometer and Gyroscope.
        /// </summary>
        public static Mpu6886AccelerometerGyroscope AccelerometerGyroscope
        {
            get
            {
                // We do this to avoid having to load the Accelerometer if not needed or not connected
                if (_mpu6886 == null)
                {
                    _mpu6886 = new(new(new I2cConnectionSettings(1, 0x68)));
                }

                return _mpu6886;
            }
        }

        /// <summary>
        /// RGB LED matrix (WS2812C).
        /// </summary>
        public static Ws2812c LedMatrix
        {
            get
            {
                if (_rgbLed == null)
                {
                    // instantiate a new Pixel controller, ATOM Matrix has 5x5 LEDs
                    _rgbLed = new Ws2812c(_rgbLedGpio, 5, 5);
                }

                return _rgbLed;
            }
        }

        static AtomMatrix()
        {
            // Setup first the I2C bus
            Configuration.SetPinFunction(21, DeviceFunction.I2C1_CLOCK);
            Configuration.SetPinFunction(25, DeviceFunction.I2C1_DATA);

            // setup the SPI bus 
            Configuration.SetPinFunction(19, DeviceFunction.SPI1_MOSI);
            Configuration.SetPinFunction(33, DeviceFunction.SPI1_MISO);
            Configuration.SetPinFunction(23, DeviceFunction.SPI1_CLOCK);

            // Setup buttons
            _gpio = new();

            LedMatrix.Image.Clear();
        }
    }
}
