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
        /// <summary>
        /// fast write.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args) {
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

            JArray array = JsonConvert.DeserializeObject<JArray>(
                File.ReadAllText(entfile));
            if (formattype == "y") {
                File.WriteAllText(extfile,
                    JsonConvert.SerializeObject(array, Formatting.Indented));
            } else if (formattype == "n") {
                File.WriteAllText(extfile,
                    JsonConvert.SerializeObject(array, Formatting.None));
            } else {
                Environment.Exit(1);
            }
        }

        static void Convert() {
            Console.Write("Name enter File: ");
            string entfile = Console.ReadLine();
            Console.Write("Name exit File: ");
            string extfile = Console.ReadLine();
            if (entfile.Substring(entfile.Length - 5, 5).IndexOf(".json") != -1) {
                JArray array = JsonConvert.DeserializeObject<JArray>(
                    File.ReadAllText(entfile));
                using (var stream = new FileStream(extfile, FileMode.Create))
                using (BsonWriter writer = new BsonWriter(stream))
                    array.WriteTo(writer);
            } else {
                using (var stream = new FileStream(entfile, FileMode.Open)) {
                    JsonSerializer ser = new JsonSerializer();
                    BsonReader reader = new BsonReader(stream);
                    reader.ReadRootValueAsArray = true;
                    JArray array = ser.Deserialize<JArray>(reader);
                    File.WriteAllText(extfile, JsonConvert.SerializeObject(array));
                }
            }
        }

        static void DropFile(string[] args) {

            foreach (string file in args) {
                //.json to .data
                if (file.Substring(file.Length - 5, 5).IndexOf(".json") != -1) {
                    JArray array = JsonConvert.DeserializeObject<JArray>(
                    File.ReadAllText(file));
                    string resultfile = GetOnlyNameFile(file) + ".dat";
                    using (var stream = new FileStream(resultfile, FileMode.Create))
                    using (BsonWriter writer = new BsonWriter(stream))
                        array.WriteTo(writer);
                } else { //.data to .json
                    JArray array;
                    string resultfile = GetOnlyNameFile(file) + ".json";
                    using (var stream = new FileStream(file, FileMode.Open))
                    using (BsonReader reader = new BsonReader(stream)) {
                        reader.ReadRootValueAsArray = true;
                        JsonSerializer serial = new JsonSerializer();
                        array = serial.Deserialize<JArray>(reader);
                        File.WriteAllText(resultfile,JsonConvert.SerializeObject(array,Formatting.Indented));
                    }
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
