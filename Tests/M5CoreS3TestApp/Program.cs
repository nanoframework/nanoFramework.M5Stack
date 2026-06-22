// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.M5Stack;
using nanoFramework.UI;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

Debug.WriteLine("Hello from M5CoreS3!");

const int requestedNativeBufferSize = 320 * 240 * 4;
M5CoreS3.InitializeScreen(requestedNativeBufferSize);
Debug.WriteLine($"Requested buffer: {requestedNativeBufferSize} bytes ({requestedNativeBufferSize / 1024} KB)");
Debug.WriteLine($"Allocated buffer: {DisplayControl.MaximumBufferSize} bytes ({DisplayControl.MaximumBufferSize / 1024} KB)");

int screenWidth = 0;
int screenHeight = 0;
int stripeHeight = 0;
Bitmap fullScreen = null;
Bitmap stripeBuffer = null;
bool fullScreenDisabledAfterOom = false;

void ConfigureNativeBuffer()
{
    int newWidth = DisplayControl.ScreenWidth;
    int newHeight = DisplayControl.ScreenHeight;

    // Reuse existing buffer if dimensions haven't changed.
    if (newWidth == screenWidth && newHeight == screenHeight
        && (fullScreen != null || stripeBuffer != null))
    {
        return;
    }

    fullScreen?.Dispose();
    stripeBuffer?.Dispose();
    fullScreen = null;
    stripeBuffer = null;

    screenWidth = newWidth;
    screenHeight = newHeight;

    int bytesPerPixelNativeBitmap = 3;
    int nativeBitmapOverhead = 128;
    int maxUsableBuffer = (int)DisplayControl.MaximumBufferSize - nativeBitmapOverhead;
    int fullScreenBytes = screenWidth * screenHeight * bytesPerPixelNativeBitmap;

    if (!fullScreenDisabledAfterOom && maxUsableBuffer >= fullScreenBytes)
    {
        try
        {
            fullScreen = DisplayControl.FullScreen;
        }
        catch (OutOfMemoryException)
        {
            fullScreenDisabledAfterOom = true;
            Debug.WriteLine("DisplayControl.FullScreen allocation failed: OOM. Using stripe fallback only.");
        }
    }
    else if (!fullScreenDisabledAfterOom)
    {
        fullScreenDisabledAfterOom = true;
        Debug.WriteLine($"Skipping FullScreen: available {maxUsableBuffer} bytes < required {fullScreenBytes} bytes.");
    }

    if (fullScreen != null)
    {
        Debug.WriteLine($"Using full native buffer: {screenWidth}x{screenHeight}");
        return;
    }

    stripeHeight = maxUsableBuffer / (screenWidth * bytesPerPixelNativeBitmap);
    if (stripeHeight < 1)
    {
        stripeHeight = 1;
    }

    while (stripeHeight >= 1 && stripeBuffer == null)
    {
        try
        {
            stripeBuffer = new Bitmap(screenWidth, stripeHeight);
        }
        catch (OutOfMemoryException)
        {
            stripeHeight /= 2;
        }
    }

    if (stripeBuffer == null)
    {
        throw new OutOfMemoryException("Unable to allocate native stripe buffer.");
    }

    Debug.WriteLine($"Using stripe native buffer: {screenWidth}x{stripeHeight}");
}

void FillDisplay(Color color)
{
    if (fullScreen != null)
    {
        fullScreen.FillRectangle(0, 0, fullScreen.Width, fullScreen.Height, color);
        fullScreen.Flush();
        return;
    }

    int y = 0;
    while (y < screenHeight)
    {
        int heightToFlush = stripeHeight;
        if ((y + heightToFlush) > screenHeight)
        {
            heightToFlush = screenHeight - y;
        }

        stripeBuffer.FillRectangle(0, 0, screenWidth, stripeHeight, color);
        stripeBuffer.Flush(0, 0, screenWidth, heightToFlush, 0, y);
        y += heightToFlush;
    }
}

void DrawSolidBar(int x, int y, int width, int height, Color color)
{
    if (width <= 0 || height <= 0)
    {
        return;
    }

    if (fullScreen != null)
    {
        fullScreen.FillRectangle(x, y, width, height, color);
        fullScreen.Flush(x, y, width, height, x, y);
        return;
    }

    if (stripeBuffer != null)
    {
        int remaining = height;
        int destinationY = y;

        while (remaining > 0)
        {
            int chunkHeight = remaining > stripeHeight ? stripeHeight : remaining;
            stripeBuffer.FillRectangle(x, 0, width, chunkHeight, color);
            stripeBuffer.Flush(x, 0, width, chunkHeight, x, destinationY);

            destinationY += chunkHeight;
            remaining -= chunkHeight;
        }

        return;
    }

    Color[] row = new Color[width];
    for (int i = 0; i < width; i++)
    {
        row[i] = color;
    }

    for (int rowIndex = 0; rowIndex < height; rowIndex++)
    {
        DisplayControl.Write((ushort)x, (ushort)(y + rowIndex), (ushort)width, 1, row);
    }
}

