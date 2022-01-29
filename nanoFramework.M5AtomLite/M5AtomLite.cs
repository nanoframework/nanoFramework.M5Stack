using Iot.Device.Button;
using System;
using System.Device.Gpio;

namespace nanoFramework.M5AtomLite
{
    public static class M5AtomLite {
        
        private static GpioButton _button;
        private static RgbLed _rgbLed;
        private static GpioController _gpio;

        /// <summary>
        /// Main button.
        /// </summary>
        public static GpioButton Button
        {
            get
            {
                if (_button == null)
                {
                    _button = new(39, _gpio, false);
                }

                return _button;
            }
        }

        /// <summary>
        /// RGB NeoPixel led
        /// </summary>
        public static RgbLed NeoPixel
        {
            get
            {
                if (_rgbLed == null)
                {
                    _rgbLed = new();
                }

                return _rgbLed;

            }
        }

        /// <summary>
        /// Gets the main GPIO Controller.
        /// </summary>
        public static GpioController GpioController => _gpio;

    }
}
