// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.M5Core2;
using nanoFramework.M5Stack;
using nanoFramework.Runtime.Native;
using System;
using System.Diagnostics;
using System.Threading;
using Console = nanoFramework.M5Stack.Console;

M5Core2.InitializeScreen();

Debug.WriteLine("Hello from M5Core2!");

M5Core2.TouchEvent += TouchEventCallback;

Thread.Sleep(Timeout.Infinite);

void TouchEventCallback(object sender, TouchEventArgs e)
{
    const string StrLB = "LEFT BUTTON PRESSED  ";
    const string StrMB = "MIDDLE BUTTON PRESSED  ";
    const string StrRB = "RIGHT BUTTON PRESSED  ";
    const string StrXY1 = "TOUCHED at X= ";
    const string StrXY2 = ",Y= ";
    const string StrID = ",Id= ";
    const string StrDoubleTouch = "Double touch. ";
    const string StrMove = "Moving... ";
    const string StrLiftUp = "Lift up. ";

    Debug.WriteLine($"Touch Panel Event Received Category= {e.EventCategory} Subcategory= {e.TouchEventCategory}");
    Console.CursorLeft = 0;
    Console.CursorTop = 0;

    Debug.WriteLine(StrXY1 + e.X + StrXY2 + e.Y + StrID + e.Id);
    Console.WriteLine(StrXY1 + e.X + StrXY2 + e.Y + StrID + e.Id + "  ");

    if ((e.TouchEventCategory & TouchEventCategory.LeftButton) == TouchEventCategory.LeftButton)
    {
        Debug.WriteLine(StrLB);
        Console.WriteLine(StrLB);
    }
    else if ((e.TouchEventCategory & TouchEventCategory.MiddleButton) == TouchEventCategory.MiddleButton)
    {
        Debug.WriteLine(StrMB);
        Console.WriteLine(StrMB);
    }
    else if ((e.TouchEventCategory & TouchEventCategory.RightButton) == TouchEventCategory.RightButton)
    {
        Debug.WriteLine(StrRB);
        Console.WriteLine(StrRB);
    }

    if ((e.TouchEventCategory & TouchEventCategory.Moving) == TouchEventCategory.Moving)
    {
        Debug.WriteLine(StrMove);
        Console.Write(StrMove);
    }

    if ((e.TouchEventCategory & TouchEventCategory.LiftUp) == TouchEventCategory.LiftUp)
    {
        Debug.WriteLine(StrLiftUp);
        Console.Write(StrLiftUp);
    }

    if ((e.TouchEventCategory & TouchEventCategory.DoubleTouch) == TouchEventCategory.DoubleTouch)
    {
        Debug.WriteLine(StrDoubleTouch);
        Console.Write(StrDoubleTouch);
    }

    Console.WriteLine("                                    ");
    Console.WriteLine("                                    ");
    Console.WriteLine("                                    ");
}
