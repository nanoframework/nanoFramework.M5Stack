// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.AtomLite;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace AtomLiteTestApp
{
    public class Program
    {
        public static void Main()
        {
            var button = AtomLite.Button;
            var rgb = AtomLite.NeoPixel;
            rgb.Image.SetPixel(0, 0, Color.FromArgb(255, 255, 0, 0));
            rgb.Update();

            button.Press += Button_Press;

            Debug.WriteLine("Hello from nanoFramework!");

            Thread.Sleep(Timeout.Infinite);
        }

        private static void Button_Press(object sender, EventArgs e)
        {
            var color = AtomLite.NeoPixel.Image.Data;
            // Coded as G, R, B
            if (color[1] > 0)
            {
                AtomLite.NeoPixel.Image.SetPixel(0, 0, Color.FromArgb(255, 0, 255, 0));
            }
            else if (color[0] > 0)
            {
                AtomLite.NeoPixel.Image.SetPixel(0, 0, Color.FromArgb(255, 0, 0, 255));
            }
            else
            {
                AtomLite.NeoPixel.Image.SetPixel(0, 0, Color.FromArgb(255, 255, 0, 0));
            }

            AtomLite.NeoPixel.Update();
        }
    }
}
