// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Device.Adc;
using System.Device.Dac;
using System.Device.Gpio;
using System.Device.I2c;
using System.Device.Spi;
using System.IO.Ports;
using Iot.Device.Magnetometer;
using Iot.Device.Mpu6886;

namespace nanoFramework.M5Stack
{
#if M5CORE
    /// <summary>
    /// M5Core2 board
    /// </summary>
    public static partial class M5Core2
#else
    /// <summary>
    /// M5Stack board
    /// </summary>
    public static partial class M5Stack
#endif
    {
        private static Bmm150 _bmm150;
        private static Mpu6886AccelerometerGyroscope _mpu6886;
        private static GpioController _gpio;
        private static DacChannel _dac1;
        private static DacChannel _dac2;
#if M5CORE
        private static nanoFramework.M5Core2.Screen _screen;
#else
        private static nanoFramework.M5Stack.Screen _screen;
#endif
        private static SerialPort _serialPort;
        private static AdcController _adc;

        /// <summary>
        /// Gets the Magnetometer.
        /// </summary>
        public static Bmm150 Magnetometer
        {
            get
            {
                // We do this to avoid having to load the magnetometer if not needed or not connected
                if (_bmm150 == null)
                {
                    _bmm150 = new(GetI2cDevice(0x10));
                }

                return _bmm150;
            }
        }

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
                    _mpu6886 = new(GetI2cDevice(0x68));
                }

                return _mpu6886;
            }
        }


        /// <summary>
        /// Gets the main GPIO Controller.
        /// </summary>
        public static GpioController GpioController => _gpio;


        /// <summary>
        /// Gets DAC1 which is GPIO 25.
        /// </summary>
        public static DacChannel Dac1
        {
            get
            {
                // We are creating it on demand
                if (_dac1 == null)
                {
                    _dac1 = DacController.GetDefault().OpenChannel(0);
                }

                return _dac1;
            }
        }

        /// <summary>
        /// Gets DAC1 which is GPIO 26.
        /// </summary>
        public static DacChannel Dac2
        {
            get
            {
                // We are creating it on demand
                if (_dac2 == null)
                {
                    _dac2 = DacController.GetDefault().OpenChannel(1);
                }

                return _dac2;
            }
        }

        /// <summary>
        /// Gets an I2C device.
        /// </summary>
        /// <param name="i2cDeviceAddress">The I2C device address on the bus.</param>
        /// <returns>The I2cDevice.</returns>
        public static I2cDevice GetI2cDevice(int i2cDeviceAddress) => new(new I2cConnectionSettings(1, i2cDeviceAddress));

        /// <summary>
        /// Gets an I2C device.
        /// </summary>
        /// <param name="i2cDeviceAddress">The I2C device address on the bus.</param>
        /// <returns>The I2cDevice.</returns>
        public static I2cDevice GetGrove(int i2cDeviceAddress) => new(new I2cConnectionSettings(1, i2cDeviceAddress));

        /// <summary>
        /// Gets an SPI Device.
        /// </summary>
        /// <param name="chipSelect">The chip select of the device, needs to be any valid GPIO.</param>
        /// <returns>An SpiDevice.</returns>
        public static SpiDevice GetSpiDevice(int chipSelect) => new(new SpiConnectionSettings(1, chipSelect));

        /// <summary>
        /// Gets the second serial port RX2/TX2
        /// </summary>
        public static SerialPort SerialPort
        {
            get
            {
                // We do this so the COM port is used only if needed
                if (_serialPort == null)
                {
                    _serialPort = new("COM2");
                }

                return _serialPort;
            }
        }
    }
}
