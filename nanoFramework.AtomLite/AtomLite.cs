// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Iot.Device.Button;
using nanoFramework.Hardware.Esp32;
using nanoFramework.Hardware.Esp32.Rmt;
using System;
using System.Device.Adc;
using System.Device.Dac;
using System.Device.Gpio;
using System.Device.I2c;
using System.Device.Spi;

namespace nanoFramework.AtomLite
{
    /// <summary>
    /// The AtomLite Board.
    /// </summary>
    public static class AtomLite
    {
        private static GpioButton _button;
        private static RgbLed _rgbLed;
        private static GpioController _gpio;
        private static DacChannel _dac1;
        private static DacChannel _dac2;
        private static AdcController _adc;
        private static TransmitterChannel _irLed;

        /// <summary>
        /// Main button.
        /// </summary>
        public static GpioButton Button
        {
            get
            {
                if (_button == null)
                {
                    _button = new(39, _gpio, false);
                }

                return _button;
            }
        }

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

        /// <summary>
        /// Gets the main <see cref="GpioController"/>.
        /// </summary>
        public static GpioController GpioController => _gpio;

        /// <summary>
        /// Gets <see cref="DacChannel"/> connected to GPIO 25.
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
        /// Gets <see cref="DacChannel"/> connected to GPIO 26.
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
        /// Gets an <see cref="I2cDevice"/>.
        /// </summary>
        /// <param name="i2cDeviceAddress">The address of the <see cref="I2cDevice"/> on the bus.</param>
        /// <returns>The I2cDevice.</returns>
        public static I2cDevice GetI2cDevice(int i2cDeviceAddress) => new(new I2cConnectionSettings(1, i2cDeviceAddress));

        /// <summary>
        /// Gets an <see cref="I2cDevice"/>.
        /// </summary>
        /// <param name="i2cDeviceAddress">The address of the <see cref="I2cDevice"/> on the bus.</param>
        /// <returns>The I2cDevice.</returns>
        public static I2cDevice GetGrove(int i2cDeviceAddress) => new(new I2cConnectionSettings(1, i2cDeviceAddress));

        /// <summary>
        /// Gets the infrared led as a RMT transmitter channel.
        /// </summary>
        public static TransmitterChannel InfraredLed
        {
            get
            {
                if (_irLed == null)
                {
                    _irLed = new(12);
                }

                return _irLed;
            }
        }

        /// <summary>
        /// Gets an <see cref="SpiDevice"/>.
        /// </summary>
        /// <param name="chipSelect">The chip select of the device, needs to be any valid GPIO.</param>
        /// <returns>An SpiDevice.</returns>
        public static SpiDevice GetSpiDevice(int chipSelect) => new(new SpiConnectionSettings(1, chipSelect));

        /// <summary>
        /// Gets an <see cref="AdcChannel"/>
        /// </summary>
        /// <param name="gpioNumber">The GPIO pin number where the <see cref="AdcChannel"/> is connected to.</param>
        /// <returns>An AdcChannel</returns>
        public static AdcChannel GetAdcGpio(int gpioNumber)
        {
            if (_adc == null)
            {
                _adc = new();
            }

            switch (gpioNumber)
            {
                case 33:
                    Configuration.SetPinFunction(12, DeviceFunction.ADC1_CH5);
                    return _adc.OpenChannel(5);
                case 32:
                    Configuration.SetPinFunction(25, DeviceFunction.ADC1_CH4);
                    return _adc.OpenChannel(4);
                default:
                    throw new ArgumentException(nameof(gpioNumber));
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
