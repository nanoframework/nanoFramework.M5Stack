// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework;
using nanoFramework.M5Stick;
using nanoFramework.Presentation.Media;
using System;
using System.Diagnostics;
using System.Threading;


Debug.WriteLine("Hello M5Stick from nanoFramework!");

M5StickC.InitializeScreen();

Debug.WriteLine($"Actual time: {DateTime.UtcNow}");

ushort[] toSend = new ushort[100];
var blue = ColorUtility.To16Bpp(Color.Blue);
var red = ColorUtility.To16Bpp(Color.Red);
var green = ColorUtility.To16Bpp(Color.Green);
var white = ColorUtility.To16Bpp(Color.White);

for (int i = 0; i < toSend.Length; i++)
{
    toSend[i] = blue;
}

Screen.Write(0, 0, 10, 10, toSend);

for (int i = 0; i < toSend.Length; i++)
{
    toSend[i] = red;
}

Screen.Write(69, 0, 10, 10, toSend);

for (int i = 0; i < toSend.Length; i++)
{
    toSend[i] = green;
}

Screen.Write(0, 149, 10, 10, toSend);

for (int i = 0; i < toSend.Length; i++)
{
    toSend[i] = white;
}

Screen.Write(69, 149, 10, 10, toSend);

Console.CursorTop = 1;
Console.Write("Press the main M5 button");

while(!M5StickC.ButtonM5.IsPressed)
{
    Thread.Sleep(10);
}

Console.Clear();
Console.ForegroundColor = Color.Pink;
Console.Write("this is a long line which will go over");
Console.WriteLine("");
Console.Write("Press the right button");

while (!M5StickC.ButtonRight.IsPressed)
{
    Thread.Sleep(10);
}

Console.Clear();
Console.Write("Click any button to check, hold M5 to exit");

bool nextDemo = false;
M5StickC.ButtonM5.IsHoldingEnabled = true;
M5StickC.ButtonM5.Holding += (sender, e) =>
{
    nextDemo = true;
};

M5StickC.ButtonRight.Press += (sender, e) =>
{
    Console.ForegroundColor = Color.Green;
    Console.CursorLeft = 0;
    Console.CursorTop = 4;
    Console.Write("Right pressed");
};


M5StickC.ButtonM5.Press += (sender, e) =>
{
    Console.ForegroundColor = Color.Pink;
    Console.CursorLeft = 0;
    Console.CursorTop = 4;
    Console.Write("M5 pressed    ");
};

while (!nextDemo)
{
    Thread.Sleep(10);
}


Console.Clear();
Console.Write("End of demo");

Thread.Sleep(Timeout.Infinite);
