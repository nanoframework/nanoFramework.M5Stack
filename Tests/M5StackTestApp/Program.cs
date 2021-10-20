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
    screen.Clear();
    screen.Write($"Left Pressed", 0, 0, 320, 240, displayFont, Color.Yellow, Color.Black);
};

M5Stack.ButtonCenter.Press += (sender, e) =>
{
    screen.Clear();
    screen.Write($"Center Pressed", 0, 0, 320, 240, displayFont, Color.Yellow, Color.Black);
};

M5Stack.ButtonRight.Press += (sender, e) =>
{
    screen.Clear();
    screen.Write($"Right Pressed", 0, 0, 320, 240, displayFont, Color.Yellow, Color.Black);
};

Thread.Sleep(Timeout.Infinite);
