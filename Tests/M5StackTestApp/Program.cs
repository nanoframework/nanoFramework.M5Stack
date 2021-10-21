using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.Threading;
using M5StackTestApp;
using nanoFramework.M5Stack;
using nanoFramework.UI;
using nanoFramework.Presentation.Media;

Debug.WriteLine("Hello from nanoFramework!");

Font displayFont = Resources.GetFont(Resources.FontResources.segoeuiregular12);
Screen screen = M5Stack.Screen;
screen.Clear();

M5Stack.ButtonLeft.Press += (sender, e) =>
{
    Screen.ForegroundColor = Color.Yellow;
    screen.Write($"Left Pressed   ", 0, 0, 320, 240);
};

M5Stack.ButtonCenter.Press += (sender, e) =>
{
    Screen.ForegroundColor = Color.Yellow;
    screen.Write($"Center Pressed   ", 0, 0, 320, 240);
};

M5Stack.ButtonRight.Press += (sender, e) =>
{
    Screen.ForegroundColor = Color.Yellow;
    screen.Write($"Right Pressed   ", 0, 0, 320, 240);
};

var acc = M5Stack.AccelerometerGyroscope;

while(true)
{
    var accVal = acc.GetAccelerometer();
    //screen.Clear();
    Screen.ForegroundColor = Color.Green;
    screen.Write($"x {accVal.X:N2}   ", 0, 20, 320, 200);
    screen.Write($"y {accVal.Y:N2}   ", 0, 40, 320, 200);
    screen.Write($"z {accVal.Z:N2}   ", 0, 60, 320, 180);
    Thread.Sleep(20);
}

Thread.Sleep(Timeout.Infinite);
