// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Iot.Device.Button;
using Iot.Device.Buzzer;
using Iot.Device.Ip5306;
using Iot.Device.Magnetometer;
using Iot.Device.Mpu6886;
using nanoFramework.Hardware.Esp32;
using nanoFramework.UI;
using System;
using System.Device.Dac;
using System.Device.Gpio;
using System.Device.I2c;
using System.Device.Spi;
using System.IO.Ports;
using UnitsNet;
using Windows.Devices.Adc;

namespace nanoFramework.M5Stack
{
    /// <summary>
    /// M5Stack board
    /// </summary>
    public static class M5Stack
    {
        private static Ip5306 _power;
        private static Bmm150 _bmm150;
        private static Mpu6886AccelerometerGyroscope _mpu6886;
        private static SerialPort _serialPort;
        private static Buzzer _buzzer;
        private static DacChannel _dac1;
        private static DacChannel _dac2;
        private static Screen _screen;
        private static GpioController _gpio;
        private static GpioButton _left;
        private static GpioButton _center;
        private static GpioButton _right;

        #region properties

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
        /// Gets the main GPIO Controller.
        /// </summary>
        public static GpioController GpioController => _gpio;

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
        /// Gets the screen.
        /// </summary>
        /// <remarks>The screen initialization takes a little bit of time, if you need the screen consider using it as early as possible in your code.</remarks>
        public static void InitializeScreen()
        {
            // If the screen is not needed, it's not going to be created
            // Note: initialization may take a little bit of time
            if (_screen == null)
            {
                _screen = new();
                Console.Font = Resource.GetFont(Resource.FontResources.consolas_regular_16);
            }
        }

        /// <summary>
        /// Gets the power management of the M5 Stack.
        /// </summary>
        /// <remarks>Please make sure to read the documentation before adjusting any element.</remarks>
        public static Ip5306 Power { get => _power; }

        #endregion

        static M5Stack()
        {
            // Setup first the I2C bus
            Configuration.SetPinFunction(22, DeviceFunction.I2C1_CLOCK);
            Configuration.SetPinFunction(21, DeviceFunction.I2C1_DATA);

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

            // Setup SPI1
            Configuration.SetPinFunction(23, DeviceFunction.SPI1_MOSI);
            Configuration.SetPinFunction(19, DeviceFunction.SPI1_MISO);
            Configuration.SetPinFunction(8, DeviceFunction.SPI1_CLOCK);

            // Setup the screen with SP2 and SD Card
            Configuration.SetPinFunction(23, DeviceFunction.SPI2_MOSI);
            Configuration.SetPinFunction(19, DeviceFunction.SPI2_MISO);
            Configuration.SetPinFunction(18, DeviceFunction.SPI2_CLOCK);

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
            switch (gpioNumber)
            {
                case 35:
                    Configuration.SetPinFunction(35, DeviceFunction.ADC1_CH7);
                    return AdcController.GetDefault().OpenChannel(7);
                case 36:
                    Configuration.SetPinFunction(36, DeviceFunction.ADC1_CH0);
                    return AdcController.GetDefault().OpenChannel(0);
                case 2:
                    Configuration.SetPinFunction(2, DeviceFunction.ADC1_CH12);
                    return AdcController.GetDefault().OpenChannel(12);
                case 12:
                    Configuration.SetPinFunction(12, DeviceFunction.ADC1_CH15);
                    return AdcController.GetDefault().OpenChannel(15);
                case 15:
                    Configuration.SetPinFunction(15, DeviceFunction.ADC1_CH13);
                    return AdcController.GetDefault().OpenChannel(13);
                case 25:
                    Configuration.SetPinFunction(25, DeviceFunction.ADC1_CH18);
                    return AdcController.GetDefault().OpenChannel(18);
                case 26:
                    Configuration.SetPinFunction(26, DeviceFunction.ADC1_CH19);
                    return AdcController.GetDefault().OpenChannel(19);
                case 13:
                    Configuration.SetPinFunction(13, DeviceFunction.ADC1_CH14);
                    return AdcController.GetDefault().OpenChannel(14);
                case 0:
                    Configuration.SetPinFunction(0, DeviceFunction.ADC1_CH11);
                    return AdcController.GetDefault().OpenChannel(11);
                case 34:
                    Configuration.SetPinFunction(34, DeviceFunction.ADC1_CH6);
                    return AdcController.GetDefault().OpenChannel(6);
                default:
                    throw new ArgumentException(nameof(gpioNumber));
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
    }
}
