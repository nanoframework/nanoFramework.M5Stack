//
// Copyright (c) 2019 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace nanoFramework.M5Stack
{
    /// <summary>
    /// Converts RGB 24 bit color schema to RGB 18 bit color schema
    /// </summary>
    public class ColorConverter
    {
        /// <summary>
        /// Converts RGB 8 bit color bytes to a 18 bit Color code
        /// </summary>
        /// <param name="r">Red spectrum of the color</param>
        /// <param name="g">Green spectrum of the color</param>
        /// <param name="b">Blue spectrum of the color</param>
        /// <returns>The converted Color as enum</returns>
        public static Color ToRgb666(byte r, byte g, byte b)
        {
            var col = (Color)(((r >> 2) << 12) | ((g >> 2) >> 6) | (b >> 2));
            return col;
        }

        /// <summary>
        /// Converts RBG 8 bit color bytes to a RGB 18 bit color code array
        /// </summary>
        /// <param name="r">Red spectrum of the color</param>
        /// <param name="g">Green spectrum of the color</param>
        /// <param name="b">Blue spectrum of the color</param>
        /// <returns>Array of 3 bytes with a RGB 18 bit schema</returns>
        public static byte[] ToRgbBytes(byte r, byte g, byte b)
        {
            var col = new byte[] { (byte)(r >> 2), (byte)(g >> 2), (byte)(b >> 2) };
            return col;
        }
        /// <summary>
        /// Converts a 24 bit RGB int based color code into a 18 bit color code
        /// </summary>
        /// <param name="colorCode"></param>
        /// <returns>Array of 3 bytes with a RGB 18 bit schema</returns>
        public static byte[] ToRgbBytes(int colorCode)
        {
            var b = (byte)((UInt32)colorCode & 0xff);
            var g = (byte)(((UInt32)colorCode >> 8) & 0xff);
            var r = (byte)(((UInt32)colorCode >> 16) & 0xff);

            var col = new byte[] { (byte)(r >> 2), (byte)(g >> 2), (byte)(b >> 2) };
            return col;
        }
    }
    /// <summary>
    /// Represents a 18bit color schema
    /// </summary>
    public enum Color : UInt32
    {
        LightSalmon = 0xFCA078,
        Salmon = 0xF88070,
        DarkSalmon = 0xE89478,
        LightCoral = 0xF08080,
        IndianRed = 0xCC5C5C,
        Crimson = 0xDC143C,
        FireBrick = 0xB02020,
        Red = 0xFC0000,
        DarkRed = 0x880000,
        Coral = 0xFC7C50,
        Tomato = 0xFC6044,
        OrangeRed = 0xFC4400,
        Gold = 0xFCD400,
        Orange = 0xFCA400,
        DarkOrange = 0xFC8C00,
        LightYellow = 0xFCFCE0,
        LemonChiffon = 0xFCF8CC,
        LightGoldenRodYellow = 0xF8F8D0,
        PapayaWhip = 0xFCECD4,
        Moccasin = 0xFCE4B4,
        Peachpuff = 0xFCD8B8,
        PaleGoldenRod = 0xECE8A8,
        Khaki = 0xF0E48C,
        DarkKhaki = 0xBCB468,
        Yellow = 0xFCFC00,
        LawnGreen = 0x7CFC00,
        LimeGreen = 0x30CC30,
        Lime = 0xFC00,
        ForestGreen = 0x208820,
        Green = 0x8000,
        DarkGreen = 0x6400,
        GreenYellow = 0xACFC2C,
        YellowGreen = 0x98CC30,
        SpringGreen = 0xFC7C,
        MediumSpringGreen = 0xF898,
        LightGreen = 0x90EC90,
        PaleGreen = 0x98F898,
        DarkSeaGreen = 0x8CBC8C,
        MediumSeaGreen = 0x3CB070,
        SeaGreen = 0x2C8854,
        Olive = 0x808000,
        DarkOliveGreen = 0x54682C,
        OliveDrab = 0x688C20,
        LightCyan = 0xE0FCFC,
        Cyan = 0xFCFC,
        AquaMarine = 0x7CFCD4,
        MediumAquaMarine = 0x64CCA8,
        PaleTurquoise = 0xACECEC,
        Turquoise = 0x40E0D0,
        MediumTurquoise = 0x48D0CC,
        DarkTurquoise = 0xCCD0,
        LightSeaGreen = 0x20B0A8,
        CadetBlue = 0x5C9CA0,
        DarkCyan = 0x8888,
        Teal = 0x8080,
        PowderBlue = 0xB0E0E4,
        LightBlue = 0xACD8E4,
        LightSkyBlue = 0x84CCF8,
        SkyBlue = 0x84CCE8,
        DeepSkyBlue = 0xBCFC,
        LightSteelBlue = 0xB0C4DC,
        DodgerBlue = 0x1C90FC,
        CornFlowerBlue = 0x6494EC,
        SteelBlue = 0x4480B4,
        RoyalBlue = 0x4068E0,
        Blue = 0xFC,
        MediumBlue = 0xCC,
        DarkBlue = 0x88,
        Navy = 0x80,
        MidnightBlue = 0x181870,
        MediumSlateBlue = 0x7868EC,
        SlateBlue = 0x6858CC,
        DarkSlateBlue = 0x483C88,
        Pink = 0xFCC0C8,
        LightPink = 0xFCB4C0,
        HotPink = 0xFC68B4,
        DeepPink = 0xFC1490,
        PaleVioletRed = 0xD87090,
        MediumVioletRed = 0xC41484,
        White = 0xFCFCFC,
        Snow = 0xFCF8F8,
        HoneyDew = 0xF0FCF0,
        MintCream = 0xF4FCF8,
        Azure = 0xF0FCFC,
        AliceBlue = 0xF0F8FC,
        GhostWhite = 0xF8F8FC,
        WhiteSmoke = 0xF4F4F4,
        SeaShell = 0xFCF4EC,
        Beige = 0xF4F4DC,
        OldLace = 0xFCF4E4,
        FloralWhite = 0xFCF8F0,
        Ivory = 0xFCFCF0,
        AntiqueWhite = 0xF8E8D4,
        Linen = 0xF8F0E4,
        LavenderBlush = 0xFCF0F4,
        MistyRose = 0xFCE4E0,
        Gainsboro = 0xDCDCDC,
        LightGray = 0xD0D0D0,
        Silver = 0xC0C0C0,
        DarkGray = 0xA8A8A8,
        Gray = 0x808080,
        DimGray = 0x686868,
        LightSlateGray = 0x748898,
        SlateGray = 0x708090,
        DarksLateGray = 0x2C4C4C,
        Black = 0x0,
        CornSilk = 0xFCF8DC,
        BlanchedAlmond = 0xFCE8CC,
        Bisque = 0xFCE4C4,
        NavajoWhite = 0xFCDCAC,
        Wheat = 0xF4DCB0,
        BurlyWood = 0xDCB884,
        Tan = 0xD0B48C,
        RosyBrown = 0xBC8C8C,
        SandyBrown = 0xF4A460,
        GoldenRod = 0xD8A420,
        Peru = 0xCC843C,
        Chocolate = 0xD0681C,
        SaddleBrown = 0x884410,
        Sienna = 0xA0502C,
        Brown = 0xA42828,
        Maroon = 0x800000
    }
}
