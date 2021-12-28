// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Iot.Device.Axp192;
using nanoFramework.M5Stack;
using nanoFramework.UI;
using System.Device.Gpio;
using System.Threading;
using UnitsNet;

namespace nanoFramework.M5Stack
{
    /// <summary>
    /// M5Core2 screen class
    /// </summary>
    public class Screen : ScreenBase
    {
        private const int ChipSelect = 5;
        private const int DataCommand = 15;
        private const int Reset = -1;
        private static Axp192 _power;
        private static byte _lumi;
        private static bool _isInitialized = false;

        /// <summary>
        /// Initializes the screen
        /// </summary>
        public Screen()
        {
            if(_isInitialized)
            {
                return;
            }

            // We're allocating anough memory for the full screen as this is a SPRAM board
            MemoryAllocationBitmap = 320 * 240 * 4;
            BackLightPin = -1;
            _power = M5Stack.M5Core2.Power;
            // Enable the screen
            Enabled = true;
            // Reset
            _power.Gpio4Value = PinValue.Low;
            Thread.Sleep(100);
            _power.Gpio4Value = PinValue.High;
            Thread.Sleep(100);
            // Create the screen
            DisplayControl.Initialize(new SpiConfiguration(2, ChipSelect, DataCommand, Reset, BackLightPin), new ScreenConfiguration(0, 0, 320, 240), (uint)MemoryAllocationBitmap);

            // For M5Core2, values from 2.5 to 3V are working fine
            // 3.0V for the screen            
            LuminosityPercentage = 100;
            _power.EnableDCDC3(true);
            _isInitialized = true;
        }

        /// <summary>
        /// Enables or disables the screen.
        /// </summary>
        public static new bool Enabled
        {
            get => IsEnabled;

            set
            {
                IsEnabled = value;
                _power.EnableLDO2(IsEnabled);
            }
        }

        /// <summary>
        /// Sets or gets the screen luminosity.
        /// </summary>
        /// <remarks> On M5Core2, anything less than 20% will be fully black</remarks>
        public static new byte LuminosityPercentage
        {
            get => _lumi;

            set
            {
                // For M5Core2, values from 2.5 to 3V are working fine
                // 2.5 V = dark, 3.0 V full luminosity
                _lumi = (byte)(value > 100 ? 100 : _lumi);
                _power.LDO3OutputVoltage = ElectricPotential.FromVolts((byte)(_lumi * 0.5 / 100.0 + 2.5));
            }
        }
    }
}
