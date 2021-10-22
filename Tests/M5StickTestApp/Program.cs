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

while(!M5StickC.M5Button.IsPressed)
{
    Thread.Sleep(10);
}

Console.Clear();
Console.ForegroundColor = Color.Pink;
Console.Write("this is a long line which will go over");
Console.WriteLine("");
Console.Write("Press the right button");

while (!M5StickC.RightButton.IsPressed)
{
    Thread.Sleep(10);
}

Console.Clear();
Console.Write("Click any button to check, double click M5 to exit");

bool nextDemo = false;
M5StickC.M5Button.DoublePress += (sender, e) =>
{
    nextDemo = true;
};

M5StickC.RightButton.Press += (sender, e) =>
{
    Console.CursorLeft = 0;
    Console.CursorTop = 2;
    Console.Write("Right pressed");
};


M5StickC.M5Button.Press += (sender, e) =>
{
    Console.CursorLeft = 0;
    Console.CursorTop = 2;
    Console.Write("M5 pressed    ");
};

while(!nextDemo)
{
    Thread.Sleep(100);
}

Thread.Sleep(Timeout.Infinite);
