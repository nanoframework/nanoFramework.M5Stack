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
            rgb.SetColor(Color.FromArgb(255, 255, 0, 0));

            button.Press += Button_Press;

            Debug.WriteLine("Hello from nanoFramework!");

            Thread.Sleep(Timeout.Infinite);
        }

        private static void Button_Press(object sender, EventArgs e)
        {
            var color = AtomLite.NeoPixel.GetColor();
            if(color.R > 0)
            {
                AtomLite.NeoPixel.SetColor(Color.FromArgb(255, 0, 255, 0));
            }
            else if (color.G > 0)
            {
                AtomLite.NeoPixel.SetColor(Color.FromArgb(255, 0, 0, 255));
            }
            else
            {
                AtomLite.NeoPixel.SetColor(Color.FromArgb(255, 255, 0, 0));
            }
        }
    }
}
