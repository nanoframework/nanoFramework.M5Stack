// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Iot.Device.Axp192;
using Iot.Device.Rtc;
using nanoFramework.Hardware.Esp32;
using nanoFramework.M5Stack;
using nanoFramework.M5Core2;
using nanoFramework.Runtime.Native;
using System;
using System.Device.Adc;
using System.Device.I2c;
using UnitsNet;
using System.Device.Gpio;
using System.IO.Ports;
using Iot.Device.Button;

namespace nanoFramework.M5Stack
{   
    public static partial class M5Core2
    {
        private static Pcf8563 _rtc;
        private static Axp192 _power;
        private static bool _powerLed;
        private static bool _vibrate;
        private static GpioButton _touchPanel;

        /// <summary>
        /// Gets the power management of the M5Core2.
        /// </summary>
        /// <remarks>Please make sure to read the documentation before adjusting any element.</remarks>
        public static Axp192 Power { get => _power; }

        /// <summary>
        /// Gets the real time clock.
        /// </summary>
        public static Pcf8563 ReatTimeClock
        {
            get => _rtc;
        }

        /// <summary>
        /// Sets on or off the Power Led.
        /// </summary>
        public static bool PowerLed
        {
            get => _powerLed;
            set
            {
                _powerLed = value;
                _power.EnableLDO2(_powerLed);
            }
        }

