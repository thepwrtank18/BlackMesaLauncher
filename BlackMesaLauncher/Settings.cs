using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BlackMesaLauncher
{
    public class Options
    {
        public bool WorkshopAddons { get; set; }
        public bool OldUI { get; set; }
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public string LaunchArgs { get; set; }
    }

    public class Settings
    {
        public static void Save(bool workshopAddons, bool oldUI, int screenWidth, int screenHeight, string launchArgs)
        {
            Options options = new Options()
            {
                WorkshopAddons = workshopAddons,
                OldUI = oldUI,
                ScreenWidth = screenWidth,
                ScreenHeight = screenHeight,
                LaunchArgs = launchArgs
            };

            string output = JsonConvert.SerializeObject(options);

            if (!File.Exists("./BMLOptions.json"))
            {
                File.Create("./BMLOptions.json").Dispose();
            }
            File.WriteAllText("./BMLOptions.json", output);
        }

        public static bool LoadWA()
        {
            Options deserialized = JsonConvert.DeserializeObject<Options>(File.ReadAllText("./BMLOptions.json"));
            return deserialized.WorkshopAddons;
        }

        public static bool LoadUI()
        {
            Options deserialized = JsonConvert.DeserializeObject<Options>(File.ReadAllText("./BMLOptions.json"));
            return deserialized.OldUI;
        }

        public static int LoadSW()
        {
            Options deserialized = JsonConvert.DeserializeObject<Options>(File.ReadAllText("./BMLOptions.json"));
            return deserialized.ScreenWidth;
        }

        public static int LoadSH()
        {
            Options deserialized = JsonConvert.DeserializeObject<Options>(File.ReadAllText("./BMLOptions.json"));
            return deserialized.ScreenHeight;
        }

        public static string LoadLA()
        {
            Options deserialized = JsonConvert.DeserializeObject<Options>(File.ReadAllText("./BMLOptions.json"));
            return deserialized.LaunchArgs;
        }
    }
}
