// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using System.Device.Gpio;

namespace nanoFramework.M5Stack
{
    /// <summary>
    /// M5 Stack screen class
    /// </summary>
    public abstract class ScreenBase
    {
        internal static int MemoryAllocationBitmap;

        internal static int BackLightPin;
        internal static GpioController Controller;
        internal static bool IsEnabled;

        /// <summary>
        /// Maximum buffer size for a Bitmap on the native side.
        /// </summary>
        // A bitmap stores colors on 3 bytes and there is an initial structure taking maximum 100 bytes
        public static int MaxBitmapSize { get => (MemoryAllocationBitmap - 100) / 3; }

        /// <summary>
        /// Enabled or disable the screen.
        /// </summary>
        public static bool Enabled
        {
            get => IsEnabled;
            set
            {
                IsEnabled = value;
                Controller.Write(BackLightPin, IsEnabled);
            }
        }

        /// <summary>
        /// Sets the screen intensity in percentage from 0 to 100.
        /// </summary>
        /// <remarks>If this is not natively supported, the threshold of 50 will make the screen either on or off.</remarks>
        public static byte LuminosityPercentage
        {
            get
            {
                return (byte)(IsEnabled ? 100 : 0);
            }

            set
            {
                Enabled = value > 50;
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