        /// <summary>
        /// Vibrate the M5Core2 when true.
        /// </summary>
        public static bool Vibrate
        {
            get => _vibrate;
            set
            {
                _vibrate = value;
                _power.EnableLDO3(_vibrate);
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
        /// Gets the TouchPanel.
        /// </summary>
        /// <remarks>Core2 TouchPanel is firstly just treated as a whole button i.e. exactly as the Core LeftButton as the Touch chip FT6336U INT output uses de same GPIO 39.The pooling or event handling when the TouchPanel has been pressing to extract the X-Y coordinates from the FT6336U via I2C bus (1) must be handled somewhere else</remarks>
        public static GpioButton TouchPanel
        {
            get
            {
                if (_touchPanel == null)
                {
                    _touchPanel = new(39, _gpio, false);
                    
                }

                return _touchPanel;
            }
        }

        static M5Core2()
        {
            // Setup first the I2C bus
            Configuration.SetPinFunction(22, DeviceFunction.I2C1_CLOCK);
            Configuration.SetPinFunction(21, DeviceFunction.I2C1_DATA);
            
            //Setup the external I2C bus (Port A on Core2)
            Configuration.SetPinFunction(33, DeviceFunction.I2C2_CLOCK);
            Configuration.SetPinFunction(32, DeviceFunction.I2C2_DATA);

            // Create the energy management device
            I2cDevice i2c = new(new I2cConnectionSettings(1, Axp192.I2cDefaultAddress));
            _power = new(i2c);
            
            //TO-DO, create a new I2cDevice also for Core2 I2C Port A

            // Configuration for M5Core2
            // AXP Vbus limit off
            _power.SetVbusSettings(false, false, VholdVoltage.V4_0, true, VbusCurrentLimit.MilliAmper100);
            // AXP192 GPIO1 and 2:OD OUTPUT
            _power.Gpio1Behavior = Gpio12Behavior.MnosLeakOpenOutput;
            _power.Gpio2Behavior = Gpio12Behavior.MnosLeakOpenOutput;
            // Enable RTC BAT charge 
            _power.SetBackupBatteryChargingControl(true, BackupBatteryCharingVoltage.V3_0, BackupBatteryChargingCurrent.MicroAmperes200);
            // Sets the ESP voltage
            _power.DcDc1Volvate = ElectricPotential.FromVolts(3.35);
            // Sets the LCD Voltage to 2.8V
            _power.DcDc3Volvate = ElectricPotential.FromVolts(2.8);
            // Sets the SD Card voltage
            _power.LDO2OutputVoltage = ElectricPotential.FromVolts(3.3);
            _power.EnableLDO2(true);
            // Sets the Vibrator voltage
            _power.LDO3OutputVoltage = ElectricPotential.FromVolts(2.0);
            // Bat charge voltage to 4.2, Current 100MA
            _power.SetChargingFunctions(true, ChargingVoltage.V4_2, ChargingCurrent.Current100mA, ChargingStopThreshold.Percent10);
            // Set ADC sample rate to 200hz
            _power.AdcFrequency = AdcFrequency.Frequency200Hz;
            _power.AdcPinCurrent = AdcPinCurrent.MicroAmperes80;
            _power.BatteryTemperatureMonitoring = true;
            _power.AdcPinCurrentSetting = AdcPinCurrentSetting.AlwaysOn;
            // Set ADC1 Enable
            _power.AdcPinEnabled= AdcPinEnabled.All;
            // Switch on the power led
            PowerLed = true;
            // Set GPIO4 as output (rest LCD)
            _power.Gpio4Behavior = Gpio4Behavior.MnosLeakOpenOutput;
            // 128ms power on, 4s power off
            _power.SetButtonBehavior(LongPressTiming.S1, ShortPressTiming.Ms128, true, SignalDelayAfterPowerUp.Ms64, ShutdownTiming.S10);
            // Set temperature protection
            _power.SetBatteryHighTemperatureThreshold(ElectricPotential.FromVolts(3.2256));
            // Enable bat detection
            _power.SetShutdownBatteryDetectionControl(false, true, ShutdownBatteryPinFunction.HighResistance, true, ShutdownBatteryTiming.S2);
            // Set Power off voltage 3.0v            
            _power.VoffVoltage = VoffVoltage.V3_0;
            // This part of the code will handle the button behavior
            _power.EnableButtonPressed(ButtonPressed.LongPressed | ButtonPressed.ShortPressed);
            _power.SetButtonBehavior(LongPressTiming.S2, ShortPressTiming.Ms128, true, SignalDelayAfterPowerUp.Ms32, ShutdownTiming.S10);

            // Setup buttons
            _gpio = new();

            // Setup SPI1
            Configuration.SetPinFunction(23, DeviceFunction.SPI1_MOSI);
            Configuration.SetPinFunction(38, DeviceFunction.SPI1_MISO);
            Configuration.SetPinFunction(18, DeviceFunction.SPI1_CLOCK);

            // Setup the screen with SP2 and SD Card
            Configuration.SetPinFunction(23, DeviceFunction.SPI2_MOSI);
            Configuration.SetPinFunction(38, DeviceFunction.SPI2_MISO);
            Configuration.SetPinFunction(18, DeviceFunction.SPI2_CLOCK);

            // Second serial port
            Configuration.SetPinFunction(13, DeviceFunction.COM2_RX);
            Configuration.SetPinFunction(14, DeviceFunction.COM2_TX);

            // Setup the time if any
            _rtc = new Pcf8563(I2cDevice.Create(new I2cConnectionSettings(1, Pcf8563.DefaultI2cAddress)));

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

                if (sysDtcore.Year < 2021)
                {
                    dt = new DateTime(2021, 11, 03, 12, 00, 00);
                    _rtc.DateTime = dt;
                    Rtc.SetSystemTime(dt);
                }
            }
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
                //On Core2 GPIO 32 is external I2C port A PA_SDA and GPIO 33 PA_SCL , therefore corresponding case 32 deleted
                
                //On Core2 GPIO 39 is used as INT output from Touch pannel FT6336U chip , , therefore corresponding case 39 deleted
                    
                case 0:
                    Configuration.SetPinFunction(0, DeviceFunction.ADC1_CH11);
                    return _adc.OpenChannel(11);
                case 2:
                    Configuration.SetPinFunction(2, DeviceFunction.ADC1_CH12);
                    return _adc.OpenChannel(12);
                case 4:
                    Configuration.SetPinFunction(4, DeviceFunction.ADC1_CH10);
                    return _adc.OpenChannel(10);
                case 12:
                    Configuration.SetPinFunction(12, DeviceFunction.ADC1_CH15);
                    return _adc.OpenChannel(15);
                case 15:
                    Configuration.SetPinFunction(15, DeviceFunction.ADC1_CH13);
                    return _adc.OpenChannel(13);
                case 25:
                    Configuration.SetPinFunction(25, DeviceFunction.ADC1_CH18);
                    return _adc.OpenChannel(18);
                case 27:
                    Configuration.SetPinFunction(26, DeviceFunction.ADC1_CH17);
                    return _adc.OpenChannel(17);
                default:
                    throw new ArgumentException(nameof(gpioNumber));
            }
        }
    }
}
