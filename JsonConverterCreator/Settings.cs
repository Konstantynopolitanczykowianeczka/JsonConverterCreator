using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace WorldGenerate {
    public class Settings {
        public static Settings settings { get; private set; }
        public bool ReadRootValueAsArray { get; set; }
        static public void GenerateSettings() {
            if (settings != null) return;
            //jsonMania ( ͡° ͜ʖ ͡°)
            if (!File.Exists("Settings.json")) {
                settings = new Settings();
                File.WriteAllText("Settings.json", JsonConvert.SerializeObject(
                    settings, Formatting.Indented));
            } else {
                settings = JsonConvert.DeserializeObject<Settings>(
                    File.ReadAllText("Settings.json"));
                
            }
        }
    }
}
