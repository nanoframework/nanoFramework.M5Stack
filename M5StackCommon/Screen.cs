// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if M5CORE2 || TOUGH
using Iot.Device.Axp192;
using nanoFramework.M5Stack;
using nanoFramework.UI;
using System.Device.Gpio;
using System.Threading;
using UnitsNet;

namespace nanoFramework.M5Stack
{
    /// <summary>
#if M5CORE2
    /// M5Core2 screen class.
#elif TOUGH
    /// M5Tough screen class.
#endif
    /// </summary>
    public class Screen : ScreenBase
    {
        /// <summary>
        /// Default memory allocation
        /// </summary>
        public const int DefaultMemoryAllocationBitmap = 320 * 240 * 4;

        private const byte DefaultScreenBrightness = 75;
        private const int ChipSelect = 5;
        private const int DataCommand = 15;
        private const int Reset = -1;
        private static Axp192 _power;
        private static byte _brightness;
        private static bool _isInitialized = false;

        /// <summary>
        /// Initializes the screen
        /// </summary>
        /// <param name="memoryBitMapAllocation">The memory allocation.</param>
        public Screen(int memoryBitMapAllocation = DefaultMemoryAllocationBitmap)
        {
            if (_isInitialized)
            {
                return;
            }

            // We're allocating enough memory for the full screen because these targets have PSRAM
            MemoryAllocationBitmap = memoryBitMapAllocation;
            // backligth is not controlled by the screen driver
            BackLightPin = -1;

#if M5CORE2
            _power = M5Stack.M5Core2.Power;
#elif TOUGH
            _power = M5Stack.Tough.Power;
#endif

            // Reset screen
            _power.Gpio4Value = PinValue.Low;
            Thread.Sleep(100);
            _power.Gpio4Value = PinValue.High;
            Thread.Sleep(100);

#if TOUGH
            // Reset touch controller
            _power.Gpio1Value = PinValue.Low;
            Thread.Sleep(100);
            _power.Gpio1Value = PinValue.High;
            Thread.Sleep(100);
#endif

            // Create the screen
            DisplayControl.Initialize(new SpiConfiguration(2, ChipSelect, DataCommand, Reset, BackLightPin), new ScreenConfiguration(0, 0, 320, 240), (uint)MemoryAllocationBitmap);

            // set initial value for brightness
            BrightnessPercentage = DefaultScreenBrightness;

            // enable back-light
            Enabled = true;

            _isInitialized = true;
        }

        /// <summary>
        /// Enables or disables the screen.
        /// </summary>
        /// <value><see langword="true"/> to enable the screen, <see langword="false"/> to disable it.</value>
        public static new bool Enabled
        {
            get => IsEnabled;

            set
            {
                IsEnabled = value;

#if M5CORE2
                _power.EnableDCDC3(IsEnabled);
#elif TOUGH
            _power.EnableLDO3(IsEnabled);
#endif
            }
        }

        /// <summary>
        /// Gets or sets the screen brightness.
        /// </summary>
        /// <value>Brightness as percentage.</value>
        public static new byte BrightnessPercentage
        {
            get => _brightness;

            set
            {
                // For M5Core2 and M5Tough, values from 2.5 to 3V are working fine
                // 2.5 V = dark, 3.0 V full luminosity
                _brightness = (byte)(value > 100 ? 100 : value);
                var backLightVoltage = ElectricPotential.FromVolts(_brightness * 0.5 / 100.0 + 2.5);
#if M5CORE2
                _power.LDO3OutputVoltage = backLightVoltage;
#elif TOUGH
                _power.LDO3OutputVoltage = backLightVoltage;
#endif
            }
        }
    }
}

#endif
