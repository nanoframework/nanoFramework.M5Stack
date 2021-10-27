[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=nanoframework_lib-nanoFramework.M5Stack&metric=alert_status)](https://sonarcloud.io/dashboard?id=nanoframework_lib-nanoFramework.M5Stack) [![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=nanoframework_lib-nanoFramework.M5Stack&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=nanoframework_lib-nanoFramework.M5Stack) [![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE) [![NuGet](https://img.shields.io/nuget/dt/nanoFramework.M5Stack.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.M5Stack/) [![#yourfirstpr](https://img.shields.io/badge/first--timers--only-friendly-blue.svg)](https://github.com/nanoframework/Home/blob/main/CONTRIBUTING.md) [![Discord](https://img.shields.io/discord/478725473862549535.svg?logo=discord&logoColor=white&label=Discord&color=7289DA)](https://discord.gg/gCyBu8T)

![nanoFramework logo](https://raw.githubusercontent.com/nanoframework/Home/main/resources/logo/nanoFramework-repo-logo.png)

-----

### Welcome to the **nanoFramework** M5Stack Library repository!

## Build status

| Component | Build Status | NuGet Package |
|:-|---|---|
| nanoFramework.M5Stack | [![Build Status](https://dev.azure.com/nanoframework/nanoFramework.M5Stack/_apis/build/status/nanoFramework.M5Stack?repoName=nanoframework%2FnanoFramework.M5Stack&branchName=main)](https://dev.azure.com/nanoframework/nanoFramework.M5Stack/_build/latest?definitionId=52&repoName=nanoframework%2FnanoFramework.M5Stack&branchName=main) | [![NuGet](https://img.shields.io/nuget/v/nanoFramework.M5Stack.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.M5Stack/) |
| nanoFramework.M5Stack (preview) | [![Build Status](https://dev.azure.com/nanoframework/nanoFramework.M5Stack/_apis/build/status/nanoFramework.M5Stack?repoName=nanoframework%2FnanoFramework.M5Stack&branchName=develop)](https://dev.azure.com/nanoframework/nanoFramework.M5Stack/_build/latest?definitionId=52&repoName=nanoframework%2FnanoFramework.M5Stack&branchName=develop) | [![NuGet](https://img.shields.io/nuget/vpre/nanoFramework.M5Stack.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.M5Stack/) |
| nanoFramework.M5Stick | [![Build Status](https://dev.azure.com/nanoframework/nanoFramework.M5Stack/_apis/build/status/nanoFramework.M5Stack?repoName=nanoframework%2FnanoFramework.M5Stack&branchName=main)](https://dev.azure.com/nanoframework/nanoFramework.M5Stack/_build/latest?definitionId=52&repoName=nanoframework%2FnanoFramework.M5Stack&branchName=main) | [![NuGet](https://img.shields.io/nuget/v/nanoFramework.M5Stick.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.M5Stick/) |
| nanoFramework.M5Stick (preview) | [![Build Status](https://dev.azure.com/nanoframework/nanoFramework.M5Stack/_apis/build/status/nanoFramework.M5Stack?repoName=nanoframework%2FnanoFramework.M5Stack&branchName=develop)](https://dev.azure.com/nanoframework/nanoFramework.M5Stack/_build/latest?definitionId=52&repoName=nanoframework%2FnanoFramework.M5Stack&branchName=develop) | [![NuGet](https://img.shields.io/nuget/vpre/nanoFramework.M5Stick.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.M5Stick/) |
| nanoFramework.M5StickCPlus | [![Build Status](https://dev.azure.com/nanoframework/nanoFramework.M5Stack/_apis/build/status/nanoFramework.M5Stack?repoName=nanoframework%2FnanoFramework.M5Stack&branchName=main)](https://dev.azure.com/nanoframework/nanoFramework.M5Stack/_build/latest?definitionId=52&repoName=nanoframework%2FnanoFramework.M5Stack&branchName=main) | [![NuGet](https://img.shields.io/nuget/v/nanoFramework.M5StickCPlus.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.M5StickCPlus/) |
| nanoFramework.M5StickCPlus (preview) | [![Build Status](https://dev.azure.com/nanoframework/nanoFramework.M5Stack/_apis/build/status/nanoFramework.M5Stack?repoName=nanoframework%2FnanoFramework.M5Stack&branchName=develop)](https://dev.azure.com/nanoframework/nanoFramework.M5Stack/_build/latest?definitionId=52&repoName=nanoframework%2FnanoFramework.M5Stack&branchName=develop) | [![NuGet](https://img.shields.io/nuget/vpre/nanoFramework.M5StickCPlus.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/nanoFramework.M5StickCPlus/) |

## Usage

Those 3 nugets provide a support for the [M5Stack](https://docs.m5stack.com/en/products?id=core), [M5StickC](https://docs.m5stack.com/en/core/m5stickc) and [M5StickCPlus](https://docs.m5stack.com/en/core/m5stickc_plus).

They bring support for the screens as well and require to be flashed with the proper image:

```shell
# Replace the com port number by your COM port
# For the M5Stack:
nanoff --target M5Stack --update --preview --serialport COM3
# For the M5StickC:
nanoff --target M5StickC --update --preview --serialport COM3 --baud 115200
# For the M5StickCPlus:
nanoff --target M5StickCPlus --update --preview --serialport COM3 --baud 115200
```

Once you have the nugets, you can then enjoy accessing the screen, the accelerometer, get a Grove I2C connecter, add events on the buttons. And you don't even need to think about anything, all is done for you in the most transparent way!

> Note: All the classes that you'll have access are all using the Lazy pattern to be instantiated including the screen. This have the advantage to use as little memory and setup time as possible.

In the samples below, we'll use either M5Stack, either M5Stick as examples, they are all working in a very similar way.

### Screen

The only thing you need to do to access the screen is to initialize it:

```csharp
M5Stack.InitializeScreen();
```

Once you've initialized it, you can access both a `Screen` static class and a `Console` static class.

THe `Screen` one brings primitives to write directly on the screen points or scare of colors as well as writing a text.

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

The Console class works in a similar way as the classic `System.Console`:

```csharp
Console.Clear();

// Test the console display
Console.Write("This is a short text. ");
Console.ForegroundColor = Color.Red;
Console.WriteLine("This one displays in red after the previous one and is already at the next line.");
Console.BackgroundColor = Color.Yellow;
Console.ForegroundColor = Color.RoyalBlue;
Console.WriteLine("And this is blue on yellow background");
Console.ResetColor();
Console.CursorLeft = 0;
Console.CursorTop = 8;
Console.Write("This is white on black again and on 9th line");
```

> Note: You can change the default font as well, you need to provide it as a property. The Cursor positions are calculated with the largest possible character.

### Buttons

The main buttons except the power buttons are exposed.

On the M5Stack they are called `ButtonLeft`, `ButtonCenter` and `ButtonRight`. You can get access to the events as well. For example:

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

### Power management

Both M5Stack and M5StickC/CPlus are exposing their power management elements. It is not recommended to change any default value except if you know what you are doing.

Please refer to the detailed examples for the [AXP192](https://github.com/nanoframework/nanoFramework.IoT.Device/blob/develop/devices/Axp192/README.md) used in the M5StickC/CPlus and [IP5306](https://github.com/nanoframework/nanoFramework.IoT.Device/blob/develop/devices/Ip5306/README.md) for the M5Stack.

### Accelerometer and Gyroscope

You can get access to the Accelerometer and Gyroscope like this:

```csharp
var ag = M5Stack.AccelerometerGyroscope;
// Do not move the M5Stack/M5Stick during the calibration
ag.Calibrate(100);
var acc = ag.GetAccelerometer();
var gyr = ag.GetGyroscope();
Debug.WriteLine($"Accelerometer data x:{acc.X} y:{acc.Y} z:{acc.Z}");
Debug.WriteLine($"Gyroscope data x:{gyr.X} y:{gyr.Y} z:{gyr.Z}\n");
```

Refer to the [MPU6886 documentation](https://github.com/nanoframework/nanoFramework.IoT.Device/blob/develop/devices/Mpu6886/README.md) for more detailed usage on this sensor.

### Magnetometer

The M5Stack has a magnetometer, you can access it as well:

```csharp
var mag = M5Stack.Magnetometer;
// It is more than strongly recommended to calibrate the magnetometer.
// Move the M5Stack in all directions to have a proper calibration.
mag.CalibrateMagnetometer(100);
var magVal = mag.ReadMagnetometer();
Console.WriteLine($"x={magVal.X:N2}   ");
Console.WriteLine($"y={magVal.Y:N2}   ");
Console.WriteLine($"Z={magVal.Z:N2}   ");
var headDir = Math.Atan2(magVal.X, magVal.Y) * 180.0 / Math.PI;
Console.WriteLine($"h={headDir:N2}  ");
```

### SerialPort

The M5Stack can provide a Serial Port, just get it like this:

```csharp
// You can access any of the Serial Port feature
M5Stack.SerialPort.Open(115200);
// Do anything else you need
M5Stack.SerialPort.Close();
```

Refer to the [SerialPort documentation](https://github.com/nanoframework/System.IO.Ports) for more information.

### ADC Channels

ADC Channels are pre setup on the M5Stack, access them like this:

```csharp
// This will give you the ADC1 channel 7 which is on pin 35
AdcChannel myChannel = M5Stack.GetAdcGpio(35);
```

Refer to the M5Stack documentation to have the mapping of the ADC channels and the pins.

## I2C Device/Grove

You can get an I2cDevice/Grove like this:

```csharp
// In this example, the I2C address of the device is 0x42:
I2cDevice myDevice = M5Stack.GetGrove(0x42);
// replacing GetGrove by GetI2cDevice will have the same impact
```

### Spi Device

The M5Stack provides as well an SPiDevice:

```csharp
// In this case GPIO5 will be used as chip select:
SpiDevice mySpi = M5Stack.GetSpiDevice(5);
```

### GPIO Controller

Similar as previously, you can get the `GpioController` on any of the M5Stack and M5StickC/CPlus:

```csharp
// This will open the GPIO 36 as output
var pin5 = M5StickC.GpioController.OpenPin(36, PinMode.Output);
```

### DAC

The M5Stack exposes 2 DAC and you can access them thru the `Dac1` and `Dac2` properties. Refer to the [DAC documentation](https://github.com/nanoframework/System.Device.Dac) for more information.

### Led

The M5StickC/CPlus exposes a led. You can access it thru the `Led` property:

```csharp
// This will toggle the led:
M5StickC.Led.Toggle();
```

### Infrared Led

The M5StickC/CPlus exposes an infrared led. You can access it thru the `InfraredLed` property. This will give you a `TransmitterChannel`. Check out the [sample pack](https://github.com/nanoframework/Samples/tree/main/samples/Hardware.Esp32.Rmt) to understand how to use it.

## Feedback and documentation

For documentation, providing feedback, issues and finding out how to contribute please refer to the [Home repo](https://github.com/nanoframework/Home).

Join our Discord community [here](https://discord.gg/gCyBu8T).

## Credits

The list of contributors to this project can be found at [CONTRIBUTORS](https://github.com/nanoframework/Home/blob/main/CONTRIBUTORS.md).

## License

The **nanoFramework** Class Libraries are licensed under the [MIT license](LICENSE.md).

## Code of Conduct

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community.
For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

### .NET Foundation

This project is supported by the [.NET Foundation](https://dotnetfoundation.org).
