// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace nanoFramework.M5Core2
{
    /// <summary>
    /// Sub event touch catgory
    /// </summary>
    [Flags]
    public enum TouchEventCategory
    {
        /// <summary>Unknown</summary>
        Unknown = 0b0000_0000,

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
    }
}
