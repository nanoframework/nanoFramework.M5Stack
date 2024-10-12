// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Iot.Device.Button;
using Iot.Device.Buzzer;
using Iot.Device.EPaper.Drivers.Jd796xx;
using Iot.Device.Rtc;
using nanoFramework.Hardware.Esp32;
using System;
using System.Device.Adc;
using System.Device.Gpio;
using System.Device.I2c;
using System.Device.Spi;

namespace nanoFramework.M5Stack
{
    /// <summary>
    /// M5 CoreInk board.
    /// </summary>
    public static class M5CoreInk
    {
        private readonly static I2cDevice _device;
        private static SpiDevice _spi;
        private static AdcController _adc;
        private static Buzzer _buzzer;
        private static GpioPin _led;
        private static GpioButton _button;
        private static GpioButton _left;
        private static GpioButton _center;
        private static GpioButton _right;
        private static GpioButton _power;
        private static GpioController _gpio;
        private static Pcf8563 _rtc;
        private static Gdew0154m09 _screen;

        private const int ScreenBusyPin = 4;
        private const int ScreenResetPin = 0;
        private const int ScreenDCPin = 15;

        #region properties

        /// <summary>
        /// Gets the upper button.
        /// </summary>
        public static GpioButton RollerLeft
        {
            get
            {
                _left ??= new(37, GpioController, false, PinMode.Input);

                return _left;
            }
        }

        /// <summary>
        /// Gets the upper button.
        /// </summary>
        public static GpioButton RollerRight
        {
            get
            {
                _right ??= new(39, GpioController, false, PinMode.Input);

                return _right;
            }
        }

        /// <summary>
        /// Gets the upper button.
        /// </summary>
        public static GpioButton RollerButton
        {
            get
            {
                _center ??= new(38, GpioController, false, PinMode.Input);

                return _center;
            }
        }

        /// <summary>
        /// Gets the upper button.
        /// </summary>
        public static GpioButton Button
        {
            get
            {
                _button ??= new(5, GpioController, false);

                return _button;
            }
        }

        /// <summary>
        /// Gets the power button.
        /// </summary>
        public static GpioButton Power
        {
            get
            {
                _power ??= new(27, GpioController, false);

                return _power;
            }
        }

        /// <summary>
        /// Gets the green led.
        /// </summary>
        public static GpioPin Led
        {
            get
            {
                _led ??= GpioController.OpenPin(10, PinMode.Output);

                return _led;
            }
        }

        /// <summary>
        /// Gets the Buzzer.
        /// </summary>
        public static Buzzer Buzzer
        {
            get
            {
                // SetPinFunction already made in the static constructor
                _buzzer ??= new(2);

                return _buzzer;
            }
        }

        /// <summary>
        /// Gets the main <see cref="GpioController"/>.
        /// </summary>
        public static GpioController GpioController
        {
            get
            {
                _gpio ??= new();

                return _gpio;
            }
        }

        /// <summary>
        /// Gets the internal clock.
        /// </summary>
        public static Pcf8563 RTC
        {
            get
            {
                _rtc ??= new(_device);

                return _rtc;
            }
        }

        /// <summary>
        /// Get the ePaper screen.
        /// </summary>
        public static Gdew0154m09 Screen
        {
            get
            {
                InitializeScreen();

                return _screen;
            }
        }

        #endregion

        static M5CoreInk()
        {
            Configuration.SetPinFunction(2, DeviceFunction.PWM1);
            Configuration.SetPinFunction(18, DeviceFunction.SPI1_CLOCK);
            Configuration.SetPinFunction(23, DeviceFunction.SPI1_MOSI);

            // RTC settings
            Configuration.SetPinFunction(21, DeviceFunction.I2C1_DATA);
            Configuration.SetPinFunction(22, DeviceFunction.I2C1_CLOCK);
            I2cConnectionSettings settings = new(1, Pcf8563.DefaultI2cAddress);
            _device = I2cDevice.Create(settings);
        }

        /// <summary>
        /// Gets an ADC channel.
        /// </summary>
        /// <param name="gpioNumber">The GPIO pin number.</param>
        /// <returns>An AdcChannel</returns>
        public static AdcChannel GetAdcGpio(int gpioNumber)
        {
            _adc ??= new();

            switch (gpioNumber)
            {
                case 35:
                    Configuration.SetPinFunction(35, DeviceFunction.ADC1_CH7);
                    return _adc.OpenChannel(7);
                case 36:
                    Configuration.SetPinFunction(36, DeviceFunction.ADC1_CH0);
                    return _adc.OpenChannel(0);
                case 2:
                    Configuration.SetPinFunction(2, DeviceFunction.ADC1_CH12);
                    return _adc.OpenChannel(12);
                case 12:
                    Configuration.SetPinFunction(12, DeviceFunction.ADC1_CH15);
                    return _adc.OpenChannel(15);
                case 15:
                    Configuration.SetPinFunction(15, DeviceFunction.ADC1_CH13);
                    return _adc.OpenChannel(13);
                case 25:
                    Configuration.SetPinFunction(25, DeviceFunction.ADC1_CH18);
                    return _adc.OpenChannel(18);
                case 26:
                    Configuration.SetPinFunction(26, DeviceFunction.ADC1_CH19);
                    return _adc.OpenChannel(19);
                case 13:
                    Configuration.SetPinFunction(13, DeviceFunction.ADC1_CH14);
                    return _adc.OpenChannel(14);
                case 0:
                    Configuration.SetPinFunction(0, DeviceFunction.ADC1_CH11);
                    return _adc.OpenChannel(11);
                case 34:
                    Configuration.SetPinFunction(34, DeviceFunction.ADC1_CH6);
                    return _adc.OpenChannel(6);
                default:
                    throw new ArgumentException(nameof(gpioNumber));
            }
        }

        /// <summary>
        /// Initialize the eInk screen.
        /// </summary>
        /// <returns>An instance of the <see cref="Gdew0154m09"/> driver.</returns>
        private static void InitializeScreen()
        {
            if (_screen == null)
            {
                var spiConnectionSettings = new SpiConnectionSettings(busId: 1, chipSelectLine: 9)
                {
                    ClockFrequency = Gdew0154m09.SpiClockFrequency,
                    Mode = Gdew0154m09.SpiMode,
                    ChipSelectLineActiveState = PinValue.Low,
                    Configuration = SpiBusConfiguration.HalfDuplex,
                    DataFlow = DataFlow.MsbFirst,
                };

                _spi = new SpiDevice(spiConnectionSettings);
                _screen = new Gdew0154m09(_spi, ScreenResetPin, ScreenBusyPin, ScreenDCPin, _gpio);
            }
        }
    }
}
