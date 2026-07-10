// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Hardware.Esp32;
using nanoFramework.UI;
using System.Device.Gpio;
using System.Threading;

namespace nanoFramework.M5Stack
{
    /// <summary>
    /// M5StickS3 screen support.
    /// </summary>
    public class Screen : ScreenBase
    {
        /// <summary>
        /// Default memory allocation for bitmap in bytes.
        /// </summary>
        public const int DefaultMemoryAllocationBitmap = 1024;

        private const int SpiBus = 1;
        private const int ChipSelect = 41;
        private const int DataCommand = 45;
        private const int Reset = 21;
        private const int NoResetInNativeDriver = -1;
        private const int DummyMisoPin = 42;
        private const int BackLight = 38;
        private const int Width = 135;
        private const int Height = 240;
        private const int OffsetX = 52;
        private const int OffsetY = 40;
        private const int ScreenMiso = 39;
        private const int ScreenClock = 40;

        private static bool _isInitialized;
        private static bool _isEnabled;

        /// <summary>
        /// Creates and initializes the screen.
        /// </summary>
        /// <param name="memoryBitMapAllocation">Bitmap allocation in bytes.</param>
        public Screen(int memoryBitMapAllocation = DefaultMemoryAllocationBitmap)
        {
            if (_isInitialized)
            {
                return;
            }

            Configuration.SetPinFunction(ScreenMiso, DeviceFunction.SPI1_MOSI);
            Configuration.SetPinFunction(ScreenClock, DeviceFunction.SPI1_CLOCK);
            Configuration.SetPinFunction(DummyMisoPin, DeviceFunction.SPI1_MISO);

            Controller = M5StickS3.GpioController;
            if (!Controller.IsPinOpen(Reset))
            {
                Controller.OpenPin(Reset, PinMode.Output);
            }

            if (!Controller.IsPinOpen(BackLight))
            {
                Controller.OpenPin(BackLight, PinMode.Output);
            }

            M5StickS3.SetLcdPower(true);
            Controller.Write(BackLight, PinValue.High);
            Thread.Sleep(100);

            // Pulse LCD reset explicitly to avoid native reset-path instability on some S3 firmwares.
            Controller.Write(Reset, PinValue.Low);
            Thread.Sleep(10);
            Controller.Write(Reset, PinValue.High);
            Thread.Sleep(100);

            uint actualBufferSize = DisplayControl.Initialize(
                new SpiConfiguration(SpiBus, ChipSelect, DataCommand, NoResetInNativeDriver, -1),
                new ScreenConfiguration(OffsetX, OffsetY, Width, Height, StickS3St7789.GraphicDriver),
                (uint)memoryBitMapAllocation);

            MemoryAllocationBitmap = (int)actualBufferSize;
            ScreenBase.BackLightPin = BackLight;
            Enabled = true;
            _isInitialized = true;
        }

        /// <summary>
        /// Enables or disables the screen.
        /// </summary>
        public new static bool Enabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                IsEnabled = value;

                if (value)
                {
                    M5StickS3.SetLcdPower(true);
                    Controller.Write(BackLight, PinValue.High);
                    Thread.Sleep(100);
                }
                else
                {
                    Thread.Sleep(2);
                    Controller.Write(BackLight, PinValue.Low);
                    M5StickS3.SetLcdPower(false);
                }
            }
        }

        /// <summary>
        /// Sets luminosity in percentage.
        /// </summary>
        public new static byte LuminosityPercentage
        {
            get => (byte)(_isEnabled ? 100 : 0);
            set => Enabled = value > 0;
        }
    }
}
