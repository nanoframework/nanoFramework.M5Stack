// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.AtomMatrix;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace AtomMatrixTestApp
{
    public class Program
    {
        public static void Main()
        {
            var button = AtomMatrix.Button;

            var rgb = AtomMatrix.LedMatrix;
            rgb.SetColor(0, 0, 255, 0);
            rgb.SetColor(5+1, 0, 255, 0);
            rgb.SetColor(10+2, 0, 255, 0);
            rgb.SetColor(15+3, 0, 255, 0);
            rgb.SetColor(20+4, 0, 255, 0);
            rgb.Update();

            rgb.SetColor(0, 0, 0, 255);
            rgb.SetColor(5 + 1, 0, 0, 255);
            rgb.SetColor(10 + 2, 0, 0, 255);
            rgb.SetColor(15 + 3, 0, 0, 255);
            rgb.SetColor(20 + 4, 0, 0, 255);
            rgb.Update();

            button.Press += Button_Press;

            Debug.WriteLine("Hello from nanoFramework!");

            Thread.Sleep(Timeout.Infinite);
        }

        private static void Button_Press(object sender, EventArgs e)
        {
            //var color = AtomMatrix.NeoPixel.GetColor();
            //if(color.R > 0)
            //{
            //    AtomMatrix.NeoPixel.SetColor(Color.FromArgb(0, 255, 0, 255));
            //}
            //else if (color.G > 0)
            //{
            //    AtomMatrix.NeoPixel.SetColor(Color.FromArgb(0, 255, 0, 255));
            //}
            //else
            //{
            //    AtomMatrix.NeoPixel.SetColor(Color.FromArgb(255, 0, 255, 0));
            //}
        }
    }
}
