// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Hardware.Esp32.Rmt;
using System.Drawing;

namespace nanoFramework.AtomMatrix
{
    /// <summary>
    /// Pixel controller for the WS2812C matrix.
    /// </summary>
    public class PixelController
    {
        // timming values from datasheet, considering a clock source of 80Mhz (which we have fixed in native)
        internal readonly RmtCommand onePulse = new(52, true, 52, false);

        internal readonly RmtCommand zeroPulse = new(14, true, 52, false);

        internal readonly RmtCommand ResetCommand = new(1400, false, 1400, false);
        internal int _gpioPin;

        /// <summary>
        /// Color for the LED matrix.
        /// </summary>
        /// <remarks>
        /// Index 0 is LED at top left corner. Growing from left to right, top to bottom.
        /// </remarks>
        public Color[] Pixels { get; set; }

        internal PixelController(int gpioPin, uint pixelCount)
        {
            _gpioPin = gpioPin;

            Pixels = new Color[pixelCount];

            for (uint i = 0; i < pixelCount; ++i)
            {
                Pixels[i] = new Color();
            }
        }

        /// <summary>
        /// Set specific color (RGB Format) on pixel the given index.
        /// </summary>
        /// <param name="index">Index of the LED to set the <see cref="Color"/>.</param>
        /// <param name="r"><see cref="Color.R"/> value to set.</param>
        /// <param name="g"><see cref="Color.G"/> value to set.</param>
        /// <param name="b"><see cref="Color.B"/> value to set.</param>
        public void SetColor(short index, byte r, byte g, byte b) => Pixels[index] = Color.FromArgb(r, g, b);

        /// <summary>
        /// Set specific color on LED at the given index.
        /// </summary>
        /// <param name="index">Index of the LED to set the <see cref="Color"/>.</param>
        /// <param name="color"><see cref="Color"/> value to set.</param>
        public void SetColor(short index, Color color) => Pixels[index] = color;

        /// <summary>
        /// Update all pixels.
        /// </summary>
        public void Update()
        {
            var transmitter = new TransmitterChannel(_gpioPin);

            ConfigureTransmitter(transmitter);

            for (uint pixel = 0; pixel < Pixels.Length; pixel++)
            {
                SerialiseColor(Pixels[pixel].G, transmitter);
                SerialiseColor(Pixels[pixel].R, transmitter);
                SerialiseColor(Pixels[pixel].B, transmitter);

                transmitter.AddCommand(ResetCommand);
            }

            transmitter.Send(true);

            transmitter.Dispose();
        }

        /// <summary>
        /// Turn off all pixels.
        /// </summary>
        public void TurnOff()
        {
            for (uint pixel = 0; pixel < Pixels.Length; pixel++)
            {
                Pixels[pixel] = Color.Black;
            }

            Update();
        }

        /// <summary>
        /// Fill in all LEDs with a color.
        /// </summary>
        /// <param name="color"><see cref="Color"/> to fill in the LEDs.</param>
        public void FillColor(Color color)
        {
            for (uint pixel = 0; pixel < Pixels.Length; pixel++)
            {
                Pixels[pixel] = color;
            }

            Update();
        }


        private void SerialiseColor(byte b, TransmitterChannel transmitter)
        {
            for (int i = 0; i < 8; ++i)
            {
                transmitter.AddCommand(((b & (1u << 7)) != 0) ? onePulse : zeroPulse);
                b <<= 1;
            }
        }

        private void ConfigureTransmitter(TransmitterChannel transmitter)
        {
            transmitter.CarrierEnabled = false;
            // this value for the clock divider considers a clock source of 80MHz which is what we have fixed in native
            transmitter.ClockDivider = 2;
            transmitter.IdleLevel = false;
        }
    }
}