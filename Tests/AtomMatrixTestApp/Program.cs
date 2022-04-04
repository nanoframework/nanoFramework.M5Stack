// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Iot.Device.Ws28xx.Esp32;
using nanoFramework.AtomMatrix;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace AtomMatrixTestApp
{
    public class Program
    {
        private static Ws2812c _ledMatrix;

        public static void Main()
        {
            var button = AtomMatrix.Button;

            _ledMatrix = AtomMatrix.LedMatrix;

            // diagonal green line
            DrawDiagonalLine(Color.Green);
            Thread.Sleep(1000);

            _ledMatrix.Image.Clear();
            _ledMatrix.Update();

            // diagonal blue line
            DrawDiagonalLine(Color.Blue);
            Thread.Sleep(1000);

            _ledMatrix.Image.Clear();
            _ledMatrix.Update();

            // diagonal red line
            DrawDiagonalLine(Color.Red);
            Thread.Sleep(1000);

            // clear LEDs
            _ledMatrix.Image.Clear();
            _ledMatrix.Update();

            button.ButtonDown += Button_ButtonDown;
            button.ButtonUp += Button_ButtonUp;

            Debug.WriteLine("Hello from nanoFramework!");

            Thread.Sleep(Timeout.Infinite);
        }

        private static void DrawDiagonalLine(Color color)
        {
            _ledMatrix.Image.SetPixel(0, 0, color);
            _ledMatrix.Image.SetPixel(1, 1, color);
            _ledMatrix.Image.SetPixel(2, 2, color);
            _ledMatrix.Image.SetPixel(3, 3, color);
            _ledMatrix.Image.SetPixel(4, 4, color);
            _ledMatrix.Update();
        }

        private static void Button_ButtonUp(object sender, EventArgs e)
        {
            _ledMatrix.Image.Clear();
            _ledMatrix.Update();
        }

        private static void Button_ButtonDown(object sender, EventArgs e)
        {
            _ledMatrix.Image.Clear(Color.Green);
            _ledMatrix.Update();
        }
    }
}
