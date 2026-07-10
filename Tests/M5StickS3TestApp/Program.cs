// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.M5Stack;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using UnitsNet;
using Iot.Device.M5Pm1;

Debug.WriteLine("Hello M5StickS3 from nanoFramework!");

int pmicDeviceId = M5StickS3.Power.GetDeviceId();
Debug.WriteLine($"M5PM1 device ID: 0x{pmicDeviceId:X4} (expected 0x{M5Pm1.DeviceId:X4}).");
Debug.WriteLine("M5PM1 initialized. Reporting power telemetry...");

M5StickS3.InitializeScreen();

ushort[] buffer = new ushort[100];
FillBuffer(buffer, Color.Red.ToBgr565());
Screen.Write(0, 0, 10, 10, buffer);

FillBuffer(buffer, Color.Green.ToBgr565());
Screen.Write((ushort)(135 - 10), 0, 10, 10, buffer);

FillBuffer(buffer, Color.Blue.ToBgr565());
Screen.Write(0, (ushort)(240 - 10), 10, 10, buffer);

FillBuffer(buffer, Color.White.ToBgr565());
Screen.Write((ushort)(135 - 10), (ushort)(240 - 10), 10, 10, buffer);

bool screenEnabled = true;
M5StickS3.ButtonA.Press += (s, e) =>
{
    screenEnabled = !screenEnabled;
    Screen.Enabled = screenEnabled;
    Debug.WriteLine($"Button A pressed, screen enabled: {screenEnabled}");
};

M5StickS3.ButtonB.Press += (s, e) =>
{
    M5StickS3.External5VEnabled = !M5StickS3.External5VEnabled;
    Debug.WriteLine($"Button B pressed, ext 5V enabled: {M5StickS3.External5VEnabled}");
};

while (true)
{
    var accel = M5StickS3.AccelerometerGyroscope.GetAccelerometer();
    var gyro = M5StickS3.AccelerometerGyroscope.GetGyroscope();
    bool isCharging = M5StickS3.IsCharging;
    ElectricPotential batteryVoltage = M5StickS3.BatteryVoltage;
    ElectricPotential vbusVoltage = M5StickS3.VBusVoltage;
    ElectricPotential outputVoltage = M5StickS3.Power.GetOutputVoltage();
    PowerSource powerSource = M5StickS3.Power.GetPowerSource();
    bool gpio0 = (bool)M5StickS3.Power.ReadGpio(Pin.Gpio0);
    int batteryMv = ToMillivolts(batteryVoltage);
    int vbusMv = ToMillivolts(vbusVoltage);
    int out5vMv = ToMillivolts(outputVoltage);
    int batteryPercent = EstimateBatteryPercent(batteryMv, isCharging, vbusMv);
    string batteryStatusText = batteryPercent >= 0
        ? $"BAT {batteryPercent,3}% CHG{(isCharging ? "ON" : "OFF")}"
        : "BAT CHARGING";
    string batteryVoltageText = $"BATV {batteryMv,4}MV";

    Debug.WriteLine($"Battery : {batteryMv} mV");
    Debug.WriteLine($"VBUS    : {vbusMv} mV");
    Debug.WriteLine($"5V out  : {out5vMv} mV");
    Debug.WriteLine($"Source  : {(int)powerSource}");
    Debug.WriteLine($"Charging: {isCharging}");
    Debug.WriteLine($"GPIO0   : {gpio0}");
    Debug.WriteLine($"Ext5V   : {M5StickS3.External5VEnabled}");
    Debug.WriteLine($"Accel=({accel.X:N2},{accel.Y:N2},{accel.Z:N2})g");
    Debug.WriteLine($"Gyro =({gyro.X:N2},{gyro.Y:N2},{gyro.Z:N2}) dps");

    const int uiScale = 1;
    DrawTelemetryLine(12, FitTelemetryText(batteryStatusText), Color.Yellow, Color.Black, uiScale);
    DrawTelemetryLine(22, FitTelemetryText(batteryVoltageText), Color.Yellow, Color.Black, uiScale);
    DrawTelemetryLine(32, FitTelemetryText($"AX {FormatSigned(accel.X)}"), Color.Lime, Color.Black, uiScale);
    DrawTelemetryLine(42, FitTelemetryText($"AY {FormatSigned(accel.Y)}"), Color.Lime, Color.Black, uiScale);
    DrawTelemetryLine(52, FitTelemetryText($"AZ {FormatSigned(accel.Z)}"), Color.Lime, Color.Black, uiScale);
    DrawTelemetryLine(62, FitTelemetryText($"GX {FormatSigned(gyro.X)}"), Color.Cyan, Color.Black, uiScale);
    DrawTelemetryLine(72, FitTelemetryText($"GY {FormatSigned(gyro.Y)}"), Color.Cyan, Color.Black, uiScale);
    DrawTelemetryLine(82, FitTelemetryText($"GZ {FormatSigned(gyro.Z)}"), Color.Cyan, Color.Black, uiScale);

    Thread.Sleep(1000);
}

