// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.UI;
using System.Device.Gpio;

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
        public const int DefaultMemoryAllocationBitmap = 320 * 240 * 4;

        private const int ChipSelect = 14;
        private const int DataCommand = 27;
        private const int Reset = 33;
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
            BackLightPin = 32;
            Controller = new();
            Controller.OpenPin(BackLightPin, PinMode.Output);
            Enabled = true;
            DisplayControl.Initialize(new SpiConfiguration(2, ChipSelect, DataCommand, Reset, BackLightPin), new ScreenConfiguration(0, 0, 320, 240), (uint)MemoryAllocationBitmap);
            _isInitialized = true;
        }
    }
}
