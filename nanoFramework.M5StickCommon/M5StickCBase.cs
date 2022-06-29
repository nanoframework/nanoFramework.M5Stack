// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Device.I2c;
using Iot.Device.Axp192;
using nanoFramework.Hardware.Esp32;
using UnitsNet;
using Iot.Device.Button;
using System.Device.Gpio;
using nanoFramework.Hardware.Esp32.Rmt;
using Iot.Device.Mpu6886;
using Iot.Device.Rtc;
using nanoFramework.Runtime.Native;
#if M5STICKC
using nanoFramework.M5StickC;
#else
using nanoFramework.M5StickCPlus;
#endif
namespace nanoFramework.M5Stack
{
    /// <summary>
    /// The base class for both M5STickC and M5StickCPlus
    /// </summary>
#if M5STICKC
    public static partial class M5StickC
#else
    public static partial class M5StickCPlus
#endif
    {
        private static Axp192 _power;
        private static GpioButton _buttonM5;
        private static GpioButton _buttonRight;
        private static GpioController _gpio;
        private static GpioPin _led;
        private static TransmitterChannel _irLed;
        private static Mpu6886AccelerometerGyroscope _accelerometer;
        private static Pcf8563 _rtc;
        private static Screen _screen;

        /// <summary>
        /// Gets the AXP192 power management. Please check the detailed documentation before adjusting it.
        /// </summary>
        public static Axp192 Power { get => _power; }

        /// <summary>
        /// Gets the button marked M5 below the screen
        /// </summary>
        public static GpioButton ButtonM5
        {
            get
            {
                if (_buttonM5 == null)
                {
                    _buttonM5 = new(37, _gpio, false);
                }

                return _buttonM5;
            }
        }

        /// <summary>
        /// Gets the button on the right side.
        /// </summary>
        public static GpioButton ButtonRight
        {
            get
            {
                if (_buttonRight == null)
                {
                    _buttonRight = new(39, _gpio, false);
                }

                return _buttonRight;
            }
        }

        /// <summary>
        /// Gets the embedded red led.
        /// </summary>
        public static GpioPin Led
        {
            get
            {
                if (_led == null)
                {
                    _led = _gpio.OpenPin(10, PinMode.Output);
                }

                return _led;
            }
        }

        /// <summary>
        /// Gets the infrared led as a RMT transmitter channel.
        /// </summary>
        public static TransmitterChannel InfraredLed
        {
            get
            {
                if (_irLed == null)
                {
                    _irLed = new(9);
                }

                return _irLed;
            }
        }

        /// <summary>
        /// Gets the Accelerometer and Gyroscope.
        /// </summary>
        public static Mpu6886AccelerometerGyroscope AccelerometerGyroscope
        {
            get
            {
                if (_accelerometer == null)
                {
                    _accelerometer = new(new I2cDevice(new I2cConnectionSettings(1, Mpu6886AccelerometerGyroscope.DefaultI2cAddress)));
                }

                return _accelerometer;
            }
        }

        /// <summary>
        /// Gets the real time clock.
        /// </summary>
        public static Pcf8563 RealTimeClock
        {
            get => _rtc;
        }

        /// <summary>
        /// Gets the main GPIO Controller.
        /// </summary>
        public static GpioController GpioController => _gpio;

#if M5STICKC
        static M5StickC()
#else
        static M5StickCPlus()
#endif
        {
            // Setup I2C
            Configuration.SetPinFunction(22, DeviceFunction.I2C1_CLOCK);
            Configuration.SetPinFunction(21, DeviceFunction.I2C1_DATA);

            // Setup the power management AXP192
            I2cDevice i2cAxp192 = new(new I2cConnectionSettings(1, Axp192.I2cDefaultAddress));
            _power = new Axp192(i2cAxp192);
            // Set ADC sample rate to 200hz
            _power.AdcFrequency = AdcFrequency.Frequency200Hz;
            _power.AdcPinCurrent = AdcPinCurrent.MicroAmperes80;
            _power.BatteryTemperatureMonitoring = true;
            _power.AdcPinCurrentSetting = AdcPinCurrentSetting.AlwaysOn;
            // Set ADC to All Enable
            _power.AdcPinEnabled = AdcPinEnabled.All;
            // Bat charge voltage to 4.2, Current 100MA
            _power.SetChargingFunctions(true, ChargingVoltage.V4_2, ChargingCurrent.Current100mA, ChargingStopThreshold.Percent10);
            // Depending on configuration enable LDO2, LDO3, DCDC1, DCDC3.
            _power.LdoDcPinsEnabled = LdoDcPinsEnabled.All;
            // 128ms power on, 4s power off
            _power.SetButtonBehavior(LongPressTiming.S1, ShortPressTiming.Ms128, true, SignalDelayAfterPowerUp.Ms64, ShutdownTiming.S10);
            // Set RTC voltage to 3.3V
            _power.PinOutputVoltage = PinOutputVoltage.V3_3;
            // Set GPIO0 to LDO
            _power.Gpio0Behavior = Gpio0Behavior.LowNoiseLDO;
            // Disable vbus hold limit
            _power.SetVbusSettings(true, false, VholdVoltage.V4_0, false, VbusCurrentLimit.MilliAmper500);
            // Set temperature protection
            _power.SetBatteryHighTemperatureThreshold(ElectricPotential.FromVolts(3.2256));
            // Enable RTC BAT charge 
            _power.SetBackupBatteryChargingControl(true, BackupBatteryCharingVoltage.V3_0, BackupBatteryChargingCurrent.MicroAmperes200);
            // Enable bat detection
            _power.SetShutdownBatteryDetectionControl(false, true, ShutdownBatteryPinFunction.HighResistance, true, ShutdownBatteryTiming.S2);
            // Set Power off voltage 3.0v            
            _power.VoffVoltage = VoffVoltage.V3_0;
            // This part of the code will handle the button behavior
            _power.EnableButtonPressed(ButtonPressed.LongPressed | ButtonPressed.ShortPressed);
            _power.SetButtonBehavior(LongPressTiming.S2, ShortPressTiming.Ms128, true, SignalDelayAfterPowerUp.Ms32, ShutdownTiming.S10);

            // Set the Grove port
            Configuration.SetPinFunction(33, DeviceFunction.I2C2_CLOCK);
            Configuration.SetPinFunction(32, DeviceFunction.I2C2_DATA);

            // Set the pins for the screen
            Configuration.SetPinFunction(15, DeviceFunction.SPI2_MOSI);
            Configuration.SetPinFunction(13, DeviceFunction.SPI2_CLOCK);
            // This is not used but must be defined
            Configuration.SetPinFunction(4, DeviceFunction.SPI2_MISO);

            // Setup the time if any
            _rtc = new Pcf8563(I2cDevice.Create(new I2cConnectionSettings(1, Pcf8563.DefaultI2cAddress)));

            DateTime dt;
            var sysDt = DateTime.UtcNow;
            try
            {
                dt = _rtc.DateTime;
                if (sysDt < dt)
                {
                    Rtc.SetSystemTime(dt);
                }
            }
            catch (Exception)
            {

                if (sysDt.Year < 2021)
                {
                    dt = new DateTime(2021, 11, 01, 12, 00, 00);
                    _rtc.DateTime = dt;
                    Rtc.SetSystemTime(dt);
                }
            }
        }


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
                Console.Font = Resources.GetFont(Resources.FontResources.consolas_regular_8);
            }
        }

        /// <summary>
        /// Gets a Grove I2C device.
        /// </summary>
        /// <param name="i2cDeviceAddress">The I2C device address on the bus.</param>
        /// <returns>The I2cDevice.</returns>
        public static I2cDevice GetGrove(int i2cDeviceAddress) => new(new I2cConnectionSettings(2, i2cDeviceAddress));
    }
}