static void FillBuffer(ushort[] buffer, ushort color)
{
    for (int i = 0; i < buffer.Length; i++)
    {
        buffer[i] = color;
    }
}

static int EstimateBatteryPercent(int millivolts, bool isCharging, int vbusMillivolts)
{
    const int emptyMv = 3000;
    const int fullMv = 4200;

    // When USB power is present and charging is active, some PMIC states can report
    // very low battery voltage transiently. Report "charging" instead of a fake 0%.
    if (isCharging && vbusMillivolts > 4300 && millivolts < emptyMv)
    {
        return -1;
    }

    if (millivolts <= emptyMv)
    {
        return isCharging ? 1 : 0;
    }

    if (millivolts >= fullMv)
    {
        return 100;
    }

    return (millivolts - emptyMv) * 100 / (fullMv - emptyMv);
}

static int ToMillivolts(ElectricPotential voltage)
{
    int millivolts = (int)voltage.Millivolts;
    if (millivolts > 0)
    {
        return millivolts;
    }

    return (int)(voltage.Volts * 1000.0);
}

static string FormatSigned(double value)
{
    int scaled = (int)(value * 100);
    int absScaled = scaled < 0 ? -scaled : scaled;
    int integer = absScaled / 100;
    int fraction = absScaled % 100;
    string sign = scaled >= 0 ? "+" : "-";
    return $"{sign}{integer}.{fraction:D2}";
}

static string FitTelemetryText(string text)
{
    const int maxChars = 14;

    if (text.Length <= maxChars)
    {
        return text;
    }

    return text.Substring(0, maxChars);
}

static void DrawTelemetryLine(ushort y, string text, Color foreground, Color background, int scale)
{
    const int width = 135;
    int height = (7 * scale) + 1;
    int charWidth = (5 * scale) + 1;

    ushort fg = foreground.ToBgr565();
    ushort bg = background.ToBgr565();
    ushort[] lineBuffer = new ushort[width * height];

    for (int i = 0; i < lineBuffer.Length; i++)
    {
        lineBuffer[i] = bg;
    }

    string upper = text.ToUpper();
    int maxChars = width / charWidth;
    if (upper.Length > maxChars)
    {
        upper = upper.Substring(0, maxChars);
    }

    for (int c = 0; c < upper.Length; c++)
    {
        byte[] glyph = GetGlyph(upper[c]);
        int xBase = c * charWidth;
        for (int row = 0; row < 7; row++)
        {
            byte bits = glyph[row];
            for (int col = 0; col < 5; col++)
            {
                if ((bits & (1 << (4 - col))) != 0)
                {
                    int x = xBase + (col * scale);
                    int yBase = row * scale;
                    for (int sy = 0; sy < scale; sy++)
                    {
                        for (int sx = 0; sx < scale; sx++)
                        {
                            int px = x + sx;
                            int py = yBase + sy;
                            int index = py * width + px;
                            if (index >= 0 && index < lineBuffer.Length)
                            {
                                lineBuffer[index] = fg;
                            }
                        }
                    }
                }
            }
        }
    }

    Screen.Write(0, y, width, (ushort)height, lineBuffer);
}

