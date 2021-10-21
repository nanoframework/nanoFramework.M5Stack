﻿using nanoFramework.Presentation.Media;
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
        private static GpioController _gpio;

        /// <summary>
        /// The font to use in the screen
        /// </summary>
        public static Font Font { get; set; }

        /// <summary>
        /// The foreground color. Default is White.
        /// </summary>
        public static Color ForegroundColor { get; set; } = Color.White;

        /// <summary>
        /// The background color. Default is Black.
        /// </summary>
        public static Color BackgroundColor { get; set; } = Color.Black;

        /// <summary>
        /// Gets the height of the screen in rows
        /// </summary>
        public static int WindowHeight { get => DisplayControl.ScreenHeight / Font.Height; }

        /// <summary>
        /// Gets the width of the screen in columns. Note this is based on the max character size.
        /// If you are using a non fix font, you will most likely be able to writ"e more characters.
        /// </summary>
        public static int WindowWidth { get => DisplayControl.ScreenWidth / Font.MaxWidth; }

        /// <summary>
        /// Gets or sets the column position.
        /// </summary>
        public static int CursorLeft { get; set; }

        /// <summary>
        /// Gets or sets the row position.
        /// </summary>
        public static int CursorTop { get; set; }

        static Screen()
        {
            int backLightPin = 32;
            int chipSelect = 14;
            int dataCommand = 27;
            int reset = 33;
            _gpio = new();
            _gpio.OpenPin(backLightPin, PinMode.Output);
            _gpio.Write(backLightPin, PinValue.High);
            DisplayControl.Initialize(new SpiConfiguration(2, chipSelect, dataCommand, reset, backLightPin), new ScreenConfiguration(0, 0, 320, 240), 1024);
        }

        /// <summary>
        /// Clears the screen.
        /// </summary>
        public void Clear() => DisplayControl.Clear();

        /// <summary>
        /// Write a text on the screen
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="width">The width of the area to display.</param>
        /// <param name="height">The height of the area to display</param>
        /// <param name="colors">A 16 bits color.</param>
        public void Write(ushort x, ushort y, ushort width, ushort height, ushort[] colors)
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
        public void Write(string text, ushort x, ushort y, ushort width, ushort height)
            => DisplayControl.Write(text, x, y, width, height, Font, ForegroundColor, BackgroundColor);

        /// <summary>
        /// Write a point directly on the screen.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="color">The 16 bits color.</param>
        public void WritePoint(ushort x, ushort y, ushort color) => DisplayControl.WritePoint(x, y, color);
    }
}
