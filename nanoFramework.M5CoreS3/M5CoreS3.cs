// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Iot.Device.Aw9523x;
using Iot.Device.Axp2101;
using Iot.Device.Bmi270;
using Iot.Device.Ft6xx6x;
using Iot.Device.Ltr553AlsWa;
using Iot.Device.Magnetometer;
using Iot.Device.Rtc;
using nanoFramework.Hardware.Esp32;
using nanoFramework.Runtime.Native;
using nanoFramework.System.IO.FileSystem;
using System;
using System.Device.Adc;
using System.Device.Gpio;
using System.Device.I2c;
using System.IO.Ports;
using System.Threading;
using UnitsNet;

namespace nanoFramework.M5Stack
{
    /// <summary>
    /// M5Stack CoreS3 board.
    /// </summary>
    public static partial class M5CoreS3
    {
        /// <summary>
        /// Internal I2C bus identifier used by on-board peripherals.
        /// </summary>
        public const int InternalI2cBusId = 1;

        /// <summary>
        /// SDA pin used by the internal I2C bus.
        /// </summary>
        public const int InternalI2cSdaPin = 12;

        /// <summary>
        /// SCL pin used by the internal I2C bus.
        /// </summary>
        public const int InternalI2cSclPin = 11;

        /// <summary>
        /// External Grove I2C bus identifier.
        /// </summary>
        public const int ExternalI2cBusId = 2;

        /// <summary>
        /// SDA pin used by the external Grove I2C bus.
        /// </summary>
        public const int ExternalI2cSdaPin = 2;

        /// <summary>
        /// SCL pin used by the external Grove I2C bus.
        /// </summary>
        public const int ExternalI2cSclPin = 1;

        private static Axp2101 _power;
        private static I2cDevice _ioExpanderI2c;
        private static Aw9523x _ioExpander;
        private static Pcf8563 _rtc;
        private static Bmi270AccelerometerGyroscope _accelerometerGyroscope;
        private static Bmm150 _magnetometer;
        private static Ltr553AlsWa _lightProximitySensor;
        private static Ft6xx6x _touchController;
        private static SDCard _sdCard;
        private static SerialPort _serialPort;
        private static AdcController _adc;
        private static GpioController _gpio;
        private static Screen _screen;

        static M5CoreS3()
        {
            ConfigureInternalI2cBus();
            ConfigureExternalI2cBus();

            _ioExpanderI2c = CreateInternalI2cDevice(Aw9523x.DefaultI2cAddress);
            _ioExpander = new(_ioExpanderI2c);
            EnableInternalBusPower();

            _power = new(CreateInternalI2cDevice(Axp2101.I2cDefaultAddress));
            EnableSensorPower();
            _rtc = new(CreateInternalI2cDevice(Pcf8563.DefaultI2cAddress));

            DateTime dt;
            var sysDtcore = DateTime.UtcNow;
            try
            {
                dt = _rtc.DateTime;
                if (sysDtcore < dt)
                {
                    Rtc.SetSystemTime(dt);
                }
            }
            catch (Exception)
            {
                if (sysDtcore.Year < 2026)
                {
                    dt = new DateTime(2026, 06, 21, 00, 00, 00);
                    _rtc.DateTime = dt;
                    Rtc.SetSystemTime(dt);
                }
            }
        }

        private static void ConfigureInternalI2cBus()
        {
            Configuration.SetPinFunction(InternalI2cSdaPin, DeviceFunction.I2C1_DATA);
            Configuration.SetPinFunction(InternalI2cSclPin, DeviceFunction.I2C1_CLOCK);
        }

        /// <summary>
        /// Configures the external Grove I2C bus.
        /// </summary>
        private static void ConfigureExternalI2cBus()
        {
            Configuration.SetPinFunction(ExternalI2cSdaPin, DeviceFunction.I2C2_DATA);
            Configuration.SetPinFunction(ExternalI2cSclPin, DeviceFunction.I2C2_CLOCK);
        }

        private static I2cDevice CreateInternalI2cDevice(int address)
        {
            return I2cDevice.Create(new I2cConnectionSettings(InternalI2cBusId, address));
        }

        /// <summary>
        /// Gets an I2C device on the CoreS3 internal bus.
        /// </summary>
        /// <param name="i2cDeviceAddress">The target device address.</param>
        /// <returns>An initialized I2C device.</returns>
        public static I2cDevice GetI2cDevice(int i2cDeviceAddress) => CreateInternalI2cDevice(i2cDeviceAddress);

        /// <summary>
        /// Gets an I2C device on the external Grove bus.
        /// </summary>
        /// <param name="i2cDeviceAddress">The target device address.</param>
        /// <returns>An initialized I2C device.</returns>
        public static I2cDevice GetGrove(int i2cDeviceAddress)
        {
            ConfigureExternalI2cBus();
            return I2cDevice.Create(new I2cConnectionSettings(ExternalI2cBusId, i2cDeviceAddress));
        }

        /// <summary>
        /// Gets the CoreS3 power management device.
        /// </summary>
        public static Axp2101 Power => _power;

        /// <summary>
        /// Gets the CoreS3 IO expander device.
        /// </summary>
        public static Aw9523x IoExpander => _ioExpander;

        /// <summary>
        /// Gets the on-board RTC.
        /// </summary>
        public static Pcf8563 RealTimeClock => _rtc;

        /// <summary>
        /// Gets the main GPIO controller.
        /// </summary>
        public static GpioController GpioController
        {
            get
            {
                if (_gpio is null)
                {
                    _gpio = new();
                }

                return _gpio;
            }
        }