static byte[] GetGlyph(char c)
{
    switch (c)
    {
        case 'A': return new byte[] { 0x0E, 0x11, 0x11, 0x1F, 0x11, 0x11, 0x11 };
        case 'B': return new byte[] { 0x1E, 0x11, 0x11, 0x1E, 0x11, 0x11, 0x1E };
        case 'C': return new byte[] { 0x0E, 0x11, 0x10, 0x10, 0x10, 0x11, 0x0E };
        case 'D': return new byte[] { 0x1E, 0x11, 0x11, 0x11, 0x11, 0x11, 0x1E };
        case 'E': return new byte[] { 0x1F, 0x10, 0x10, 0x1E, 0x10, 0x10, 0x1F };
        case 'F': return new byte[] { 0x1F, 0x10, 0x10, 0x1E, 0x10, 0x10, 0x10 };
        case 'G': return new byte[] { 0x0E, 0x11, 0x10, 0x13, 0x11, 0x11, 0x0E };
        case 'H': return new byte[] { 0x11, 0x11, 0x11, 0x1F, 0x11, 0x11, 0x11 };
        case 'I': return new byte[] { 0x0E, 0x04, 0x04, 0x04, 0x04, 0x04, 0x0E };
        case 'J': return new byte[] { 0x01, 0x01, 0x01, 0x01, 0x11, 0x11, 0x0E };
        case 'K': return new byte[] { 0x11, 0x12, 0x14, 0x18, 0x14, 0x12, 0x11 };
        case 'L': return new byte[] { 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x1F };
        case 'M': return new byte[] { 0x11, 0x1B, 0x15, 0x15, 0x11, 0x11, 0x11 };
        case 'N': return new byte[] { 0x11, 0x19, 0x15, 0x13, 0x11, 0x11, 0x11 };
        case 'O': return new byte[] { 0x0E, 0x11, 0x11, 0x11, 0x11, 0x11, 0x0E };
        case 'P': return new byte[] { 0x1E, 0x11, 0x11, 0x1E, 0x10, 0x10, 0x10 };
        case 'Q': return new byte[] { 0x0E, 0x11, 0x11, 0x11, 0x15, 0x12, 0x0D };
        case 'R': return new byte[] { 0x1E, 0x11, 0x11, 0x1E, 0x14, 0x12, 0x11 };
        case 'S': return new byte[] { 0x0F, 0x10, 0x10, 0x0E, 0x01, 0x01, 0x1E };
        case 'T': return new byte[] { 0x1F, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04 };
        case 'U': return new byte[] { 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x0E };
        case 'V': return new byte[] { 0x11, 0x11, 0x11, 0x11, 0x11, 0x0A, 0x04 };
        case 'W': return new byte[] { 0x11, 0x11, 0x11, 0x15, 0x15, 0x15, 0x0A };
        case 'X': return new byte[] { 0x11, 0x11, 0x0A, 0x04, 0x0A, 0x11, 0x11 };
        case 'Y': return new byte[] { 0x11, 0x11, 0x0A, 0x04, 0x04, 0x04, 0x04 };
        case 'Z': return new byte[] { 0x1F, 0x01, 0x02, 0x04, 0x08, 0x10, 0x1F };
        case '0': return new byte[] { 0x0E, 0x11, 0x13, 0x15, 0x19, 0x11, 0x0E };
        case '1': return new byte[] { 0x04, 0x0C, 0x04, 0x04, 0x04, 0x04, 0x0E };
        case '2': return new byte[] { 0x0E, 0x11, 0x01, 0x02, 0x04, 0x08, 0x1F };
        case '3': return new byte[] { 0x1E, 0x01, 0x01, 0x0E, 0x01, 0x01, 0x1E };
        case '4': return new byte[] { 0x02, 0x06, 0x0A, 0x12, 0x1F, 0x02, 0x02 };
        case '5': return new byte[] { 0x1F, 0x10, 0x10, 0x1E, 0x01, 0x01, 0x1E };
        case '6': return new byte[] { 0x0E, 0x10, 0x10, 0x1E, 0x11, 0x11, 0x0E };
        case '7': return new byte[] { 0x1F, 0x01, 0x02, 0x04, 0x08, 0x08, 0x08 };
        case '8': return new byte[] { 0x0E, 0x11, 0x11, 0x0E, 0x11, 0x11, 0x0E };
        case '9': return new byte[] { 0x0E, 0x11, 0x11, 0x0F, 0x01, 0x01, 0x0E };
        case ':': return new byte[] { 0x00, 0x04, 0x04, 0x00, 0x04, 0x04, 0x00 };
        case '.': return new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x04 };
        case '+': return new byte[] { 0x00, 0x04, 0x04, 0x1F, 0x04, 0x04, 0x00 };
        case '-': return new byte[] { 0x00, 0x00, 0x00, 0x1F, 0x00, 0x00, 0x00 };
        case '/': return new byte[] { 0x01, 0x02, 0x02, 0x04, 0x08, 0x08, 0x10 };
        case '%': return new byte[] { 0x19, 0x19, 0x02, 0x04, 0x08, 0x13, 0x13 };
        case ' ': return new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        default: return new byte[] { 0x1F, 0x11, 0x01, 0x02, 0x04, 0x00, 0x04 };
    }
}
