// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Hardware.Esp32;
using nanoFramework.UI;
using System.Threading;

namespace nanoFramework.M5Stack
{
    /// <summary>
    /// CoreS3 screen support based on the managed Ili9342 driver.
    /// </summary>
    public class Screen : ScreenBase
    {
        /// <summary>
        /// Default bitmap memory allocation in bytes for the screen buffer.
        /// </summary>
        public const int DefaultMemoryAllocationBitmap = ScreenWidth * ScreenHeight * 4;

        private const int ChipSelect = 3;
        private const int DataCommand = 35;
        private const int DummyMisoPin = 39;
        private const int Reset = -1;
        private const int SpiBus = 1;
        private const int ScreenWidth = 320;
        private const int ScreenHeight = 240;
        private const byte DefaultBrightness = 100;
        private static bool _isInitialized;
        private static bool _isEnabled;
        private static byte _brightness = DefaultBrightness;

        /// <summary>
        /// Initializes a new instance of the <see cref="Screen"/> class.
        /// </summary>
        /// <param name="memoryBitMapAllocation">The amount of memory to allocate for the screen buffer in bytes.</param>
        public Screen(int memoryBitMapAllocation = DefaultMemoryAllocationBitmap)
        {
            if (_isInitialized)
            {
                return;
            }

            // MISO must be assigned to a valid pin on ESP32 even for write-only displays.
            // Keep LCD D/C on GPIO35 and use a dummy, unconnected MISO pin instead.
            Configuration.SetPinFunction(DummyMisoPin, DeviceFunction.SPI1_MISO);
            Configuration.SetPinFunction(37, DeviceFunction.SPI1_MOSI);
            Configuration.SetPinFunction(36, DeviceFunction.SPI1_CLOCK);

            // CoreS3 drives LCD_RST from the AW9523 expander rather than a direct GPIO.
            M5CoreS3.SetLcdReset(false);
            Thread.Sleep(8);
            M5CoreS3.SetLcdReset(true);
            Thread.Sleep(64);

            uint actualBufferSize = DisplayControl.Initialize(
                new SpiConfiguration(SpiBus, ChipSelect, DataCommand, Reset, -1),
                new ScreenConfiguration(0, 0, ScreenWidth, ScreenHeight, CoreS3Ili9342.GraphicDriver),
                (uint)memoryBitMapAllocation);

            MemoryAllocationBitmap = (int)actualBufferSize;
            BackLightPin = -1;
            BrightnessPercentage = DefaultBrightness;
            Enabled = true;
            _isInitialized = true;
        }

        /// <summary>
        /// Turns the display backlight on or off.
        /// </summary>
        public static new bool Enabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                IsEnabled = value;
                M5CoreS3.SetScreenBacklight(_isEnabled ? _brightness : (byte)0);
            }
        }

        /// <summary>
        /// Gets or sets the backlight brightness percentage.
        /// </summary>
        public static byte BrightnessPercentage
        {
            get => _brightness;

            set
            {
                _brightness = value > 100 ? (byte)100 : value;
                if (_isEnabled)
                {
                    M5CoreS3.SetScreenBacklight(_brightness);
                }
            }
        }

    }
}