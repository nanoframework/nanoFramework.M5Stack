// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Iot.Device.Bmi270;
using Iot.Device.Button;
using Iot.Device.M5Pm1;
using nanoFramework.Hardware.Esp32;
using nanoFramework.Runtime.Native;
using System.Device.Gpio;
using System.Device.I2c;
using System.Diagnostics;
using UnitsNet;

namespace nanoFramework.M5Stack
{
    /// <summary>
    /// M5StickS3 board support.
    /// </summary>
    public static class M5StickS3
    {
        /// <summary>
        /// The internal I2C bus ID for the PMIC and accelerometer/gyroscope.
        /// </summary>
        public const int InternalI2cBusId = 1;

        private const int InternalI2cSdaPin = 47;
        private const int InternalI2cSclPin = 48;

        /// <summary>
        /// The internal I2C bus ID for the Grove connector.
        /// </summary>
        public const int GroveI2cBusId = 2;

        /// <summary>
        /// The Grove connector SDA pin.
        /// </summary>
        public const int GroveI2cSdaPin = 9;

        /// <summary>
        /// The Grove connector SCL pin.
        /// </summary>
        public const int GroveI2cSclPin = 10;

        private const int ButtonAPin = 11;
        private const int ButtonBPin = 12;

        private static M5Pm1 _pmic;
        private static GpioController _gpio;
        private static GpioButton _buttonA;
        private static GpioButton _buttonB;
        private static Bmi270AccelerometerGyroscope _accelerometerGyroscope;
        private static Screen _screen;

        static M5StickS3()
        {
            Configuration.SetPinFunction(InternalI2cSdaPin, DeviceFunction.I2C1_DATA);
            Configuration.SetPinFunction(InternalI2cSclPin, DeviceFunction.I2C1_CLOCK);
            Configuration.SetPinFunction(GroveI2cSdaPin, DeviceFunction.I2C2_DATA);
            Configuration.SetPinFunction(GroveI2cSclPin, DeviceFunction.I2C2_CLOCK);

            // M5PM1 requires a 100 kHz I2C bus on M5StickS3.
            I2cConnectionSettings pmicSettings = new I2cConnectionSettings(InternalI2cBusId, M5Pm1.I2cDefaultAddress, I2cBusSpeed.StandardMode);
            _pmic = new M5Pm1(new I2cDevice(pmicSettings));
            int deviceId = _pmic.GetDeviceId();
            Debug.WriteLine($"M5PM1 device ID: 0x{deviceId:X4} (expected 0x{M5Pm1.DeviceId:X4}).");
            InitializePmic();
        }

        /// <summary>
        /// Gets the main GPIO controller.
        /// </summary>
        public static GpioController GpioController
        {
            get
            {
                if (_gpio == null)
                {
                    _gpio = new GpioController();
                }

                return _gpio;
            }
        }

        /// <summary>
        /// Gets the button below the display.
        /// </summary>
        public static GpioButton ButtonA
        {
            get
            {
                if (_buttonA == null)
                {
                    _buttonA = new GpioButton(ButtonAPin, GpioController, false);
                }

                return _buttonA;
            }
        }

        /// <summary>
        /// Gets the side button.
        /// </summary>
        public static GpioButton ButtonB
        {
            get
            {
                if (_buttonB == null)
                {
                    _buttonB = new GpioButton(ButtonBPin, GpioController, false);
                }

                return _buttonB;
            }
        }

        /// <summary>
        /// Gets the onboard accelerometer and gyroscope.
        /// </summary>
        public static Bmi270AccelerometerGyroscope AccelerometerGyroscope
        {
            get
            {
                if (_accelerometerGyroscope == null)
                {
                    // Important: keep standard mode because of the M5PM1
                    _accelerometerGyroscope = new Bmi270AccelerometerGyroscope(
                        new I2cDevice(new I2cConnectionSettings(
                            InternalI2cBusId,
                            Bmi270AccelerometerGyroscope.DefaultI2cAddress,
                            I2cBusSpeed.StandardMode)));
                }

                return _accelerometerGyroscope;
            }
        }

        /// <summary>
        /// Reads the battery voltage in millivolts from the PMIC.
        /// </summary>
        public static ElectricPotential BatteryVoltage => _pmic.GetBatteryVoltage();

        /// <summary>
        /// Reads the VBUS voltage in millivolts from the PMIC.
        /// </summary>
        public static ElectricPotential VBusVoltage => _pmic.GetVbusVoltage();

        /// <summary>
        /// Gets whether charging is currently active.
        /// </summary>
        public static bool IsCharging => _pmic.IsCharging;

        /// <summary>
        /// Gets or sets the 5V output state.
        /// </summary>
        public static bool External5VEnabled
        {
            get => _pmic.ExternalOutputEnabled;
            set => _pmic.ExternalOutputEnabled = value;
        }

        /// <summary>
        /// Gets the Power Management device.
        /// </summary>
        public static M5Pm1 Power => _pmic;

        /// <summary>
        /// Initializes the display.
        /// </summary>
        /// <param name="memoryBitMapAllocation">Bitmap allocation in bytes.</param>
        public static void InitializeScreen(int memoryBitMapAllocation = Screen.DefaultMemoryAllocationBitmap)
        {
            if (_screen == null)
            {
                try
                {
                    // PM1_G2 powers the LCD path.
                    _pmic.SetGpioFunction(Pin.Gpio2, GpioFunction.Gpio);
                    _pmic.SetGpioDrive(Pin.Gpio2, GpioDrive.PushPull);
                    _pmic.SetGpioMode(Pin.Gpio2, PinMode.Output);
                    _pmic.WriteGpio(Pin.Gpio2, PinValue.High);
                    _screen = new Screen(memoryBitMapAllocation);
                }
                catch (System.Exception ex)
                {
                    Debug.WriteLine($"M5StickS3.InitializeScreen failed: {ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets a device on the internal I2C bus.
        /// </summary>
        public static I2cDevice GetI2cDevice(int i2cDeviceAddress)
            => new I2cDevice(new I2cConnectionSettings(InternalI2cBusId, i2cDeviceAddress, I2cBusSpeed.StandardMode));

        /// <summary>
        /// Gets a device on the Grove I2C bus.
        /// </summary>
        public static I2cDevice GetGrove(int i2cDeviceAddress)
            => new I2cDevice(new I2cConnectionSettings(GroveI2cBusId, i2cDeviceAddress, I2cBusSpeed.FastMode));

        internal static void SetLcdPower(bool enabled)
        {
            _pmic.WriteGpio(Pin.Gpio2, enabled);
        }

        private static void InitializePmic()
        {
            // Constructor wake-up already applies the reliability init (I2C sleep off, watchdog off).

            // PM1_G3 is used by vendor firmware to gate the audio PA.
            _pmic.SetGpioFunction(Pin.Gpio3, GpioFunction.Gpio);
            _pmic.SetGpioDrive(Pin.Gpio3, GpioDrive.PushPull);
            _pmic.SetGpioMode(Pin.Gpio3, PinMode.Output);
            _pmic.WriteGpio(Pin.Gpio3, PinValue.Low);
        }
    }
}
