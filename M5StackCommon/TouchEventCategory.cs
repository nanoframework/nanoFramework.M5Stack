// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if TOUGH || M5CORE2
using System;
#endif

#if TOUGH
namespace nanoFramework.Tough
#elif M5CORE2
namespace nanoFramework.M5Core2
#endif
#if TOUGH || M5CORE2
{
    /// <summary>
    /// Sub event touch category.
    /// </summary>
#if M5CORE2
    [Flags]
#endif
    public enum TouchEventCategory
    {
        /// <summary>Unknown.</summary>
        Unknown = 0b0000_0000,

#if M5CORE2
        /// <summary>Left Button</summary>
        LeftButton = 0b0000_0001,

        /// <summary>Middle Button</summary>
        MiddleButton = 0b0000_0010,

        /// <summary>Right Button</summary>
        RightButton = 0b0000_0100,

        /// <summary>Double Touch</summary>
        DoubleTouch = 0b0000_1000,

        /// <summary>Moving</summary>
        Moving = 0b0001_0000,

        /// <summary>Lift Up</summary>
        LiftUp = 0b0010_0000,
#else
        /// <summary>
        /// Screen touched.
        /// </summary>
        ScreenTouch,  

        /// <summary>
        /// Touch gone.
        /// </summary>
        TouchGone,
#endif
    }
}
#endif