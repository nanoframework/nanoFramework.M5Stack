// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using nanoFramework.M5Core2;
using nanoFramework.M5Stack;
using nanoFramework.Networking;
using Console = nanoFramework.M5Stack.Console;

const string StrLB = "LEFT BUTTON PRESSED ";
const string StrMB = "MIDDLE BUTTON PRESSED";
const string StrRB = "RIGHT BUTTON PRESSED";
const string StrTouchEvent = "TOUCHED at (x,y): ";
const string StrEventID = ", Id=";
const string StrTouchEventCatagory = "Event Catagory: ";
const string StrDoubleTouch = "Double Touch.";
const string StrMove = "Moving...";
const string StrLiftUp = "Lift Up.";
const string StrGyro = "Gyroscope (x,y,z): ";
const string StrAcc = "Accelerometer (x,y,z): ";
const string StrCpuT = "CPU Temperature: ";

// Ensure set to false as can otherwise cause a long vibrate on deploy!
M5Core2.Vibrate = false; 
M5Core2.InitializeScreen();

Debug.WriteLine("Hello from M5Core2!");
Console.WriteLine("Hello from M5Core2!");
Vibrate(500);


// It is strongly recommended to connect to the wifi while testing the touch interface
// as wifi can create discrepancies on the touch interrupt pins.
// This has been managed in the driver but should be tested as well.
const string Ssid = "YourWifiSsidHere";
const string Password = "YourWifiPasswordHere";

// Give 60 seconds to the wifi join to happen
CancellationTokenSource cs = new(60000);
var success = WifiNetworkHelper.ConnectDhcp(Ssid, Password, requiresDateTime: true, token: cs.Token);
if (!success)
{
    // Something went wrong, you can get details with the ConnectionError property:
    Debug.WriteLine($"Can't connect to the network, error: {WifiNetworkHelper.Status}");
    if (WifiNetworkHelper.HelperException != null)
    {
        Debug.WriteLine($"ex: {WifiNetworkHelper.HelperException}");
    }
    Console.ForegroundColor = Color.Red;
    Console.WriteLine($"Error connecting to WiFi SSID: {Ssid}.");
    Console.WriteLine("Check SSID and Password.");
    Console.ForegroundColor = Color.White;
}


var mpu6886 = M5Core2.AccelerometerGyroscope;

M5Core2.TouchEvent += TouchEventCallback;

Console.ForegroundColor = Color.Green;
Console.WriteLine("Press the display or a button to get started!");
Console.ForegroundColor = Color.White;

Thread.Sleep(Timeout.Infinite);


void TouchEventCallback(object sender, TouchEventArgs e)
{
    Console.Clear();

    Debug.WriteLine($"Touch Panel Event Received Category= {e.EventCategory} Subcategory= {e.TouchEventCategory}");
    Console.CursorLeft = 0;
    Console.CursorTop = 0;

    var eventMsg = $"{StrTouchEvent}({e.X},{e.Y}){StrEventID}{e.Id}";
    Debug.WriteLine(eventMsg);
    Console.WriteLine(eventMsg);

    if ((e.TouchEventCategory & TouchEventCategory.LeftButton) == TouchEventCategory.LeftButton)
    {
        Vibrate(150);
        Debug.WriteLine(StrLB);
        Console.WriteLine(StrLB);
    }
    else if ((e.TouchEventCategory & TouchEventCategory.MiddleButton) == TouchEventCategory.MiddleButton)
    {
        Vibrate(150);
        Debug.WriteLine(StrMB);
        Console.WriteLine(StrMB);
    }
    else if ((e.TouchEventCategory & TouchEventCategory.RightButton) == TouchEventCategory.RightButton)
    {
        Vibrate(150);
        Debug.WriteLine(StrRB);
        Console.WriteLine(StrRB);
    }

    if ((e.TouchEventCategory & TouchEventCategory.Moving) == TouchEventCategory.Moving)
    {
        Debug.WriteLine($"{StrTouchEventCatagory}{StrMove}");
        Console.WriteLine($"{StrTouchEventCatagory}{StrMove}");
    }

    if ((e.TouchEventCategory & TouchEventCategory.LiftUp) == TouchEventCategory.LiftUp)
    {
        Debug.WriteLine($"{StrTouchEventCatagory}{StrLiftUp}");
        Console.WriteLine($"{StrTouchEventCatagory}{StrLiftUp}");
    }

    if ((e.TouchEventCategory & TouchEventCategory.DoubleTouch) == TouchEventCategory.DoubleTouch)
    {
        // Note: the M5Core2 does not support this event.
        Debug.WriteLine($"{StrTouchEventCatagory}{StrDoubleTouch}");
        Console.WriteLine($"{StrTouchEventCatagory}{StrDoubleTouch}");
    }

    Console.WriteLine("");
    Console.WriteLine("");

    Console.WriteLine($"{StrGyro}:");
    Console.WriteLine($"  ({mpu6886.GetGyroscope().X:N2},{mpu6886.GetGyroscope().Y:N2},{mpu6886.GetGyroscope().Z:N2})");

    Console.WriteLine($"{StrAcc}:");
    Console.WriteLine($"  ({mpu6886.GetAccelerometer().X:N2},{mpu6886.GetAccelerometer().Y:N2},{mpu6886.GetAccelerometer().Z:N2})");
    
    Debug.WriteLine($"{StrCpuT}{M5Core2.Power.GetInternalTemperature().DegreesCelsius}°C");
    //Note: we cannot use the proper symbol on the display with the default font!
    Console.WriteLine($"{StrCpuT}{M5Core2.Power.GetInternalTemperature().DegreesCelsius}_C");

}

void Vibrate(int milliSeconds)
{
    M5Core2.Vibrate = true;
    Thread.Sleep(milliSeconds);
    M5Core2.Vibrate = false;
}
