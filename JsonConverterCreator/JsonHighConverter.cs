using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using System.IO;

namespace WorldGenerate {
    public class JsonHighConverter {
        public Formatting FormatJson { get; set; }
        public bool ReadRootValueAsArray { get; set; }
        JsonSerializer serializer;

        public JsonHighConverter() {
            serializer = new JsonSerializer();
            ReadRootValueAsArray = false;
        }

        public JsonHighConverter(Formatting format) : this() {
            FormatJson = format;
        }

        public JsonHighConverter(Formatting format, bool ReadRootValueAsArray)
            : this(format){
            this.ReadRootValueAsArray = ReadRootValueAsArray;
        }

        public byte[] JsonToBson(string json) {
            using(var str = new MemoryStream()) {
                using (var writer = new BsonWriter(str)) {
                    serializer.Serialize(writer, JsonConvert.DeserializeObject(json));
                }
                return str.ToArray();
            }
        }

        public string BsonToJson(byte[] Bson) {
            using(var str = new MemoryStream(Bson)) {
                using(var reader = new BsonReader(str)) {
                    str.Seek(0, SeekOrigin.Begin);
                    reader.ReadRootValueAsArray = ReadRootValueAsArray;
                    return JsonConvert.SerializeObject(serializer.Deserialize(reader),FormatJson);
                }
            }
        }

        public string JsonToJson(string json) {
            //.....wat ( ͡° ͜ʖ ͡°)    this is bad. naprawdę nie ma szybszego sposobu na format?
            return JsonConvert.SerializeObject(JsonConvert.DeserializeObject(json),FormatJson);
            
        }
    }
}
