// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Hardware.Esp32.Rmt;
using System.Drawing;

namespace nanoFramework.M5AtomLite
{
    /// <summary>
    /// The RGB Led controller for M5Stack Atom Lite. 
    /// </summary>
    public class RgbLed
    {
        protected const int RgbLedPin = 27;
        // 80MHz / 4 => min pulse 0.00us
        protected const byte ClockDivider = 4;
        // one pulse duration in us
        protected const float MinPulse = 1000000.0f / (80000000 / ClockDivider);

        // default datasheet values
        protected readonly RmtCommand OnePulse =
            new RmtCommand((ushort)(0.7 / MinPulse), true, (ushort)(0.6 / MinPulse), false);

        protected readonly RmtCommand ZeroPulse =
            new RmtCommand((ushort)(0.35 / MinPulse), true, (ushort)(0.8 / MinPulse), false);

        protected readonly RmtCommand ResCommand =
            new RmtCommand((ushort)(25 / MinPulse), false, (ushort)(26 / MinPulse), false);

        protected Color Pixel;
        private readonly int _gpioPin;

        internal RgbLed(int gpioPin = RgbLedPin)
        {
            _gpioPin = gpioPin;
            Pixel = Color.Black;

        }

        /// <summary>
        /// Sets the NeoPixel to the specified rgb color.
        /// </summary>
        /// <param name="color">The color that is used</param>
        public void SetColor(Color color)
        {
            Pixel = color;
            Update();
        }

        /// <summary>
        /// Gets the rgb color from the NeoPixel.
        /// </summary>
        /// <returns>The rgb color</returns>
        public Color GetColor()
        {
            return Pixel;
        }

        private void Update()
        {
            using (var commandList = new TransmitterChannel(_gpioPin))
            {
                ConfigureTransmitter(commandList);

                SerializeColor(Pixel.G, commandList);
                SerializeColor(Pixel.R, commandList);
                SerializeColor(Pixel.B, commandList);

                commandList.AddCommand(ResCommand); // RET
                commandList.Send(true);
            }
        }

        private void SerializeColor(byte b, TransmitterChannel commandList)
        {
            for (var i = 0; i < 8; ++i)
            {
                commandList.AddCommand(((b & (1u << 7)) != 0) ? OnePulse : ZeroPulse);
                b <<= 1;
            }
        }

        protected void ConfigureTransmitter(TransmitterChannel commandList)
        {
            commandList.CarrierEnabled = false;
            commandList.ClockDivider = ClockDivider;
            commandList.SourceClock = SourceClock.APB;
            commandList.IdleLevel = false;
            commandList.IsChannelIdle = true;
        }
    }
}
