using Iot.Device.Buzzer;
using nanoFramework.Hardware.Esp32;
using System;

namespace nanoFramework.M5Stick
{
    public static partial class M5StickCPlus
    {
        private static Buzzer _buzzer;

        /// <summary>
        /// Get the buzzer from the M5StickCPlus
        /// </summary>
        public static Buzzer Buzzer
        {
            get
            {
                if(_buzzer == null)
                {
                    Configuration.SetPinFunction(2, DeviceFunction.PWM1);
                    _buzzer = new(2);
                }

                return _buzzer;
            }
        }
    }
}