string[] Glyph(char c)
{
    switch (c)
    {
        case 'T': return new[] { "11111", "00100", "00100", "00100", "00100", "00100", "00100" };
        case 'R': return new[] { "11110", "10001", "10001", "11110", "10100", "10010", "10001" };
        case 'B': return new[] { "11110", "10001", "10001", "11110", "10001", "10001", "11110" };
        case 'L': return new[] { "10000", "10000", "10000", "10000", "10000", "10000", "11111" };
        case 'P': return new[] { "11110", "10001", "10001", "11110", "10000", "10000", "10000" };
        case 'A': return new[] { "01110", "10001", "10001", "11111", "10001", "10001", "10001" };
        case 'S': return new[] { "01111", "10000", "10000", "01110", "00001", "00001", "11110" };
        case 'F': return new[] { "11111", "10000", "10000", "11110", "10000", "10000", "10000" };
        case 'I': return new[] { "11111", "00100", "00100", "00100", "00100", "00100", "11111" };
        case 'C': return new[] { "01111", "10000", "10000", "10000", "10000", "10000", "01111" };
        case 'D': return new[] { "11110", "10001", "10001", "10001", "10001", "10001", "11110" };
        case 'E': return new[] { "11111", "10000", "10000", "11110", "10000", "10000", "11111" };
        case 'G': return new[] { "01111", "10000", "10000", "10011", "10001", "10001", "01111" };
        case 'N': return new[] { "10001", "11001", "10101", "10011", "10001", "10001", "10001" };
        case 'O': return new[] { "01110", "10001", "10001", "10001", "10001", "10001", "01110" };
        case 'U': return new[] { "10001", "10001", "10001", "10001", "10001", "10001", "01110" };
        case 'W': return new[] { "10001", "10001", "10001", "10101", "10101", "10101", "01010" };
        case 'Y': return new[] { "10001", "10001", "01010", "00100", "00100", "00100", "00100" };
        default: return new[] { "00000", "00000", "00000", "00000", "00000", "00000", "00000" };
    }
}

void DrawChar(int x, int y, char c, Color color, int scale = 3)
{
    var glyph = Glyph(c);
    for (int rowIndex = 0; rowIndex < glyph.Length; rowIndex++)
    {
        string row = glyph[rowIndex];
        for (int colIndex = 0; colIndex < row.Length; colIndex++)
        {
            if (row[colIndex] != '1')
            {
                continue;
            }

            for (int ys = 0; ys < scale; ys++)
            {
                for (int xs = 0; xs < scale; xs++)
                {
                    int px = x + colIndex * scale + xs;
                    int py = y + rowIndex * scale + ys;
                    if (px >= 0 && py >= 0 && px < screenWidth && py < screenHeight)
                    {
                        DisplayControl.WritePoint((ushort)px, (ushort)py, color);
                    }
                }
            }
        }
    }
}

void DrawWord(int x, int y, string text, Color color, int scale = 2, int spacing = 2)
{
    int glyphWidth = 5 * scale;
    for (int i = 0; i < text.Length; i++)
    {
        DrawChar(x + i * (glyphWidth + spacing), y, text[i], color, scale);
    }
}

