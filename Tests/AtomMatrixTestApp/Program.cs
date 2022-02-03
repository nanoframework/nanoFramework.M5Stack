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
        private static PixelController _ledMatrix;

        public static void Main()
        {
            var button = AtomMatrix.Button;

            _ledMatrix = AtomMatrix.LedMatrix;

            // diagonal green line
            _ledMatrix.SetColor(0, 0, 255, 0);
            _ledMatrix.SetColor(5 + 1, 0, 255, 0);
            _ledMatrix.SetColor(10 + 2, 0, 255, 0);
            _ledMatrix.SetColor(15 + 3, 0, 255, 0);
            _ledMatrix.SetColor(20 + 4, 0, 255, 0);
            _ledMatrix.Update();

            Thread.Sleep(1000);
            _ledMatrix.TurnOff();

            // diagonal blue line
            _ledMatrix.SetColor(0, 0, 0, 255);
            _ledMatrix.SetColor(5 + 1, 0, 0, 255);
            _ledMatrix.SetColor(10 + 2, 0, 0, 255);
            _ledMatrix.SetColor(15 + 3, 0, 0, 255);
            _ledMatrix.SetColor(20 + 4, 0, 0, 255);
            _ledMatrix.Update();

            Thread.Sleep(1000);
            _ledMatrix.TurnOff();

            // diagonal red line
            _ledMatrix.SetColor(0, 255, 0, 0);
            _ledMatrix.SetColor(5 + 1, 255, 0, 0);
            _ledMatrix.SetColor(10 + 2, 255, 0, 0);
            _ledMatrix.SetColor(15 + 3, 255, 0, 0);
            _ledMatrix.SetColor(20 + 4, 255, 0, 0);
            _ledMatrix.Update();

            Thread.Sleep(1000);

            // clear LEDs
            _ledMatrix.TurnOff();

            button.ButtonDown += Button_ButtonDown;
            button.ButtonUp += Button_ButtonUp;

            Debug.WriteLine("Hello from nanoFramework!");

            Thread.Sleep(Timeout.Infinite);
        }

        private static void Button_ButtonUp(object sender, EventArgs e)
        {
            _ledMatrix.TurnOff();
        }

        private static void Button_ButtonDown(object sender, EventArgs e)
        {
            _ledMatrix.FillColor(Color.Green);
        }
    }
}
