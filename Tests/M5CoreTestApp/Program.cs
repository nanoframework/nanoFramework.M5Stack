// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.M5Stack;
using nanoFramework.Presentation.Media;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading;
using Console = nanoFramework.M5Stack.Console;

Debug.WriteLine("Hello from nanoFramework!");

M5Core.InitializeScreen();
Console.Clear();

// Test the console display
Console.Write("This is a short text. ");
Console.ForegroundColor = Color.Red;
Console.WriteLine("This one displays in red after the previous one and is already at the next line.");
Console.BackgroundColor = Color.Yellow;
Console.ForegroundColor = Color.RoyalBlue;
Console.WriteLine("And this is really ugly but it's like that");
Console.ResetColor();
Console.Write("*@$+=}");
Console.WriteLine("*@$+=}");
Console.WriteLine("");
Console.WriteLine("1 line empty before");
Console.WriteLine("Press left button to continue");

while (!M5Core.ButtonLeft.IsPressed)
{
    Thread.Sleep(10);
}

Console.Clear();

Console.WriteLine("Calibrating the accelerator, do not touch it!");
var acc = M5Core.AccelerometerGyroscope;
acc.Calibrate(100);
Console.WriteLine("");
Console.WriteLine("Calibrating the magnetometer, please move it all around");
var mag = M5Core.Magnetometer;
mag.CalibrateMagnetometer(100);

M5Core.ButtonLeft.Press += (sender, e) =>
{
    Console.ForegroundColor = Color.Yellow;
    Console.CursorLeft = 0;
    Console.CursorTop = 0;
    Console.Write($"Left Pressed  ");
};

M5Core.ButtonCenter.Press += (sender, e) =>
{
    Console.ForegroundColor = Color.Yellow;
    Console.CursorLeft = 0;
    Console.CursorTop = 0;
    Console.Write($"Center Pressed");
};

M5Core.ButtonRight.Press += (sender, e) =>
{
    Console.ForegroundColor = Color.Yellow;
    Console.CursorLeft = 0;
    Console.CursorTop = 0;
    Console.Write($"Right Pressed ");
};

Console.Clear();

var power = M5Core.Power;
Vector3 accVal;
Vector3 gyroVal;
Vector3 magVal;

while (true)
{
    accVal = acc.GetAccelerometer();
    gyroVal = acc.GetGyroscope();
    magVal = mag.ReadMagnetometer();
    var headDir = Math.Atan2(magVal.X, magVal.Y) * 180.0 / Math.PI;
    Console.ForegroundColor = Color.Green;
    Console.CursorLeft = 0;
    Console.CursorTop = 1;
    Console.WriteLine("Accelerator:");
    Console.WriteLine($"  x={accVal.X:N2} ");
    Console.WriteLine($"  y={accVal.Y:N2} ");
    Console.WriteLine($"  z={accVal.Z:N2} ");
    Console.ForegroundColor = Color.AliceBlue;
    Console.WriteLine("Gyroscope:");
    Console.WriteLine($"  x={gyroVal.X:N2}  ");
    Console.WriteLine($"  y={gyroVal.Y:N2}  ");
    Console.WriteLine($"  z={gyroVal.Z:N2}  ");
    Console.ForegroundColor = Color.Coral;
    Console.CursorLeft = Console.WindowWidth / 2 - 2;
    Console.CursorTop = 1;
    Console.Write("Magnetometer:");
    Console.CursorLeft = Console.WindowWidth / 2 - 2;
    Console.CursorTop++;
    Console.Write($"  x={magVal.X:N2}   ");
    Console.CursorLeft = Console.WindowWidth / 2 - 2;
    Console.CursorTop++;
    Console.Write($"  y={magVal.Y:N2}   ");
    Console.CursorLeft = Console.WindowWidth / 2 - 2;
    Console.CursorTop++;
    Console.Write($"  z={magVal.Z:N2}   ");
    Console.CursorLeft = Console.WindowWidth / 2 - 2;
    Console.CursorTop++;
    Console.Write($"  h={headDir:N2}  ");
    Console.ForegroundColor = Color.DarkBlue;
    Console.CursorLeft = Console.WindowWidth / 2 - 2;
    Console.CursorTop = 6;
    Console.Write("Power:");
    Console.CursorLeft = Console.WindowWidth / 2 - 2;
    Console.CursorTop++;
    Console.BackgroundColor = power.IsCharging ? Color.Black : Color.Red;
    Console.Write($"  Charging {power.IsCharging}");
    Console.BackgroundColor = Color.Black;
    Console.Write("  ");
    Console.CursorLeft = Console.WindowWidth / 2 - 2;
    Console.CursorTop++;
    Console.Write($"  Full {power.IsBatteryFull} ");
    Thread.Sleep(20);
}

Thread.Sleep(Timeout.Infinite);
