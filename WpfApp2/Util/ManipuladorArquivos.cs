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
    public class ManipuladorArquivos
    {
        public static string JsonSerializar(List<Peca> peca)
        {
            return JsonConvert.SerializeObject(peca);
        }

        public static List<Peca> JsonDesserializar(string Json)
        {
            return JsonConvert.DeserializeObject<List<Peca>>(Json);
        }

        public static void EscreverArquivo(List<Peca> pecasList)
        {
            using (StreamWriter sw = new StreamWriter(@"C:\dados\arquivo.json"))
            {
                sw.WriteLine(JsonSerializar(pecasList));
            }
        }

        public static List<Peca> LerArquivo()
        {
            List<Peca> peca;

            var strJson = "";
            try
            {
                using (StreamReader sr = new StreamReader(@"C:\dados\arquivo.json"))
                {
                    strJson = sr.ReadToEnd();
                    peca = JsonDesserializar(strJson);
                }
                return peca;
            }
            catch
            {
                return null;
            }
        }

    }
}
