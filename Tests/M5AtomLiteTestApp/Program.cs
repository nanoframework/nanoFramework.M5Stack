using System;
using System.Diagnostics;
using System.Threading;
using nanoFramework.M5AtomLite;

namespace M5AtomLiteTestApp
{
    public class Program
    {
        public static void Main()
        {
            var button = M5AtomLite.Button;
            var rgb = M5AtomLite.NeoPixel;
            rgb.SetColor(new Color(255, 0, 0));

            button.Press += Button_Press;

            Debug.WriteLine("Hello from nanoFramework!");

            Thread.Sleep(Timeout.Infinite);

            // Browse our samples repository: https://github.com/nanoframework/samples
            // Check our documentation online: https://docs.nanoframework.net/
            // Join our lively Discord community: https://discord.gg/gCyBu8T
        }

        private static void Button_Press(object sender, EventArgs e)
        {
            var color = M5AtomLite.NeoPixel.GetColor();
            if(color.R > 0)
            {
                M5AtomLite.NeoPixel.SetColor(new Color(0, 255, 0));
            }
            else if (color.G > 0)
            {
                M5AtomLite.NeoPixel.SetColor(new Color(0, 0, 255));
            }
            else
            {
                M5AtomLite.NeoPixel.SetColor(new Color(255, 0, 0));
            }

        }
    }
}
