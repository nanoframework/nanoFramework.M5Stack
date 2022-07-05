// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Iot.Device.Axp192;
using nanoFramework.M5Stack;
using nanoFramework.UI;
using UnitsNet;

namespace nanoFramework.M5Stack
{
    /// <summary>
    /// M5 Stack screen class
    /// </summary>
    public class Screen : ScreenBase
    {
        /// <summary>
        /// Default memory allocation
        /// </summary>
        public const int DefaultMemoryAllocationBitmap = 1024;

        private const int ChipSelect = 5;
        private const int DataCommand = 23;
        private const int Reset = 18;
        private static Axp192 _power;
        private static int _lumi;
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

            MemoryAllocationBitmap = memoryBitMapAllocation;
            // Not used in Stick versions, AXP is doing this
            BackLightPin = -1;

#if M5STICKC
            _power = M5StickC.Power;
#else
            _power = M5StickCPlus.Power;
#endif

            var displaySpiConfig = new SpiConfiguration(
                1,
                ChipSelect,
                DataCommand,
                Reset,
                BackLightPin);

#if M5STICKC
            var screenConfig = new ScreenConfiguration(
                26,
                1,
                80,
                160);
#else
            var screenConfig = new ScreenConfiguration(
                52,
                40,
                135,
                240);
#endif

            _ = DisplayControl.Initialize(
                displaySpiConfig,
                screenConfig,
                (uint)MemoryAllocationBitmap);

            // Enable the screen
            Enabled = true;

            // For M5Stick, values from 2.6 to 3V are working fine
            LuminosityPercentage = 100;
            _power.EnableLDO3(true);
            _isInitialized = true;
        }

        /// <summary>
        /// Enables or disables the screen.
        /// </summary>
        public new static bool Enabled
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
        public new static int LuminosityPercentage
        {
            get => _lumi;

            set
            {
                // For M5Stick, values from 8 to 12 are working fine
                // 2.5 V = dark, 3.0 V full luminosity
                _lumi = value > 100 ? 100 : value;
                _power.LDO3OutputVoltage = ElectricPotential.FromVolts(_lumi * 0.5 / 100.0 + 2.5);
            }
        }
    }
}