void DrawOrientationLegend(int border)
{
    int panelX = border + 6;
    int panelY = border + 6;
    int panelWidth = screenWidth - (2 * border) - 12;
    int panelHeight = screenHeight - (2 * border) - 12;

    if (panelWidth < 80 || panelHeight < 60)
    {
        return;
    }

    DrawSolidBar(panelX, panelY, panelWidth, panelHeight, Color.FromArgb(24, 24, 24));
    DrawSolidBar(panelX, panelY, panelWidth, 1, Color.Gray);
    DrawSolidBar(panelX, panelY + panelHeight - 1, panelWidth, 1, Color.Gray);
    DrawSolidBar(panelX, panelY, 1, panelHeight, Color.Gray);
    DrawSolidBar(panelX + panelWidth - 1, panelY, 1, panelHeight, Color.Gray);

    DrawWord(panelX + 4, panelY + 4, "COLORS", Color.White, 2);

    int lineY = panelY + 22;
    int lineGap = 17;
    int swatch = 10;

    void DrawLegendLine(int y, char side, Color expectedColor, Color textColor, string colorName)
    {
        DrawSolidBar(panelX + 4, y + 2, swatch, swatch, expectedColor);
        DrawChar(panelX + 20, y, side, textColor, 2);
        DrawWord(panelX + 34, y, colorName, Color.White, 2);
    }

    DrawLegendLine(lineY + 0 * lineGap, 'T', Color.Red, Color.White, "RED");
    DrawLegendLine(lineY + 1 * lineGap, 'R', Color.Green, Color.Black, "GREEN");
    DrawLegendLine(lineY + 2 * lineGap, 'B', Color.Blue, Color.White, "BLUE");
    DrawLegendLine(lineY + 3 * lineGap, 'L', Color.Yellow, Color.Black, "YELLOW");
}

void DrawOrientationTestFrame(DisplayOrientation orientation)
{
    FillDisplay(Color.Black);

    int border = Math.Min(screenWidth, screenHeight) / 10;
    if (border < 12)
    {
        border = 12;
    }

    Color green = Color.FromArgb(0, 255, 0);  // Explicit pure green RGB
    DrawSolidBar(0, 0, screenWidth, border, Color.Red);
    DrawSolidBar(screenWidth - border, 0, border, screenHeight, green);
    DrawSolidBar(0, screenHeight - border, screenWidth, border, Color.Blue);
    DrawSolidBar(0, 0, border, screenHeight, Color.Yellow);

    int letterWidth = 5 * 3;
    int letterHeight = 7 * 3;

    DrawChar((screenWidth - letterWidth) / 2, 2, 'T', Color.White);
    DrawChar(screenWidth - border + 2, (screenHeight - letterHeight) / 2, 'R', Color.Black);
    DrawChar((screenWidth - letterWidth) / 2, screenHeight - letterHeight - 2, 'B', Color.White);
    DrawChar(2, (screenHeight - letterHeight) / 2, 'L', Color.Black);

    // Display expected color names directly on each side.
    DrawWord((screenWidth / 2) - 12, border + 3, "RED", Color.White, 1);
    DrawWord(screenWidth - border + 2, (screenHeight / 2) - 8, "GREEN", Color.Black, 1, 1);
    DrawWord((screenWidth / 2) - 14, screenHeight - border + 3, "BLUE", Color.White, 1);
    DrawWord(2, (screenHeight / 2) - 8, "YELLOW", Color.Black, 1, 1);

    DrawOrientationLegend(border);

    string orientationLabel = orientation switch
    {
        DisplayOrientation.Portrait => "PORTRAIT",
        DisplayOrientation.Landscape180 => "LANDSCAPE",
        DisplayOrientation.Portrait180 => "PORTRAIT",
        _ => "LANDSCAPE",
    };
    DrawWord(border + 4, 2, orientationLabel, Color.White, 2);
}

ConfigureNativeBuffer();

// Show memory allocation info at startup
FillDisplay(Color.Black);
DrawWord(4, 4,   "MEMORY ALLOCATION", Color.Cyan, 1);
DrawWord(4, 18,  "REQ:", Color.White, 1);
DrawWord(36, 18, (requestedNativeBufferSize / 1024).ToString(), Color.Yellow, 1);
DrawWord(64, 18, "KB", Color.White, 1);
DrawWord(4, 30,  "GOT:", Color.White, 1);
DrawWord(36, 30, (DisplayControl.MaximumBufferSize / 1024).ToString(), DisplayControl.MaximumBufferSize >= requestedNativeBufferSize ? Color.Lime : Color.Red, 1);
DrawWord(64, 30, "KB", Color.White, 1);
Thread.Sleep(3000);

DisplayOrientation[] orientations = new[]
{
    DisplayOrientation.Landscape,
    DisplayOrientation.Portrait,
    DisplayOrientation.Landscape180,
    DisplayOrientation.Portrait180,
};

while (true)
{
    for (int i = 0; i < orientations.Length; i++)
    {
        DisplayOrientation orientation = orientations[i];
        bool changed = DisplayControl.ChangeOrientation(orientation);
        Debug.WriteLine($"Orientation {orientation}, changed={changed}, size={DisplayControl.ScreenWidth}x{DisplayControl.ScreenHeight}");

        ConfigureNativeBuffer(); // reallocates only if dimensions changed (portrait <-> landscape)
        DrawOrientationTestFrame(orientation);
        Thread.Sleep(1500);
    }
}
