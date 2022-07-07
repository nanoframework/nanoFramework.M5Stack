[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=nanoframework_lib-nanoFramework.M5Stack&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=nanoframework_lib-nanoFramework.M5Stack) [![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE) [![#yourfirstpr](https://img.shields.io/badge/first--timers--only-friendly-blue.svg)](https://github.com/nanoframework/Home/blob/main/CONTRIBUTING.md) [![Discord](https://img.shields.io/discord/478725473862549535.svg?logo=discord&logoColor=white&label=Discord&color=7289DA)](https://discord.gg/gCyBu8T)

![nanoFramework logo](https://raw.githubusercontent.com/nanoframework/Home/main/resources/logo/nanoFramework-repo-logo.png)

-----

# Welcome to the .NET **nanoFramework** M5Stack Libraries repository

## Build status

| Component | Build Status | NuGet Package |
|:-|---|---|
| nanoFramework.M5Core | [![Build Status](https://dev.azure.com/nanoframework/nanoFramework.M5Stack/_apis/build/status/nanoFramework.M5Stack?repoName=nanoframework%2FnanoFramework.M5Stack&branchName=main)](https://dev.azure.com/nanoframework/nanoFramework.M5Stack/_build/latest?definitionId=52&repoName=nanoframework%2FnanoFramework.M5Stack&branchName=main) | [![NuGet](https://img.shields.io/nuget/v/nanoFramework.M5Core.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.M5Stack/) |
| nanoFramework.M5Stick | [![Build Status](https://dev.azure.com/nanoframework/nanoFramework.M5Stack/_apis/build/status/nanoFramework.M5Stack?repoName=nanoframework%2FnanoFramework.M5Stack&branchName=main)](https://dev.azure.com/nanoframework/nanoFramework.M5Stack/_build/latest?definitionId=52&repoName=nanoframework%2FnanoFramework.M5Stack&branchName=main) | [![NuGet](https://img.shields.io/nuget/v/nanoFramework.M5StickC.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.M5StickC/) |
| nanoFramework.M5StickCPlus | [![Build Status](https://dev.azure.com/nanoframework/nanoFramework.M5Stack/_apis/build/status/nanoFramework.M5Stack?repoName=nanoframework%2FnanoFramework.M5Stack&branchName=main)](https://dev.azure.com/nanoframework/nanoFramework.M5Stack/_build/latest?definitionId=52&repoName=nanoframework%2FnanoFramework.M5Stack&branchName=main) | [![NuGet](https://img.shields.io/nuget/v/nanoFramework.M5StickCPlus.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.M5StickCPlus/) |
| nanoFramework.M5Core2 | [![Build Status](https://dev.azure.com/nanoframework/nanoFramework.M5Stack/_apis/build/status/nanoFramework.M5Stack?repoName=nanoframework%2FnanoFramework.M5Stack&branchName=main)](https://dev.azure.com/nanoframework/nanoFramework.M5Stack/_build/latest?definitionId=52&repoName=nanoframework%2FnanoFramework.M5Stack&branchName=main) | [![NuGet](https://img.shields.io/nuget/v/nanoFramework.M5Core2.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.M5Core2/) |
| nanoFramework.Fire | [![Build Status](https://dev.azure.com/nanoframework/nanoFramework.M5Stack/_apis/build/status/nanoFramework.M5Stack?repoName=nanoframework%2FnanoFramework.M5Stack&branchName=main)](https://dev.azure.com/nanoframework/nanoFramework.M5Stack/_build/latest?definitionId=52&repoName=nanoframework%2FnanoFramework.M5Stack&branchName=main) | [![NuGet](https://img.shields.io/nuget/v/nanoFramework.Fire.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.Fire/) |
| nanoFramework.AtomLite | [![Build Status](https://dev.azure.com/nanoframework/nanoFramework.M5Stack/_apis/build/status/nanoFramework.M5Stack?repoName=nanoframework%2FnanoFramework.M5Stack&branchName=main)](https://dev.azure.com/nanoframework/nanoFramework.M5Stack/_build/latest?definitionId=52&repoName=nanoframework%2FnanoFramework.M5Stack&branchName=main) | [![NuGet](https://img.shields.io/nuget/v/nanoFramework.AtomLite.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.AtomLite/) |
| nanoFramework.AtomMatrix | [![Build Status](https://dev.azure.com/nanoframework/nanoFramework.M5Stack/_apis/build/status/nanoFramework.M5Stack?repoName=nanoframework%2FnanoFramework.M5Stack&branchName=main)](https://dev.azure.com/nanoframework/nanoFramework.M5Stack/_build/latest?definitionId=52&repoName=nanoframework%2FnanoFramework.M5Stack&branchName=main) | [![NuGet](https://img.shields.io/nuget/v/nanoFramework.AtomMatrix.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.AtomMatrix/) |

## Usage

These NuGet packages provide a support for M5Stack products:

- [Core (gray) and Basic](https://docs.m5stack.com/en/products?id=core)
- [M5StickC](https://docs.m5stack.com/en/core/m5stickc)
- [M5StickCPlus](https://docs.m5stack.com/en/core/m5stickc_plus)
- [M5Core2](https://docs.m5stack.com/en/core/core2)
- [Fire](https://docs.m5stack.com/en/core/fire)
- [Atom Lite](https://docs.m5stack.com/en/core/atom_lite)
- [Atom Matrix](https://docs.m5stack.com/en/core/atom_matrix)

> Note 1: Before trying to add NuGet packages to your projects and/or before flashing the devices (see next section) using MS Visual Studio (VS), open VS > Tools > Options > NuGet Package Manager > Package Sources  and make sure that it contains an entry pointing to <https://api.nuget.org/v3/index.json>, otherwise add it.
> Note 2: When invoking VS > Project > Manage NuGet Packages make sure that in the Package source drop-down menu (right upper corner) "nuget.org" is selected.

The NuGets bring support for the screens as well and require to be flashed with the proper image (using [`nanoff`](https://github.com/nanoframework/nanoFirmwareFlasher) dotnet CLI).
On the examples below replace `COM3` with the appropriate number of the COM port to which your device is connected. (on Windows you can check this in the Device Manager).

For the M5Core:

```shell
nanoff --target M5Core --update --serialport COM3
```

For the M5StickC:

```shell
nanoff --target M5StickC --update --serialport COM3 --baud 115200
```

For the M5StickCPlus:

```shell
nanoff --target M5StickCPlus --update --serialport COM3
```

For the M5Core2 and Fire:

```shell
nanoff --target M5Core2 --update --serialport COM3
```

For the Atom Lite and Matrix:

```shell
nanoff --target ESP32_PICO --update --serialport COM3
```

> Note 3: If the `nanoff` commands fails, make sure you have followed instruction from Note 1 above.

Once you have the NuGets, you can then enjoy accessing the screen, the accelerometer, get a Grove I2C connecter, add events on the buttons. And you don't even need to think about anything, all is done for you in the most transparent way!

> Note 4: All the classes that you'll have access are all using the Lazy pattern to be instantiated including the screen. This have the advantage to use as little memory and setup time as possible.

In the samples below, we'll use either M5Core or M5Stick as examples, they are all working in a very similar way.

### Namespaces

Make sure you add the proper namespaces reference to your C# program header e.g.
using nanoFramework;

### Screen

The only thing you need to do to access the screen is to initialize it (please note that `InitializeScreen()` it's target specific) e.g.:

For Core:

```csharp
M5Core.InitializeScreen();
```

For StickCPlus:

```csharp
M5StickCPlus.InitializeScreen();
```

Once you've initialized it, you can access both a `Screen` static class and a `Console` static class.

THe `Screen` one brings primitives to write directly on the screen points or scare of colours as well as writing a text.

For example, you can write a blue square of 10x10 at the position 0, 0 like this:

```csharp
ushort[] toSend = new ushort[100];
ushort blue = ColorUtility.To16Bpp(Color.Blue);

for (int i = 0; i < toSend.Length; i++)
{
    toSend[i] = blue;
}

Screen.Write(0, 0, 10, 10, toSend);
```

The Console class works in a similar way as the classic `System.Console`. In order to use it you have to reference it using the fully qualified name of the methods, like this:

```csharp
nanoFramework.Console.Clear();

// Test the console display
nanoFramework.Console.Write("This is a short text. ");
nanoFramework.Console.ForegroundColor = nanoFramework.Presentation.Media.Color.Red;
nanoFramework.Console.WriteLine("This one displays in red after the previous one and is already at the next line.");
nanoFramework.Console.BackgroundColor = nanoFramework.Presentation.Media.Color.Yellow;
nanoFramework.Console.ForegroundColor = nanoFramework.Presentation.Media.Color.RoyalBlue;
nanoFramework.Console.WriteLine("And this is blue on yellow background");
nanoFramework.Console.ResetColor();
nanoFramework.Console.CursorLeft = 0;
nanoFramework.Console.CursorTop = 8;
nanoFramework.Console.Write("This is white on black again and on 9th line");
```

> Note: You can change the default font as well, you need to provide it as a property. The Cursor positions are calculated with the largest possible character.

M5Core2 and Fire have PSRAM, so you can get a full screen buffer as well. Refer to the [Graphics samples](https://github.com/nanoframework/Samples#graphics-for-screens) to understand all you can do with it.

If you have intensive graphic need with any of the M5Stack, you can adjust the memory requested. While both M5Core2 and Fire have PSRAM and can accommodate very large amount like 2 Mb or more, the ones without cannot go more than few Kb or tens of Kb.

```csharp
// This will allocate 2 Mb of memory for the graphics
M5Core2.InitializeScreen(2 * 1024 * 1024);
```

### Buttons

The main buttons except the power buttons are exposed.

On the M5Stack and Fire they are called `ButtonLeft`, `ButtonCenter` and `ButtonRight`. You can get access to the events as well. For example:

```csharp
M5Stack.ButtonLeft.Press += (sender, e) =>
{
    Console.ForegroundColor = Color.Yellow;
    Console.CursorLeft = 0;
    Console.CursorTop = 0;
    Console.Write($"Left Pressed  ");
};
```

On the M5StickC/CPlus they are called `ButtonM5` and `ButtonRight`. You can get access to the status of the button, the events and everything you need. For example:

```csharp
while (!M5StickC.ButtonRight.IsPressed)
{
    Thread.Sleep(10);
}

M5StickC.M5Button.IsHoldingEnabled = true;
M5StickC.M5Button.Holding += (sender, e) =>
{
    Console.Write("M5 button hold long.");
};
```

On the Atom Lite/Matrix it's called `Button`. You can get access to the status of the button, the events and everything you need. For example:

```csharp
AtomLite.Button.Press +=> (sender, e)
{
    var color = AtomLite.NeoPixel.GetColor();
    if(color.R > 0)
    {
        AtomLite.NeoPixel.SetColor(Color.FromArgb(255, 0, 255, 0));
    }
    else if (color.G > 0)
    {
        AtomLite.NeoPixel.SetColor(Color.FromArgb(255, 0, 0, 255));
    }
    else
    {
        AtomLite.NeoPixel.SetColor(Color.FromArgb(255, 255, 0, 0));
    }
};
```

> Note: The M5Core2 has touch screen and the buttons are "virtual"". See next section to see how to use them.

### M5Core2 touch panel and buttons

The touch panel comes with the screen. Both are initialized and activated at the same time. To get the touch events, you'll have to register to the `TouchEvent` event:

```csharp
M5Core2.InitializeScreen();
M5Core2.TouchEvent += TouchEventCallback;
```

Here is an example on how to check if you are on a button or not and get the various elements:

```csharp
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
```

The `TouchEventCategory` enum is a flag and can combine buttons and states. The buttons are mutually exclusive, so you can only have the Left, Middle or Right button, the states are `Moving` and `LiftUp`. `Moving` is happening when a contact has already been made and the touch point is moving. `LiftUp` will appear when the contact is released.

`DoubleTouch` is a specific that let you know there is another contact point happening. Each contact point will receive this flag. The event will be raised 2 times, one for each point. In a double touch context, you may not get the second point `LiftUp` event but you'll get the change with the disappearance of the DoubleTouch flag and the final `LiftUp` on the first point.

### Power management

The M5Core and M5StickC/CPlus are exposing their power management elements. It is not recommended to change any default value except if you know what you are doing.

Please refer to the detailed examples for the [AXP192](https://github.com/nanoframework/nanoFramework.IoT.Device/blob/develop/devices/Axp192/README.md) used in the M5StickC/CPlus; M5Core2 and [IP5306](https://github.com/nanoframework/nanoFramework.IoT.Device/blob/develop/devices/Ip5306/README.md) for the M5Core and Fire.

### Accelerometer and Gyroscope

You can get access to the Accelerometer and Gyroscope like this:

```csharp
var ag = M5Core.AccelerometerGyroscope;
// Do not move the M5Core/M5Stick during the calibration
ag.Calibrate(100);
var acc = ag.GetAccelerometer();
var gyr = ag.GetGyroscope();
Debug.WriteLine($"Accelerometer data x:{acc.X} y:{acc.Y} z:{acc.Z}");
Debug.WriteLine($"Gyroscope data x:{gyr.X} y:{gyr.Y} z:{gyr.Z}\n");
```

Refer to the [MPU6886 documentation](https://github.com/nanoframework/nanoFramework.IoT.Device/blob/develop/devices/Mpu6886/README.md) for more detailed usage on this sensor.

### Magnetometer

The M5Core has a magnetometer, you can access it as well:

```csharp
var mag = M5Core.Magnetometer;
// It is more than strongly recommended to calibrate the magnetometer.
// Move the M5Core in all directions to have a proper calibration.
mag.CalibrateMagnetometer(100);
var magVal = mag.ReadMagnetometer();
Console.WriteLine($"x={magVal.X:N2}   ");
Console.WriteLine($"y={magVal.Y:N2}   ");
Console.WriteLine($"Z={magVal.Z:N2}   ");
var headDir = Math.Atan2(magVal.X, magVal.Y) * 180.0 / Math.PI;
Console.WriteLine($"h={headDir:N2}  ");
```

### SerialPort

The M5Core and M5Core2 can provide a Serial Port, just get it like this:

```csharp
// You can access any of the Serial Port feature
M5Core.SerialPort.Open(115200);
// Do anything else you need
M5Core.SerialPort.Close();
```

Refer to the [SerialPort documentation](https://github.com/nanoframework/System.IO.Ports) for more information.

### ADC Channels

ADC Channels are pre setup on the M5Core, M5Core2, Fire and Atom Lite/Matrix, access them like this:

```csharp
// This will give you the ADC1 channel 7 which is on pin 35 of M5Core
AdcChannel myChannel = M5Core.GetAdcGpio(35);
```

Refer to the M5Stack documentation to have the mapping of the ADC channels and the pins.

## I2C Device/Grove

You can get an I2cDevice/Grove like this:

```csharp
// In this example, the I2C address of the device is 0x42:
I2cDevice myDevice = M5Core.GetGrove(0x42);
// replacing GetGrove by GetI2cDevice will have the same impact
```

### SPI Device

The M5Core, M5Core2, Fire and Atom Lite/Matrix provides as well an `SpiDevice`:

```csharp
// In this case GPIO5 will be used as chip select:
SpiDevice mySpi = M5Core.GetSpiDevice(5);
```

### GPIO Controller

Similar as previously, you can get the `GpioController` on any of the M5Core, M5Core2, Fire and M5StickC/CPlus:

```csharp
// This will open the GPIO 36 as output
var pin5 = M5StickC.GpioController.OpenPin(36, PinMode.Output);
```

### DAC

The M5Core, M5Core2, Fire and Atom Lite/Matrix exposes 2 DAC and you can access them thru the `Dac1` and `Dac2` properties. Refer to the [DAC documentation](https://github.com/nanoframework/System.Device.Dac) for more information.

### Led

The M5StickC/CPlus exposes a led. You can access it thru the `Led` property:

```csharp
// This will toggle the led:
M5StickC.Led.Toggle();
```

### Infrared Led

The M5StickC/CPlus and Atom Lite/Matrix exposes an infrared led. You can access it thru the `InfraredLed` property. This will give you a `TransmitterChannel`. Check out the [sample pack](https://github.com/nanoframework/Samples/tree/main/samples/Hardware.Esp32.Rmt) to understand how to use it.

### NeoPixel on AtomLite

The Atom Lite exposes a rgb led. You can access it thru the `NeoPixel` property:

```csharp
// This will set NeoPixel to green:
AtomLite.NeoPixel.Image.SetPixel(0, 0, Color.Green);
AtomLite.NeoPixel.Update();
```

### RGB LED matrix on AtomMatrix and Led bar on Fire

The Atom Matrix has a matrix of 25 RGB LEDs.
The position of the LEDs in the array follows their placement in the matrix, being 0 the one at the top left corner, growing left to right, top to bottom.

You can access it thru the `LedMatrix` property on the AtomMatrix, like this:

```csharp
// This will set the RGB LED at position 2, 2 to green
AtomMatrix.LedMatrix.Image.SetPixel(2, 2, Color.Green);
```

Similarly, you have access to the `LedBar` property on the Fire:

```csharp
// This will set the second RGB LED to green
Fire.LedBar.Image.SetPixel(2, 0, Color.Green);
```

After you're done with updating all the LEDs that you want to change, flush the updated to the LEDs, like this:

```csharp
// This will update all RGB LED 
AtomMatrix.LedMatrix.Update();
```

And on the Fire:

```csharp
// This will update all RGB LED 
Fire.LedBar.Update();
```

## Feedback and documentation

For documentation, providing feedback, issues and finding out how to contribute please refer to the [Home repo](https://github.com/nanoframework/Home).

Join our Discord community [here](https://discord.gg/gCyBu8T).

## Credits

The list of contributors to this project can be found at [CONTRIBUTORS](https://github.com/nanoframework/Home/blob/main/CONTRIBUTORS.md).

## License

The **nanoFramework** Class Libraries are licensed under the [MIT license](LICENSE.md).

## Code of Conduct

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behaviour in our community.
For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

### .NET Foundation

This project is supported by the [.NET Foundation](https://dotnetfoundation.org).
