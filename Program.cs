using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Office.Interop.PowerPoint;
using Newtonsoft.Json;

//Thanks to CSharpFritz and EngstromJimmy for their gists, snippets, and thoughts.

namespace PowerPointToOBSSceneSwitcher
{
    class Program
    {
        private static Application ppt = new Microsoft.Office.Interop.PowerPoint.Application();
        private static ObsLocal OBS;
        static async Task Main(string[] args)
        {
            Console.Write("Connecting to PowerPoint...");
            ppt.SlideShowNextSlide += App_SlideShowNextSlide;
            Console.WriteLine("connected");

            Console.Write("Connecting to OBS...");
            OBS = new ObsLocal();
            await OBS.Connect();
            Console.WriteLine("connected");

            Console.ReadLine();
        }

        async static void App_SlideShowNextSlide(SlideShowWindow Wn)
        {
            if (Wn != null)
            {
                Console.WriteLine($"Moved to Slide Number {Wn.View.Slide.SlideNumber}");
                //Text starts at Index 2 ¯\_(ツ)_/¯
                var note = String.Empty;
                try
                {
                    note = Wn.View.Slide.CustomLayout.Name;
                }
                catch { /*no notes*/ }
                if (!String.IsNullOrEmpty(note))
                {
                  
                    Console.WriteLine($"  Switching to OBS Scene named \"{note}\"");
                    OBS.ChangeScene(note);
                }
            }
        }
    }
}