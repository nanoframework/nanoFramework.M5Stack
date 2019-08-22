//
// Copyright (c) 2019 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

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
        static Lcd()
        {
            // TODO inits and configs
        }

        /// <summary>
        /// Switch the display to energy saving mode.
        /// </summary>
        /// <remarks>
        /// Call <see cref="Wakeup"/> to wake up the display.
        /// </remarks>
        public void Sleep()
        {

        }

        /// <summary>
        /// Restore the display from energy saving mode.
        /// </summary>
        /// <remarks>
        /// Since the LCD backlight of M5Stack is controlled separately, please adjust it with SetBrightness() if necessary.
        /// </remarks>
        public void Wakeup()
        {

        }

        #region native handlers

        // TODO native handler methods will be placed here

        #endregion
    }
}
