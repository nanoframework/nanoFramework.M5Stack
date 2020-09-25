//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace nanoFramework.M5Stack
{
    /// <summary>
    /// Represents the M5Stack LCD. The class provides methods and properties that an app can use to configure and write to the LCD.
    /// </summary>
    /// <remarks>
    /// The screen pixel is 320x240, with the top left corner of the screen being the origin (0,0).
    /// </remarks>
    public class Lcd
    {
        /// <summary>
        /// Constructor
        /// </summary>
        static Lcd()
        {
            // TODO inits and configs
            begin();
        }

        /// <summary>
        /// Switch the display to energy saving mode.
        /// </summary>
        /// <remarks>
        /// Call <see cref="Wakeup"/> to wake up the display.
        /// </remarks>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void Sleep();

        /// <summary>
        /// Restore the display from energy saving mode.
        /// </summary>
        /// <remarks>
        /// Since the LCD backlight of M5Stack is controlled separately, please adjust it with SetBrightness() if necessary.
        /// </remarks>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void Wakeup();

        /// <summary>
        /// Adjust the display backlight.
        /// </summary>
        /// <param name="brightness">0: Off - 255:Full bringness</param>
        /// <remarks>s
        /// 1) The backlight is controlled by PWM (44.1 KHz).
        /// 2) Many backlights have a direct effect on battery consumption.
        /// </remarks>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void SetBrightness(uint brightness);

        /// <summary>
        /// Display a bar that shows the progress.
        /// </summary>
        /// <remarks>
        /// Please erase the background beforehand to draw only the additional amount.
        /// </remarks>
        /// <param name="x">Coordinate X(left corner)</param>
        /// <param name="y">Coordinate Y(left corner)</param>
        /// <param name="w">width (px)</param>
        /// <param name="h">height(px)</param>
        /// <param name="progress">progress(0-100%)</param>
        /// <param name="color">Color of the bar</param>
        /// <param name="backgroundColor">Color of the Background</param>        
        public static void ProgressBar(uint x, uint y, uint w, uint h, uint progress, Color color = Color.White, Color backgroundColor = Color.Black)
        {
            fillRect(x, y, w, h, backgroundColor);
            progressBar(x, y, w, h, progress, color);
        }

        /// <summary>
        /// Generate a QR code.
        /// </summary>
        /// <remarks>
        /// Please indicate the appropriate QR code version according to the number of characters.
        /// </remarks>
        /// <param name="text">String to embed in QR</param>
        /// <param name="x">Coordinate X(left corner)</param>
        /// <param name="y">Coordinate Y(left corner)</param>
        /// <param name="width">width (px)</param>
        /// <param name="version">QR code version</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void QRCode(string text, uint x, uint y, uint width, int version);

        /// <summary>
        /// Draw a bitmap
        /// </summary>
        /// <remarks>
        /// The color code is expressed by a total of 16 bits: red 5 bits, green 6 bits and blue 5 bits from the top.
        /// </remarks>
        /// <param name="x">Coordinate X(left corner)</param>
        /// <param name="y">Coordinate Y(left corner)</param>
        /// <param name="width">width (px)</param>
        /// <param name="height">height(px</param>
        /// <param name="data">image data</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void DrawBitmap(uint x, uint y, uint width, uint height, uint[] data);

        /// <summary>
        /// Draw a bitmap
        /// </summary>
        /// <remarks>
        /// The color code is expressed by a total of 16 bits: red 5 bits, green 6 bits and blue 5 bits from the top.
        /// </remarks>
        /// <param name="x">Coordinate X(left corner)</param>
        /// <param name="y">Coordinate Y(left corner)</param>
        /// <param name="width">width (px)</param>
        /// <param name="height">height(px</param>
        /// <param name="data">image data</param>
        /// <param name="transparent">Transparent color code</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void DrawBitmap(uint x, uint y, uint width, uint height, uint[] data, Color transparent);
      
        /// <summary>
        /// Defines the scale of a JPEG
        /// </summary>
        public enum JpegDivider
        {
            /// <summary>
            /// No care
            /// </summary>
            JPEG_DIV_NONE,
            /// <summary>
            /// 1/2
            /// </summary>
            JPEG_DIV_2,
            /// <summary>
            /// 1/4
            /// </summary>
            JPEG_DIV_4,
            /// <summary>
            /// 1/8
            /// </summary>
            JPEG_DIV_8,
            /// <summary>
            /// MAX
            /// </summary>
            JPEG_DIV_MAX
        }

        /// <summary>
        /// Read JPEG data from memory and draw it.
        /// </summary>
        /// <remarks>
        /// Depending on the size, the number of bits and the format (progressive etc.), it may not be possible to expand.
        /// </remarks>
        /// <param name="jpg_data">top of data</param>
        /// <param name="jpg_len">data length</param>
        /// <param name="x">Coordinate X (left corner)</param>
        /// <param name="y">Coordinate Y (left corner)</param>
        /// <param name="maxWidth">Maximum width (px)</param>
        /// <param name="maxHeight">Maximum height (px)</param>
        /// <param name="offX">offset X (px)</param>
        /// <param name="offY">offset Y (px)</param>
        /// <param name="scale">scale</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void DrawJpg(byte[] jpg_data, uint jpg_len, uint x = 0, uint y = 0, uint maxWidth = 0, uint maxHeight = 0, uint offX = 0, uint offY = 0, JpegDivider scale = JpegDivider.JPEG_DIV_NONE);

        /// <summary>
        /// Fill the entire screen with the specified color.
        /// </summary>
        /// <param name="color">the color to be filled</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void FillScreen(Color color);

        /// <summary>
        /// Set the foreground color and background color of the displayed text.
        /// </summary>
        /// <param name="color">the color of text</param>
        /// <param name="backgroundColor">the background color of text</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void SetTextColor(Color color, Color backgroundColor);

        /// <summary>
        /// Move the cursor to (x0, y0).
        /// </summary>
        /// <param name="x">Coordinate X</param>
        /// <param name="y">Coordinate Y</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void SetCursor(uint x, uint y);

        /// <summary>
        /// Get the cursor of x
        /// </summary>
        /// <returns>Coordinate X</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static uint GetCursorX();

        /// <summary>
        /// Get the cursor of y
        /// </summary>
        /// <returns>Coordinate Y</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static uint GetCursorY();

        /// <summary>
        /// Set the Size of Text.
        /// </summary>
        /// <param name="size">Size of the text</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void SetTextSize(uint size);

        /// <summary>
        /// Fill color use of clear screen.
        /// </summary>
        /// <param name="color">Color</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void Clear(Color color);

        /// <summary>
        /// Draw a point at position (x, y).
        /// </summary>
        /// <param name="x">Coordinate X</param>
        /// <param name="y">Coordinate Y</param>
        /// <param name="color">Color of the pixel</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void DrawPixel(uint x, uint y, Color color);

        /// <summary>
        /// Blend foreground and background and return new colour.
        /// </summary>
        /// <param name="alpha">transparency</param>
        /// <param name="foregound">Foreground color</param>
        /// <param name="background">Background color</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Color AlphaBlend(uint alpha, Color foregound, Color background);

        /// <summary>
        /// Draws a character of the specified color from the specified start point and size
        /// </summary>
        /// <param name="x">Coordinate X (upper left)</param>
        /// <param name="y">Coordinate Y (upper left)</param>
        /// <param name="c">Character code</param>
        /// <param name="color">Drawing color</param>
        /// <param name="background">Background color</param>
        /// <param name="size">Character size</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void DrawChar(uint x, uint y, char c, Color color, Color background, uint size);

        /// <summary>
        /// Draws a character of the specified color from the specified start point
        /// </summary>
        /// <param name="c">Charagter code</param>
        /// <param name="x">Coordinate X (upper left)</param>
        /// <param name="y">Coordinate Y (upper left)</param>
        /// <param name="font">** tbc Character size or referenced font ?**</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void DrawChar(char c, uint x, uint y, uint font);

        /// <summary>
        /// Draw a long integer.
        /// </summary>
        /// <param name="number">Number</param>
        /// <param name="x">Coordinate of X</param>
        /// <param name="y">Coordinate of Y</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void DrawNumber(long number, uint x, uint y);

        /// <summary>
        /// DrawFloat, prints 7 non zero digits maximum
        /// </summary>
        /// <param name="number">Number</param>
        /// <param name="dp">Within seven decimal places</param>
        /// <param name="x">Coordinate of X</param>
        /// <param name="y">Coordinate of Y</param>
        /// <returns>tbd</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static uint DrawFloat(float number, uint dp, uint x, uint y);

        /// <summary>
        /// Draw a vertical line from X to Y.
        /// </summary>
        /// <param name="x">X position of start point</param>
        /// <param name="y">Y position of start point</param>
        /// <param name="length">line length</param>
        /// <param name="color">Line color</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void DrawFastVLine(uint x, uint y, uint length, Color color);

        /// <summary>
        /// Draw a horizontal line from X to Y.
        /// </summary>
        /// <param name="x">X position of start point</param>
        /// <param name="y">Y position of start point</param>
        /// <param name="length">line length</param>
        /// <param name="color">Line color</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void DrawFastHLine(uint x, uint y, uint length, Color color);

        /// <summary>
        /// Draw the line from point (x,y) to point (x1,y1).
        /// </summary>
        /// <param name="x0">X position of start point</param>
        /// <param name="y0">Y position of start point</param>
        /// <param name="x1">X position of end point</param>
        /// <param name="y1">Y position of end point</param>
        /// <param name="color">Line color</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void DrawLine(uint x0, uint y0, uint x1, uint y1, Color color);

        /// <summary>
        /// Number of the quater
        /// </summary>
        public enum QuaterCorn
        {
            /// <summary>
            /// First quater
            /// </summary>
            One = 1,
            /// <summary>
            /// Second quater
            /// </summary>
            Two = 2,
            /// <summary>
            /// Third quater
            /// </summary>
            Three = 3,
            /// <summary>
            /// fourth quater
            /// </summary>
            Four = 4
        }

        /// <summary>
        /// Draw a quarter circle with the center at the point x0 and y0, with radius r, and a quarter C
        /// </summary>
        /// <param name="x0">Centerpoint X</param>
        /// <param name="y0">Centerpoint Y</param>
        /// <param name="r">Radius</param>
        /// <param name="cornername">QuaterCorn</param>
        /// <param name="color">Quater color</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void DrawCircleHelper(uint x0, uint y0, uint r, QuaterCorn cornername, Color color);

        /// <summary>
        ///  Draw a circle with the center at the point x0 and y0
        /// </summary>
        /// <param name="x">Centerpoint X</param>
        /// <param name="y">Centerpoint Y</param>
        /// <param name="r">Raduis</param>
        /// <param name="color">Circle color</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void DrawCircle(uint x, uint y, uint r, Color color);

        /// <summary>
        /// Draw a filled circle on point(x0, y0)
        /// </summary>
        /// <param name="x">Centerpoint X</param>
        /// <param name="y">Centerpoint Y</param>
        /// <param name="r">Radius</param>
        /// <param name="color">Circle color</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void FillCircle(uint x, uint y, uint r, Color color);

        /// <summary>
        /// Draw a triangel between points (x,y), (x1,y1) and (x2,y2).
        /// </summary>
        /// <param name="x0">X position of start point</param>
        /// <param name="y0">Y position of start point</param>
        /// <param name="x1">X position of second point</param>
        /// <param name="y1">Y position of second point</param>
        /// <param name="x2">X position of third point</param>
        /// <param name="y2">Y position of third point</param>
        /// <param name="color">Color</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void DrawTriangle(uint x0, uint y0, uint x1, uint y1, uint x2, uint y2, Color color);

        /// <summary>
        /// Fill the triangel between points (x,y), (x1,y1) and (x2,y2).
        /// </summary>
        /// <param name="x0">X position of start point</param>
        /// <param name="y0">Y position of start point</param>
        /// <param name="x1">X position of second point</param>
        /// <param name="y1">Y position of second point</param>
        /// <param name="x2">X position of third point</param>
        /// <param name="y2">Y position of third point</param>
        /// <param name="color">Color</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void FillTriangle(uint x0, uint y0, uint x1, uint y1, uint x2, uint y2, Color color);

        /// <summary>
        /// Draw the rectangle from the upper left point at (x,y) and width and height.
        /// </summary>
        /// <param name="x">X position of the rectangle</param>
        /// <param name="y">Y position of the rectangle</param>
        /// <param name="width">Width of rectangle</param>
        /// <param name="height">Height of rectangle</param>
        /// <param name="color">Color</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void DrawRect(uint x, uint y, uint width, uint height, Color color);

        /// <summary>
        /// Draw a filled rectangle from the upper left point at (x,y) and width and height.
        /// </summary>
        /// <param name="x">X position of the rectangle</param>
        /// <param name="y">Y position of the rectangle</param>
        /// <param name="width">Width of rectangle</param>
        /// <param name="height">Height of rectangle</param>
        /// <param name="color">Color</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void FillRect(uint x, uint y, uint width, uint height, Color color);

        /// <summary>
        /// Draw the rectangle with rounded corners from the upper left point at (x,y) and width and height. Corner radius is given by radius argument.
        /// </summary>
        /// <param name="x">X position of the rectangle</param>
        /// <param name="y">Y position of the rectangle</param>
        /// <param name="width">Width of rectangle</param>
        /// <param name="height">Height of rectangle</param>
        /// <param name="radius">Radius of the corner circle</param>
        /// <param name="color">Color</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void DrawRoundRect(uint x, uint y, uint width, uint height, uint radius, Color color);

        /// <summary>
        /// Draw the filled rectangle with rounded corners from the upper left point at (x,y) and width and height. Corner radius is given by radius argument.
        /// </summary>
        /// <param name="x">X position of the rectangle</param>
        /// <param name="y">Y position of the rectangle</param>
        /// <param name="width">Width of rectangle</param>
        /// <param name="height">Height of rectangle</param>
        /// <param name="radius">Radius of the corner circle</param>
        /// <param name="color">Color</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void FillRoundRect(uint x, uint y, uint width, uint height, uint radius, Color color);

        /// <summary>
        /// Draw an ellipse with the top left point (x, y) and the width and height.
        /// </summary>
        /// <param name="x">Center X coordinate of the ellipse</param>
        /// <param name="y">Center Y coordinate of the ellipse</param>
        /// <param name="rx">Width of circle</param>
        /// <param name="ry">Height of circle</param>
        /// <param name="color">Circle color</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void DrawEllipse(uint x, uint y, uint rx, uint ry, Color color);

        /// <summary>
        /// Draw a filled ellipse with the top left point (x, y) and the width and height.
        /// </summary>
        /// <param name="x">Center X coordinate of the ellipse</param>
        /// <param name="y">Center Y coordinate of the ellipse</param>
        /// <param name="rx">Width of circle</param>
        /// <param name="ry">Height of circle</param>
        /// <param name="color">Circle color</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void FillEllipse(uint x, uint y, uint rx, uint ry, Color color);

        /// <summary>
        /// Rotate Definition
        /// </summary>
        public enum Rotate
        {
            Rotate0 = 0,
            Rotate90 = 1,
            Rotate180 = 2,
            Rotate270 = 3,
            ReverseRotate0 = 4,
            ReverseRotate90 = 5,
            ReverseRoate180 = 6,
            ReverseRotate270 = 7
        }

        /// <summary>
        /// Rotate the screen.
        /// </summary>
        /// <param name="rotate">Rotatation enum</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void SetRotation(Rotate rotate);

        /// <summary>
        /// Reverse the screen color in negative / positive.
        /// </summary>
        /// <param name="invert">Inverted</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void InvertDisplay(bool invert);

        /// <summary>
        /// Load a font
        /// </summary>
        /// <param name="fontFileName"></param>
        /// <param name="stream"></param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void LoadFont(string fontFileName, Stream stream);

        /// <summary>
        /// Whether to automatically wrap the display
        /// </summary>
        /// <param name="wrapX">X direction</param>
        /// <param name="wrapY">Y direction</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void SetTextWrap(bool wrapX, bool wrapY);

        /// <summary>
        /// 
        /// </summary>
        public enum TextPosition : uint
        {
            /// <summary>
            /// // Top left (default)
            /// </summary>
            TL_DATUM = 0,
            /// <summary>
            /// // Top centre
            /// </summary>
            TC_DATUM = 1,
            /// <summary>
            /// // Top right
            /// </summary>
            TR_DATUM = 2,
            /// <summary>
            /// // Middle left
            /// </summary>
            ML_DATUM = 3,
            /// <summary>
            /// // Centre left, same as above
            /// </summary>
            CL_DATUM = 3,
            /// <summary>
            /// // Middle centre
            /// </summary>
            MC_DATUM = 4,
            /// <summary>
            /// // Centre centre, same as above
            /// </summary>
            CC_DATUM = 4,
            /// <summary>
            /// // Middle right
            /// </summary>
            MR_DATUM = 5,
            /// <summary>
            /// // Centre right, same as above
            /// </summary>
            CR_DATUM = 5,
            /// <summary>
            /// // Bottom left
            /// </summary>
            BL_DATUM = 6,
            /// <summary>
            /// // Bottom centre
            /// </summary>
            BC_DATUM = 7,
            /// <summary>
            /// // Bottom right
            /// </summary>
            BR_DATUM = 8,
            /// <summary>
            /// // Left character baseline (Line the 'A' character would sit on)
            /// </summary>
            L_BASELINE = 9,
            /// <summary>
            /// // Centre character baseline
            /// </summary>
            C_BASELINE = 10,
            /// <summary>
            /// // Right character baseline
            /// </summary>
            R_BASELINE = 11 
        }

        /// <summary>
        /// Set the text position reference datum
        /// </summary>
        /// <param name="textPosition">Text plotting alignment </param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void SetTextDatum(TextPosition textPosition);

        /// <summary>
        /// Text background padding some pixel to over-write the old text
        /// </summary>
        /// <param name="x_width">Blanked area will be width of pixels</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void SetTextPadding(uint x_width);

        /// <summary>
        /// Return the rotation value (as used by setRotation())
        /// </summary>
        /// <returns>Rotation</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static uint GetRotation();

        /// <summary>
        /// Return the pixel width of display (per current rotation)
        /// </summary>
        /// <returns>Width</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static uint GetWidth();

        /// <summary>
        /// Return the pixel height of display (per current rotation)
        /// </summary>
        /// <returns>Height</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static uint GetHeight();

        #region native handlers

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void begin(); 

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern static void progressBar(uint x, uint y, uint w, uint h, uint progress, Color color);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern static void fillRect(uint x, uint y, uint width, uint height, Color color);
       
        #endregion
    }
}
