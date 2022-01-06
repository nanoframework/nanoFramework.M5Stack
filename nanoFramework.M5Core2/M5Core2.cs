// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Iot.Device.Axp192;
using Iot.Device.Ft6xx6x;
using Iot.Device.Rtc;
using nanoFramework.Hardware.Esp32;
using nanoFramework.M5Core2;
using nanoFramework.Runtime.Native;
using System;
using System.Device.Adc;
using System.Device.I2c;
using System.Device.Gpio;
using UnitsNet;
using System.Threading;
using nanoFramework.Runtime.Events;

namespace nanoFramework.M5Stack
{
    public static partial class M5Core2
    {
        private const int TouchPinInterrupt = 39;
        private static Pcf8563 _rtc;
        private static Axp192 _power;
        private static bool _powerLed;
        private static bool _vibrate;
        private static Ft6xx6x _touchController;
        private static Thread _touchThrad;

        /// <summary>
        /// Touch event handler for the touch event.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The touch event argument.</param>
        public delegate void TouchEventHandler(object sender, TouchEventArgs e);

        /// <summary>
        /// Touch event handler.
        /// </summary>
        public static event TouchEventHandler TouchEvent;

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
        /// Gets the touch controller.
        /// </summary>
        public static Ft6xx6x TouchController
        {
            get
            {
                if (_touchController == null)
                {
                    InitializeScreen();
                }

                return _touchController;
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
                _touchController = new(I2cDevice.Create(new I2cConnectionSettings(1, Ft6xx6x.DefaultI2cAddress)));
                _touchController.SetInterruptMode(false);
                _touchThrad = new(ThreadTouchCallback);
                _gpio.OpenPin(TouchPinInterrupt, PinMode.Input);
                _gpio.RegisterCallbackForPinValueChangedEvent(TouchPinInterrupt, PinEventTypes.Rising | PinEventTypes.Falling, TouchCallback);
            }
        }

        private static void TouchCallback(object sender, PinValueChangedEventArgs pinValueChangedEventArgs)
        {
            if (pinValueChangedEventArgs.ChangeType == PinEventTypes.Falling)
            {
                if (_touchThrad.ThreadState == ThreadState.Unstarted)
                {
                    _touchThrad.Start();
                }
                else
                {
                    _touchThrad.Resume();
                }
            }
            else
            {
                _touchThrad.Suspend();
                var point = _touchController.GetPoint(true);
                var touchCategory = CheckIfInButtons(point.X, point.Y, TouchEventCategory.Unknown) | TouchEventCategory.LiftUp;                
                TouchEvent?.Invoke(_touchController, new TouchEventArgs() { TimeStamp = DateTime.UtcNow, EventCategory = EventCategory.Touch, TouchEventCategory = touchCategory, X = point.X, Y = point.Y, Id = point.TouchId });
            }
        }

        private static void ThreadTouchCallback()
        {
            int touchNumber;
            TouchEventCategory touchCategory;
            do
            {
                touchNumber = _touchController.GetNumberPoints();
                if (touchNumber == 1)
                {
                    var point = _touchController.GetPoint(true);
                    touchCategory = CheckIfInButtons(point.X, point.Y, TouchEventCategory.Unknown);
                    touchCategory  = point.Event == Event.Contact ? touchCategory | TouchEventCategory.Moving : touchCategory;
                    TouchEvent?.Invoke(_touchController, new TouchEventArgs() { TimeStamp = DateTime.UtcNow, EventCategory = EventCategory.Touch, TouchEventCategory = touchCategory, X = point.X, Y = point.Y, Id = point.TouchId });
                }
                else if (touchNumber == 2)
                {
                    var dp = _touchController.GetDoublePoints();
                    touchCategory = CheckIfInButtons(dp.Point1.X, dp.Point1.Y, TouchEventCategory.DoubleTouch);
                    touchCategory = dp.Point1.Event == Event.Contact ? touchCategory | TouchEventCategory.Moving : touchCategory;
                    TouchEvent?.Invoke(_touchController, new TouchEventArgs() { TimeStamp = DateTime.UtcNow, EventCategory = EventCategory.Touch, TouchEventCategory = touchCategory, X = dp.Point1.X, Y = dp.Point1.Y, Id = dp.Point1.TouchId });
                    touchCategory = CheckIfInButtons(dp.Point2.X, dp.Point2.Y, TouchEventCategory.DoubleTouch);
                    touchCategory = dp.Point2.Event == Event.Contact ? touchCategory | TouchEventCategory.Moving : touchCategory;
                    TouchEvent?.Invoke(_touchController, new TouchEventArgs() { TimeStamp = DateTime.UtcNow, EventCategory = EventCategory.Touch, TouchEventCategory = touchCategory, X = dp.Point2.X, Y = dp.Point2.Y, Id = dp.Point2.TouchId });
                }

                // This is necessary to give time to the touch sensor
                // In theory, the wait should be calculated with the period
                Thread.Sleep(10);
            } while (true);
        }

        private static TouchEventCategory CheckIfInButtons(int x, int y, TouchEventCategory touchCategory)
        {
            // Positions of the buttons on the X axis
            const int XLeft = 83;
            const int XMiddle = 182;
            const int XRight = 271;
            // On the Y one (same for all
            const int YButtons = 263;
            // The delta in pixel for the button size
            const int DeltaPixel = 24;
            // Check if we are in Y
            if ((y <= YButtons + DeltaPixel) && (y >= YButtons - DeltaPixel))
            {
                if ((x <= XLeft + DeltaPixel) && (x >= XLeft - DeltaPixel))
                {
                    touchCategory |= TouchEventCategory.LeftButton;
                }
                else if ((x <= XMiddle + DeltaPixel) && (x >= XMiddle - DeltaPixel))
                {
                    touchCategory |= TouchEventCategory.MiddleButton;
                }
                else if ((x <= XRight + DeltaPixel) && (x >= XRight - DeltaPixel))
                {
                    touchCategory |= TouchEventCategory.RightButton;
                }
            }

            return touchCategory;
        }

        static M5Core2()
        {
            // Setup first the I2C bus
            Configuration.SetPinFunction(22, DeviceFunction.I2C1_CLOCK);
            Configuration.SetPinFunction(21, DeviceFunction.I2C1_DATA);

            // Create the energy management device
            I2cDevice i2c = new(new I2cConnectionSettings(1, Axp192.I2cDefaultAddress));
            _power = new(i2c);

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
            _power.AdcPinEnabled = AdcPinEnabled.All;
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
                case 32:
                    Configuration.SetPinFunction(32, DeviceFunction.ADC1_CH4);
                    return _adc.OpenChannel(4);
                case 39:
                    Configuration.SetPinFunction(39, DeviceFunction.ADC1_CH3);
                    return _adc.OpenChannel(3);
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
