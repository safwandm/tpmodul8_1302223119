using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace tpmodul8_1302223119
{
    public class Config
    {
        public string satuan_suhu { get; set; }
        public int batas_hari_demam { get; set; }
        public string pesan_ditolak { get; set; }
        public string pesan_diterima { get; set; }

        public Config(string satuan_suhu, int batas_hari_demam, string pesan_ditolak, string pesan_diterima)
        {
            this.satuan_suhu = satuan_suhu;
            this.batas_hari_demam = batas_hari_demam;
            this.pesan_ditolak = pesan_ditolak;
            this.pesan_diterima = pesan_diterima;
        }
    }

    public class CovidConfig 
    {
        public Config config { get; set; }
        string filePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "/covid_config.json";

        public CovidConfig() 
        {
            try 
            {
                string json = File.ReadAllText(filePath);
                config = JsonSerializer.Deserialize<Config>(json);
                if (config.satuan_suhu != "celcius" || config.satuan_suhu != "fahrenheit")
                {
                    throw new ArgumentException();
                }
            }
            catch 
            {
                config = new Config("celcius", 14, "Anda tidak diperbolehkan masuk ke dalam gedung ini", "Anda dipersilahkan untuk masuk ke dalam gedung ini");

                JsonSerializerOptions options = new JsonSerializerOptions()
                {
                    WriteIndented = true
                };

                String jsonString = JsonSerializer.Serialize(config, options);
                File.WriteAllText(filePath, jsonString);
            }
        }

        public void checkSuhu()
        {

            bool valid = true;

            Console.Write("Berapa suhu badan anda saat ini? Dalam " + config.satuan_suhu + ": ");
            double satSuhu = double.Parse(Console.ReadLine());

            Console.Write("Berapa hari yang lalu (perkiraan) anda terakhir memiliki gejala demam?: ");
            int batasHariDemam = int.Parse(Console.ReadLine());

            if (config.satuan_suhu == "celcius")
            {
                if (satSuhu < 36.5 || satSuhu > 37.5)
                {
                    valid =  false;
                }
            }
            else if (config.satuan_suhu == "fahrenheit")
            {
                if (satSuhu < 97.7 || satSuhu > 99.5)
                {
                    valid = false;
                }
            }

            if (batasHariDemam >= config.batas_hari_demam)
            {
                valid = false;
            }

            if (valid)
            {
                Console.WriteLine(config.pesan_diterima);
            }
            else
            {
                Console.WriteLine(config.pesan_ditolak);
            }
        }
        public void UbahSatuan()
        {
            config = new Config(config.satuan_suhu == "celcius" ? "fahrenheit" : "celcius", 14, "Anda tidak diperbolehkan masuk ke dalam gedung ini", "Anda dipersilahkan untuk masuk ke dalam gedung ini");

            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            String jsonString = JsonSerializer.Serialize(config, options);
            File.WriteAllText(filePath, jsonString);

        }


    }

}
