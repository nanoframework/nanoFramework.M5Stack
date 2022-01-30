// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using nanoFramework.M5AtomLite;

namespace M5AtomLiteTestApp
{
    public class Program
    {
        public static void Main()
        {
            var button = M5AtomLite.Button;
            var rgb = M5AtomLite.NeoPixel;
            rgb.SetColor(Color.FromArgb(255, 255, 0, 0));

            button.Press += Button_Press;

            Debug.WriteLine("Hello from nanoFramework!");

            Thread.Sleep(Timeout.Infinite);
        }

        private static void Button_Press(object sender, EventArgs e)
        {
            var color = M5AtomLite.NeoPixel.GetColor();
            if(color.R > 0)
            {
                M5AtomLite.NeoPixel.SetColor(Color.FromArgb(255, 0, 255, 0));
            }
            else if (color.G > 0)
            {
                M5AtomLite.NeoPixel.SetColor(Color.FromArgb(255, 0, 0, 255));
            }
            else
            {
                M5AtomLite.NeoPixel.SetColor(Color.FromArgb(255, 255, 0, 0));
            }
        }
    }
}
