// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.UI;

namespace nanoFramework.M5Stack
{
    /// <summary>
    /// CoreS3-specific managed ILI9342 driver.
    /// </summary>
    internal static class CoreS3Ili9342
    {
        private static GraphicDriver _driver;

        private enum ILI9342Command : byte
        {
            Nop = 0x00,
            PowerState = 0x10,
            SleepOut = 0x11,            InversionOff = 0x20,
            InversionOn = 0x21,            IdleModeOff = 0x38,
            DisplayOn = 0x29,
            ColumnAddressSet = 0x2A,
            PageAddressSet = 0x2B,
            MemoryWrite = 0x2C,
            MemoryAccessControl = 0x36,
            ColorMode = 0x3A,
            WriteDisplayBrightness = 0x51,
            InterfaceSignalControl = 0xB0,
            DisplayFunctionControl = 0xB6,
            PowerControl1 = 0xC0,
            PowerControl2 = 0xC1,
            VcomControl1 = 0xC5,
            ExternalCommand = 0xC8,
            PositiveGammaCorrection = 0xE0,
            NegativeGammaCorrection = 0xE1,
            InterfaceControl = 0xF6,
        }

        private enum Orientation : byte
        {
            My = 0x80,   // Vertical flip
            Mx = 0x40,   // Horizontal flip
            Mv = 0x20,   // Row/column exchange (90° rotate)
            Ml = 0x10,   // Vertical refresh order
            Bgr = 0x08,  // BGR color order
            Mh = 0x04,   // Horizontal refresh order
        }

        public static GraphicDriver GraphicDriver
        {
            get
            {
                if (_driver == null)
                {
                    _driver = new GraphicDriver()
                    {
                        BitsPerPixel = 16,
                        MemoryWrite = (byte)ILI9342Command.MemoryWrite,
                        SetColumnAddress = (byte)ILI9342Command.ColumnAddressSet,
                        SetRowAddress = (byte)ILI9342Command.PageAddressSet,
                        InitializationSequence = new byte[]
                        {
                            // Match M5GFX Panel_ILI9342 list0 exactly.
                            (byte)GraphicDriverCommandType.Command, 4, (byte)ILI9342Command.ExternalCommand, 0xFF, 0x93, 0x42,
                            (byte)GraphicDriverCommandType.Command, 3, (byte)ILI9342Command.PowerControl1, 0x12, 0x12,
                            (byte)GraphicDriverCommandType.Command, 2, (byte)ILI9342Command.PowerControl2, 0x03,
                            (byte)GraphicDriverCommandType.Command, 2, (byte)ILI9342Command.VcomControl1, 0xF2,
                            (byte)GraphicDriverCommandType.Command, 2, (byte)ILI9342Command.InterfaceSignalControl, 0xE0,
                            (byte)GraphicDriverCommandType.Command, 4, (byte)ILI9342Command.InterfaceControl, 0x01, 0x00, 0x00,
                            // Force 16-bit RGB565 pixel format for nanoFramework write path.
                            (byte)GraphicDriverCommandType.Command, 2, (byte)ILI9342Command.ColorMode, 0x55,
                            (byte)GraphicDriverCommandType.Command, 16, (byte)ILI9342Command.PositiveGammaCorrection, 0x00, 0x0C, 0x11, 0x04, 0x11, 0x08, 0x37, 0x89, 0x4C, 0x06, 0x0C, 0x0A, 0x2E, 0x34, 0x0F,
                            (byte)GraphicDriverCommandType.Command, 16, (byte)ILI9342Command.NegativeGammaCorrection, 0x00, 0x0B, 0x11, 0x05, 0x13, 0x09, 0x33, 0x67, 0x48, 0x07, 0x0E, 0x0B, 0x2E, 0x33, 0x0F,
                            (byte)GraphicDriverCommandType.Command, 5, (byte)ILI9342Command.DisplayFunctionControl, 0x08, 0x82, 0x1D, 0x04,
                            (byte)GraphicDriverCommandType.Command, 1, (byte)ILI9342Command.InversionOn,
                            (byte)GraphicDriverCommandType.Command, 1, (byte)ILI9342Command.IdleModeOff,
                            (byte)GraphicDriverCommandType.Command, 1, (byte)ILI9342Command.SleepOut,
                            (byte)GraphicDriverCommandType.Sleep, 12,
                            (byte)GraphicDriverCommandType.Command, 1, (byte)ILI9342Command.DisplayOn,
                        },
                        // MADCTL values derived from M5GFX Panel_LCD::getMadCtl table with offset_rotation=3.
                        // Uses symbolic Orientation enum values for clarity.
                        OrientationLandscape = new byte[]
                        {
                            (byte)GraphicDriverCommandType.Command, 2, (byte)ILI9342Command.MemoryAccessControl, (byte)Orientation.Bgr,
                        },
                        OrientationPortrait = new byte[]
                        {
                            (byte)GraphicDriverCommandType.Command, 2, (byte)ILI9342Command.MemoryAccessControl, (byte)(Orientation.Mx | Orientation.Mv | Orientation.Bgr | Orientation.Mh),
                        },
                        OrientationLandscape180 = new byte[]
                        {
                            (byte)GraphicDriverCommandType.Command, 2, (byte)ILI9342Command.MemoryAccessControl, (byte)(Orientation.My | Orientation.Mx | Orientation.Ml | Orientation.Bgr | Orientation.Mh),
                        },
                        OrientationPortrait180 = new byte[]
                        {
                            (byte)GraphicDriverCommandType.Command, 2, (byte)ILI9342Command.MemoryAccessControl, (byte)(Orientation.My | Orientation.Mv | Orientation.Ml | Orientation.Bgr),
                        },
                        PowerModeNormal = new byte[]
                        {
                            (byte)GraphicDriverCommandType.Command, 3, (byte)ILI9342Command.PowerState, 0x00, 0x00,
                        },
                        PowerModeSleep = new byte[]
                        {
                            (byte)GraphicDriverCommandType.Command, 3, (byte)ILI9342Command.PowerState, 0x00, 0x01,
                        },
                        DefaultOrientation = DisplayOrientation.Landscape,
                        Brightness = (byte)ILI9342Command.WriteDisplayBrightness,
                        SetWindowType = SetWindowType.X16bitsY16Bit,
                    };
                }

                return _driver;
            }
        }
    }
}