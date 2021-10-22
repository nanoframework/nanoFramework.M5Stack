using nanoFramework.Presentation.Media;
using nanoFramework.UI;
using System;
using System.Device.Gpio;

namespace nanoFramework.M5Stack
{
    /// <summary>
    /// M5 Stack screen class
    /// </summary>
    public class Screen : ScreenBase
    {
        private const int ChipSelect = 14;
        private const int DataCommand = 27;
        private const int Reset = 33;

        static Screen()
        {
            BackLightPin = 32;
            Controller = new();
            Controller.OpenPin(BackLightPin, PinMode.Output);
            Enabled = true;
            DisplayControl.Initialize(new SpiConfiguration(2, ChipSelect, DataCommand, Reset, BackLightPin), new ScreenConfiguration(0, 0, 320, 240), (uint)MemoryAllocationBitmap);
        }
    }
}
