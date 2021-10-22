// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using System;

namespace nanoFramework
{
    /// <summary>
    /// Console class to display text on screens
    /// </summary>
    public static class Console
    {
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
        public static int CursorLeft { get; set; } = 0;

        /// <summary>
        /// Gets or sets the row position.
        /// </summary>
        public static int CursorTop { get; set; } = 0;

        /// <summary>
        /// Clears the screen.
        /// </summary>
        public static void Clear()
        {
            DisplayControl.Clear();
            CursorLeft = 0;
            CursorTop = 0;
        }

        /// <summary>
        /// Resets the colors to their default, White for the foreground and Black for the background.
        /// </summary>
        public static void ResetColor()
        {
            ForegroundColor = Color.White;
            BackgroundColor = Color.Black;
        }

        /// <summary>
        /// Writes a text on the screen at the cursor position.
        /// Cursor position will automatically increase.
        /// </summary>
        /// <remarks>No new line character will be recognized, use WriteLine instead.</remarks>
        /// <param name="text">The text to display.</param>
        public static void Write(string text)
        {
            // NOTE: Some work needs to be adjusted on the native side to properly handle the character display. Once done, adjustments will be needed.
            // Make the math for the lines, it's based out the Max width of the character and won't be perfect
            // Console is recommended with fixed size fonts
            ushort width = (ushort)(DisplayControl.ScreenWidth - CursorLeft * Font.MaxWidth);
            if (text.Length < width / Font.MaxWidth)
            {
                DisplayControl.Write(text, (ushort)(CursorLeft * Font.MaxWidth), (ushort)(CursorTop * Font.Height), (ushort)(DisplayControl.ScreenWidth - 1), (ushort)(DisplayControl.ScreenHeight - 1), Font, ForegroundColor, BackgroundColor);
                CursorLeft += text.Length;
            }
            else
            {
                DisplayControl.Write(text.Substring(0, width / Font.MaxWidth), (ushort)(CursorLeft * Font.MaxWidth), (ushort)(CursorTop * Font.Height), (ushort)(DisplayControl.ScreenWidth - 1), (ushort)(DisplayControl.ScreenHeight - 1), Font, ForegroundColor, BackgroundColor);
                CursorTop++;
                string newTxt = text.Substring(width / Font.MaxWidth);
                DisplayControl.Write(newTxt, 0, (ushort)(CursorTop * Font.Height), (ushort)(DisplayControl.ScreenWidth - Font.MaxWidth - 1), (ushort)(DisplayControl.ScreenHeight - 1), Font, ForegroundColor, BackgroundColor);
                CursorLeft = newTxt.Length % WindowWidth;
                CursorTop += newTxt.Length / WindowWidth;
                CursorTop = CursorTop > WindowHeight ? WindowHeight : CursorTop;
            }
        }

        /// <summary>
        /// Writes a text on the screen at the cursor position and goes to the next line.
        /// </summary>
        /// <param name="text">The text to display.</param>
        public static void WriteLine(string text)
        {
            Write(text);
            CursorLeft = 0;
            CursorTop++;
            CursorTop = CursorTop > WindowHeight ? WindowHeight : CursorTop;
        }
    }
}
