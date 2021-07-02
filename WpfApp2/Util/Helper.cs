using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using WpfApp2.Model;

namespace WpfApp2.Util
{
    public class Helper
    {
        public static string JsonSerialize(List<Part> part)
        {
            return JsonConvert.SerializeObject(part);
        }

        public static List<Part> JsonDeserialize(string Json)
        {
            return JsonConvert.DeserializeObject<List<Part>>(Json);
        }

        public static void Serialize(List<Part> pecasList)
        {
            using (StreamWriter sw = new StreamWriter(@"C:\dados\arquivo.json"))
            {
                sw.WriteLine(JsonSerialize(pecasList));
            }
        }

        public static List<Part> Deserialize()
        {
            List<Part> part;

            var strJson = "";
            try
            {
                using (StreamReader sr = new StreamReader(@"C:\dados\arquivo.json"))
                {
                    strJson = sr.ReadToEnd();
                    part = JsonDeserialize(strJson);
                }
                return part;
            }
            catch
            {
                return null;
            }
        }

    }
}
