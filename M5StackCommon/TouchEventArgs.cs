// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if TOUGH || M5CORE2
using nanoFramework.Runtime.Events;
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
    /// Touch event arguments
    /// </summary>
    public class TouchEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the touch event sub category.
        /// </summary>
        public TouchEventCategory TouchEventCategory { get; set; }

        /// <summary>
        /// Gets or sets the event category.
        /// </summary>
        public EventCategory EventCategory { get; set; }

        /// <summary>
        /// Gets or sets the coordinates of the point X.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the coordinates of the point Y.
        /// </summary>
        public int Y { get; set; }

#if M5CORE2
        /// <summary>
        /// Gets or sets the contacty point Id. This is useful in a multi point context.
        /// </summary>
        public byte Id { get; set; }
#endif
        /// <summary>
        /// Gets or sets the time stamp.
        /// </summary>
        public DateTime TimeStamp { get; set; }
    }
}
#endif