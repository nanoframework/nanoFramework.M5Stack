using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.Threading;
using M5StackTestApp;
using nanoFramework.M5Stack;
using nanoFramework.UI;
using nanoFramework.Presentation.Media;
using nanoFramework;

Debug.WriteLine("Hello from nanoFramework!");

M5Stack.InitilazeScreen();
Console.Clear();

// Test the console display
Console.Write("This is a short text. ");
Console.ForegroundColor = Color.Red;
Console.WriteLine("This one displays in red after the previous one and is already at the next line.");
Console.BackgroundColor = Color.Yellow;
Console.ForegroundColor = Color.RoyalBlue;
Console.WriteLine("And this is really ugly but it's like that");
Console.ResetColor();
Console.Write("*ù$+=}");
Console.WriteLine("");
Console.WriteLine("1 line empty before");

M5Stack.ButtonLeft.Press += (sender, e) =>
{
    Console.ForegroundColor = Color.Yellow;
    Console.CursorLeft = 0;
    Console.CursorTop = 0;
    Console.Write($"Left Pressed  ");
};

M5Stack.ButtonCenter.Press += (sender, e) =>
{
    Console.ForegroundColor = Color.Yellow;
    Console.CursorLeft = 0;
    Console.CursorTop = 0;
    Console.Write($"Center Pressed");
};

M5Stack.ButtonRight.Press += (sender, e) =>
{
    Console.ForegroundColor = Color.Yellow;
    Console.CursorLeft = 0;
    Console.CursorTop = 0;
    Console.Write($"Right Pressed ");
};

var acc = M5Stack.AccelerometerGyroscope;

while(true)
{
    var accVal = acc.GetAccelerometer();
    Console.ForegroundColor = Color.Green;
    Console.CursorLeft = 0;
    Console.CursorTop = 2;
    Console.Write($"x={accVal.X:N2} ");
    Console.CursorLeft = 0;
    Console.CursorTop++;
    Console.Write($"y={accVal.Y:N2} ");
    Console.CursorLeft = 0;
    Console.CursorTop++;
    Console.Write($"z={accVal.Z:N2} ");
    Thread.Sleep(20);
}

Thread.Sleep(Timeout.Infinite);
