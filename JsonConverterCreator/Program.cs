using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Bson;
using System.IO;
using WorldGenerate;

namespace JsonConverterGenerator {

    public class pos {
        public int x, y;
        public pos(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }

    public class Location {
        public string Name { get; protected set; }
        public pos Position { get; set; }
        public string Description { get; set; }

        public Location(pos Posi, string Name, string Desci) {
            Position = Posi;
            this.Name = Name;
            Description = Desci;
        }
    }

    class Program {
        class Settings {
            public bool ReadRootValueAsArray;
        }
        static Settings settings;

        static void InstallSettings() {
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

        static void Main(string[] args) {
            InstallSettings();
            if (args.Length > 0) {
                DropFile(args);
                return;
            }
            while (true) {
                Console.Clear();
                Console.WriteLine("1.Create format random [Location] beginlevel.json");
                Console.WriteLine("2.level.json <=reformat=> level.json");
                Console.WriteLine("3.json <=> .dat");
                Console.WriteLine("4.exit");
                int value = int.Parse(Console.ReadLine());
                switch (value) {
                    case 1:
                        New();
                        break;
                    case 2:
                        Reformat();
                        break;
                    case 3:
                        Convert();
                        break;
                    case 4:
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            }
        }
        static void New() {
            List<Location> array = new List<Location>();
            array.Add(new Location(new pos(0, 0), "0", "0"));
            array.Add(new Location(new pos(0, 0), "0", "0"));
            File.WriteAllText("level.json", JsonConvert.SerializeObject(array, Formatting.Indented));
        }

        static void Reformat() {
            Console.Write("To Indented format? [y/n]");
            string formattype = Console.ReadLine().ToLower();
            Console.Write("Name enter File: ");
            string entfile = Console.ReadLine();
            Console.Write("Name exit File: ");
            string extfile = Console.ReadLine();
            JsonHighConverter Convertus =  new JsonHighConverter();
            if (formattype == "y") Convertus.FormatJson = Formatting.Indented;
            else if (formattype == "n") Convertus.FormatJson = Formatting.None;
            else Environment.Exit(1);
            File.WriteAllText(extfile, Convertus.JsonToJson(File.ReadAllText(entfile)));
        }

        static void Convert() {
            Console.Write("Name enter File: ");
            string entfile = Console.ReadLine();
            Console.Write("Name exit File: ");
            string extfile = Console.ReadLine();
            JsonHighConverter Convertus = new JsonHighConverter(Formatting.Indented, true);
            if (entfile.Substring(entfile.Length - 5, 5).IndexOf(".json") != -1) {
                File.WriteAllBytes(extfile,
                        Convertus.JsonToBson(File.ReadAllText(entfile)));
            } else {
                File.WriteAllText(extfile,
                    Convertus.BsonToJson(File.ReadAllBytes(entfile)));
            }
        }

        static void DropFile(string[] args) {
            JsonHighConverter Convertus = new JsonHighConverter(Formatting.Indented,true);
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
