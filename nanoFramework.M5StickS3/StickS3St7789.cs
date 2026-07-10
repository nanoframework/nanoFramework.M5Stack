// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.UI;

namespace nanoFramework.M5Stack
{
    /// <summary>
    /// M5StickS3-specific managed ST7789 driver.
    /// </summary>
    internal static class StickS3St7789
    {
        private static GraphicDriver _driver;

        private enum St7789Command : byte
        {
            SleepOut = 0x11,
            DisplayOn = 0x29,
            ColumnAddressSet = 0x2A,
            RowAddressSet = 0x2B,
            MemoryWrite = 0x2C,
            MemoryAccessControl = 0x36,
            ColorMode = 0x3A,
            InversionOn = 0x21,
        }

        private enum Orientation : byte
        {
            My = 0x80,
            Mx = 0x40,
            Mv = 0x20,
            Bgr = 0x08,
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
                        MemoryWrite = (byte)St7789Command.MemoryWrite,
                        SetColumnAddress = (byte)St7789Command.ColumnAddressSet,
                        SetRowAddress = (byte)St7789Command.RowAddressSet,
                        InitializationSequence = new byte[]
                        {
                            (byte)GraphicDriverCommandType.Command, 2, (byte)St7789Command.ColorMode, 0x55,
                            (byte)GraphicDriverCommandType.Command, 1, (byte)St7789Command.InversionOn,
                            (byte)GraphicDriverCommandType.Command, 1, (byte)St7789Command.SleepOut,
                            (byte)GraphicDriverCommandType.Sleep, 12,
                            (byte)GraphicDriverCommandType.Command, 1, (byte)St7789Command.DisplayOn,
                        },
                        OrientationLandscape = new byte[]
                        {
                            (byte)GraphicDriverCommandType.Command, 2, (byte)St7789Command.MemoryAccessControl, (byte)Orientation.Bgr,
                        },
                        OrientationPortrait = new byte[]
                        {
                            (byte)GraphicDriverCommandType.Command, 2, (byte)St7789Command.MemoryAccessControl, (byte)(Orientation.Mv | Orientation.Bgr),
                        },
                        OrientationLandscape180 = new byte[]
                        {
                            (byte)GraphicDriverCommandType.Command, 2, (byte)St7789Command.MemoryAccessControl, (byte)(Orientation.Mx | Orientation.Bgr),
                        },
                        OrientationPortrait180 = new byte[]
                        {
                            (byte)GraphicDriverCommandType.Command, 2, (byte)St7789Command.MemoryAccessControl, (byte)(Orientation.Mx | Orientation.My | Orientation.Mv | Orientation.Bgr),
                        },
                        DefaultOrientation = DisplayOrientation.Landscape,
                        SetWindowType = SetWindowType.X16bitsY16Bit,
                    };
                }

                return _driver;
            }
        }
    }
}
