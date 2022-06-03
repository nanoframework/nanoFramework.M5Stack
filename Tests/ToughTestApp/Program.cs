// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.M5Stack;
using nanoFramework.Networking;
using nanoFramework.Tough;
using System.Diagnostics;
using System.Threading;
using Console = nanoFramework.M5Stack.Console;

Tough.InitializeScreen();

Debug.WriteLine("Hello from Tough!");

Console.WriteLine("Hello from Tough!");

const string Ssid = "SSID";
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
}

Tough.TouchEvent += TouchEventCallback;

Thread.Sleep(Timeout.Infinite);

void TouchEventCallback(object sender, TouchEventArgs e)
{
    const string StrXY1 = "TOUCHED at X= ";
    const string StrXY2 = ",Y= ";

    Console.CursorLeft = 0;
    Console.CursorTop = 0;

    Debug.WriteLine(StrXY1 + e.X + StrXY2 + e.Y);
    Console.WriteLine(StrXY1 + e.X + StrXY2 + e.Y + "  ");

    Console.WriteLine("                                      ");
}
