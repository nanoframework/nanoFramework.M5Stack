// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Iot.Device.Button;
using Iot.Device.Buzzer;
using Iot.Device.Ip5306;
using Iot.Device.Ws28xx.Esp32;
using nanoFramework.Fire;
using nanoFramework.Hardware.Esp32;
using System;
using System.Device.Adc;
using System.Device.I2c;
using UnitsNet;

namespace nanoFramework.M5Stack
{
    /// <summary>
    /// Fire board
    /// </summary>
    public static partial class Fire
    {
        private static readonly Ip5306 _power;
        private static GpioButton _left;
        private static GpioButton _center;
        private static GpioButton _right;
        private static Buzzer _buzzer;
        private static Sk6812 _rgbLed;

        #region properties        

        /// <summary>
        /// RGB Led bar containing 10 elements
        /// </summary>
        public static Sk6812 LedBar
        {
            get
            {
                if (_rgbLed == null)
                {
                    _rgbLed = new(15, 10);
                }

                return _rgbLed;
            }
        }

        /// <summary>
        /// Left button.
        /// </summary>
        public static GpioButton ButtonLeft
        {
            get
            {
                if (_left == null)
                {
                    _left = new(39, _gpio, false);
                }

                return _left;
            }
        }

        /// <summary>
        /// Center button.
        /// </summary>
        public static GpioButton ButtonCenter
        {
            get
            {
                if (_center == null)
                {
                    _center = new(38, _gpio, false);
                }

                return _center;
            }
        }

        /// <summary>
        /// Right button.
        /// </summary>
        public static GpioButton ButtonRight
        {
            get
            {
                if (_right == null)
                {
                    _right = new(37, _gpio, false);
                }

                return _right;
            }
        }

        /// <summary>
        /// Gets the power management of the M5 Stack.
        /// </summary>
        /// <remarks>Please make sure to read the documentation before adjusting any element.</remarks>
        public static Ip5306 Power { get => _power; }

        /// <summary>
        /// Gets the Buzzer.
        /// </summary>
        public static Buzzer Buzzer
        {
            get
            {
                // We do this in case you prefer to use the DAC channels which are using the same pins
                if (_buzzer == null)
                {
                    // Setup buzzer
                    Configuration.SetPinFunction(25, DeviceFunction.PWM1);
                    _buzzer = new(25);
                }

                return _buzzer;
            }
        }

        #endregion

        /// <summary>
        /// Gets the screen.
        /// </summary>
        /// <param name="memoryBitMapAllocation">The memory allocation.</param>
        /// <remarks>The screen initialization takes a little bit of time, if you need the screen consider using it as early as possible in your code.</remarks>
        public static void InitializeScreen(int memoryBitMapAllocation = Screen.DefaultMemoryAllocationBitmap)
        {
            // If the screen is not needed, it's not going to be created
            // Note: initialization may take a little bit of time
            if (_screen == null)
            {
                _screen = new(memoryBitMapAllocation);
                Console.Font = Resource.GetFont(Resource.FontResources.consolas_regular_16);
            }
        }

        static Fire()
        {
            // Setup first the I2C bus
            Configuration.SetPinFunction(22, DeviceFunction.I2C1_CLOCK);
            Configuration.SetPinFunction(21, DeviceFunction.I2C1_DATA);
            // Same for PortA than for the internal one
            _portANumber = 1;

            // Create the energy management device
            I2cDevice i2c = new(new I2cConnectionSettings(1, Ip5306.SecondaryI2cAddress));
            _power = new(i2c);

            // Configuration for M5Stack
            _power.ButtonOffEnabled = true;
            _power.BoostOutputEnabled = false;
            _power.AutoPowerOnEnabled = true;
            _power.ChargerEnabled = true;
            _power.BoostEnabled = true;
            _power.LowPowerOffEnabled = true;
            _power.FlashLightBehavior = ButtonPress.Doubleclick;
            _power.SwitchOffBoostBehavior = ButtonPress.LongPress;
            _power.BoostWhenVinUnpluggedEnabled = true;
            _power.ChargingUnderVoltage = ChargingUnderVoltage.V4_55;
            _power.ChargingLoopSelection = ChargingLoopSelection.Vin;
            _power.ChargingCurrent = ElectricCurrent.FromMilliamperes(2250);
            _power.ConstantChargingVoltage = ConstantChargingVoltage.Vm28;
            _power.ChargingCuttOffVoltage = ChargingCutOffVoltage.V4_17;
            _power.LightDutyShutdownTime = LightDutyShutdownTime.S32;
            _power.ChargingCutOffCurrent = ChargingCutOffCurrent.C500mA;
            _power.ChargingCuttOffVoltage = ChargingCutOffVoltage.V4_2;

            // Setup buttons
            _gpio = new();

            // Config GPIOs for SPI (screen and SD Card)
            Configuration.SetPinFunction(23, DeviceFunction.SPI1_MOSI);
            Configuration.SetPinFunction(19, DeviceFunction.SPI1_MISO);
            Configuration.SetPinFunction(18, DeviceFunction.SPI1_CLOCK);

            // Second serial port
            Configuration.SetPinFunction(16, DeviceFunction.COM2_RX);
            Configuration.SetPinFunction(17, DeviceFunction.COM2_TX);
        }

        /// <summary>
        /// Gets an ADC channel
        /// </summary>
        /// <param name="gpioNumber">The GPIO pin number</param>
        /// <returns>An AdcChannel</returns>
        public static AdcChannel GetAdcGpio(int gpioNumber)
        {
            if (_adc == null)
            {
                _adc = new();
            }

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
    }
}
