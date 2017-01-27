using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace WorldGenerate {
    public class DropFile {
        public static void DropFiles(string[] args) {
            
            JsonHighConverter Convertus = new JsonHighConverter(Formatting.Indented, Settings
                .settings.ReadRootValueAsArray);
            foreach (string file in args) {
                //.json to .data
                if (file.Substring(file.Length - 5, 5).IndexOf(".json") != -1) {
                    File.WriteAllBytes(GetOnlyNameFile(file) + ".dat",
                        Convertus.JsonToBson(File.ReadAllText(file)));
                } else { //.data to .json
                    File.WriteAllText(GetOnlyNameFile(file) + ".json",
                    Convertus.BsonToJson(File.ReadAllBytes(file)));
                }
                Console.WriteLine(file + " end");
            }
        }
        static string GetOnlyNameFile(string filelocation) {
            string name = new FileInfo(filelocation).Name;
            int index = name.LastIndexOf('.');
            if (index != -1) name = name.Remove(index);
            return name;
        }
    }
}
