using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using System;
using System.Device.Gpio;

namespace nanoFramework.M5Stack
{
    /// <summary>
    /// M5 Stack screen class
    /// </summary>
    public class Screen
    {
        private const int BackLightPin = 32;
        private const int ChipSelect = 14;
        private const int DataCommand = 27;
        private const int Reset = 33;
        private const int MemoryAllocationBitmap = 1024;

        private static GpioController _gpio;
        private static bool _enabled;
        

        static Screen()
        {
            _gpio = new();
            _gpio.OpenPin(BackLightPin, PinMode.Output);
            Enabled = true;
            DisplayControl.Initialize(new SpiConfiguration(2, ChipSelect, DataCommand, Reset, BackLightPin), new ScreenConfiguration(0, 0, 320, 240), MemoryAllocationBitmap);
        }

        /// <summary>
        /// MAximum buffer size for a Bitmap on the native side.
        /// </summary>
        public static int MaxBitmapSize = (MemoryAllocationBitmap - 100) / 3;

        /// <summary>
        /// Enabled or disable the screen.
        /// </summary>
        public static bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                _gpio.Write(BackLightPin, _enabled);
            }
        }

        /// <summary>
        /// Clears the screen.
        /// </summary>
        public static void Clear() => DisplayControl.Clear();

        /// <summary>
        /// Write a text on the screen
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="width">The width of the area to display.</param>
        /// <param name="height">The height of the area to display</param>
        /// <param name="colors">A 16 bits color.</param>
        public static void Write(ushort x, ushort y, ushort width, ushort height, ushort[] colors)
            => DisplayControl.Write(x, y, width, height, colors);

        /// <summary>
        /// Writes on the screen some text.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="width">The width of the area to display.</param>
        /// <param name="height">The height of the area to display.</param>
        /// <param name="font">The font to use.</param>
        /// <param name="foreground">Foreground color.</param>
        /// <param name="background">Background color.</param>
        public static void Write(string text, ushort x, ushort y, ushort width, ushort height, Font font, Color foreground, Color background)
            => DisplayControl.Write(text, x, y, width, height, font, foreground, background);

        /// <summary>
        /// Write a point directly on the screen.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="color">The 16 bits color.</param>
        public static void WritePoint(ushort x, ushort y, ushort color) => DisplayControl.WritePoint(x, y, color);
    }
}