        /// <summary>
        /// Gets the BMI270 accelerometer and gyroscope.
        /// </summary>
        public static Bmi270AccelerometerGyroscope AccelerometerGyroscope
        {
            get
            {
                if (_accelerometerGyroscope == null)
                {
                    _accelerometerGyroscope = new(CreateInternalI2cDevice(Bmi270AccelerometerGyroscope.SecondaryI2cAddress));
                    _accelerometerGyroscope.EnableAuxiliaryI2c(Bmm150.SecondaryI2cAddress);
                }

                return _accelerometerGyroscope;
            }
        }

        /// <summary>
        /// Gets the BMM150 magnetometer through the BMI270 auxiliary bus.
        /// </summary>
        public static Bmm150 Magnetometer
        {
            get
            {
                if (_magnetometer == null)
                {
                    var imu = AccelerometerGyroscope;
                    _magnetometer = new(CreateInternalI2cDevice(Bmi270AccelerometerGyroscope.SecondaryI2cAddress), new Bmm150I2cBmi270(Bmm150.SecondaryI2cAddress));
                }

                return _magnetometer;
            }
        }

        /// <summary>
        /// Gets the on-board ambient light and proximity sensor.
        /// </summary>
        public static Ltr553AlsWa LightProximitySensor
        {
            get
            {
                if (_lightProximitySensor == null)
                {
                    _lightProximitySensor = new(CreateInternalI2cDevice(Ltr553AlsWa.DefaultI2cAddress));
                }

                return _lightProximitySensor;
            }
        }

        /// <summary>
        /// Gets the FT6336U touch controller.
        /// </summary>
        public static Ft6xx6x TouchController
        {
            get
            {
                if (_touchController == null)
                {
                    _touchController = new(CreateInternalI2cDevice(Ft6xx6x.DefaultI2cAddress));
                }

                return _touchController;
            }
        }

        /// <summary>
        /// Gets the SD Card.
        /// </summary>
        public static SDCard SDCard
        {
            get
            {
                if (_sdCard == null)
                {
                    // We always have this configuration for all the M5Core, M5Core2, Fire and even Tough
                    _sdCard = new SDCard(new SDCardSpiParameters() { spiBus = 1, chipSelectPin = 4 });
                }

                return _sdCard;
            }
        }

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
        /// Gets the ADC Controller.
        /// </summary>
        public static AdcController Adc
        {
            get
            {
                // We do this so the ADC is used only if needed
                if (_adc == null)
                {
                    _adc = new AdcController();
                }

                return _adc;
            }
        }

        private static void EnableInternalBusPower()
        {
            if (IoExpander.ChipId != Aw9523x.ExpectedChipId)
            {
                throw new InvalidOperationException("Unexpected AW9523 chip ID.");
            }

            // Put both AW9523 ports in GPIO mode as done by the vendor CoreS3 init path.
            WriteRegister(_ioExpanderI2c, 0x12, 0xFF);
            WriteRegister(_ioExpanderI2c, 0x13, 0xFF);

            IoExpander.SetOutputBits(Port.Port0, OutputMask.PortBit1);
            IoExpander.SetOutputBits(Port.Port1, OutputMask.PortBit7);

            Thread.Sleep(10);
        }

        private static void EnableSensorPower()
        {
            if (Power.GetChipId() != Axp2101.ChipId)
            {
                throw new InvalidOperationException("Unexpected AXP2101 chip ID.");
            }

            Power.DcDc1Voltage = ElectricPotential.FromVolts(3.3);
            Power.EnableDcDc1();
            Power.Aldo1Voltage = ElectricPotential.FromVolts(1.8);
            Power.EnableAldo1();
            Power.Aldo2Voltage = ElectricPotential.FromVolts(3.3);
            Power.EnableAldo2();
            Power.Aldo3Voltage = ElectricPotential.FromVolts(3.3);
            Power.EnableAldo3();
            Power.Aldo4Voltage = ElectricPotential.FromVolts(3.3);
            Power.EnableAldo4();
            Power.Bldo1Voltage = ElectricPotential.FromVolts(3.3);
            Power.EnableBldo1();
            Power.Bldo2Voltage = ElectricPotential.FromVolts(3.3);
            Power.EnableBldo2();

            Thread.Sleep(20);
        }

        /// <summary>
        /// Initializes the CoreS3 display using the managed Ili9342 driver.
        /// </summary>
        /// <param name="memoryBitMapAllocation">The bitmap buffer allocation.</param>
        public static void InitializeScreen(int memoryBitMapAllocation = Screen.DefaultMemoryAllocationBitmap)
        {
            if (_screen == null)
            {
                _screen = new(memoryBitMapAllocation);
            }
        }

        internal static void SetLcdReset(bool high)
        {
            byte direction = IoExpander.ReadDirectionPort(Port.Port1);
            IoExpander.WriteDirectionPort(Port.Port1, (byte)(direction & ~(byte)OutputMask.PortBit1));
            IoExpander.UpdateOutputBits(Port.Port1, OutputMask.PortBit1, high);
        }

        internal static void SetScreenBacklight(byte brightnessPercentage)
        {
            if (brightnessPercentage == 0)
            {
                Power.DisableDldo1();
                return;
            }

            Power.Dldo1Voltage = ElectricPotential.FromMillivolts((((brightnessPercentage * 255) / 100 + 641) >> 5) * 100 + 500);
            Power.EnableDldo1();
        }

        private static void WriteRegister(I2cDevice device, byte register, byte value)
        {
            device.Write(new byte[] { register, value });
        }
    }
}