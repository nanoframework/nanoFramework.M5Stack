// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Iot.Device.Axp192;
using nanoFramework.M5Stack;
using nanoFramework.UI;
using System;
using System.Device.Gpio;

namespace nanoFramework.M5Stick
{
    /// <summary>
    /// M5 Stack screen class
    /// </summary>
    public class Screen : ScreenBase
    {
        private const int ChipSelect = 5;
        private const int DataCommand = 23;
        private const int Reset = 18;
        private static Axp192 _power;
        private static byte _lumi;

        static Screen()
        {
            // Not used in Stick versions, AXP is doing this
            BackLightPin = -1;            
#if M5STICKC
            _power = M5StickC.Power;
            DisplayControl.Initialize(new SpiConfiguration(2, ChipSelect, DataCommand, Reset, BackLightPin), new ScreenConfiguration(26, 1, 80, 160), (uint)MemoryAllocationBitmap);
#else
            _power = M5StickCPlus.Power;
            DisplayControl.Initialize(new SpiConfiguration(2, ChipSelect, DataCommand, Reset, BackLightPin), new ScreenConfiguration(52, 40, 135, 240), (uint)MemoryAllocationBitmap);
#endif
            // Enable the screen
            Enabled = true;
            // For M5Stick, values from 8 to 12 are working fine
            LuminosityPercentage = 40;
            // 3.0V for the screen
            _power.SetLDO3Output(0xC);
            _power.EnableLDO3(true);
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
        /// <remarks> On M5Stick, anything less than 20% will be fully black</remarks>
        public static new byte LuminosityPercentage
        {
            get => _lumi;

            set
            {
                // For M5Stick, values from 8 to 12 are working fine
                // 7 = dark, 12 full luminosity
                _lumi = (byte)(value > 100 ? 100 : _lumi);
                _power.SetLDO3Output((byte)(_lumi * 5 / 100 + 7));
            }
        }
    }
}
