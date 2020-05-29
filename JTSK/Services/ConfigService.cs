using JTSK.Models;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace JTSK.Services
{
    class ConfigService
    {
        string _pathUser = Path.Combine(FileSystem.AppDataDirectory, "userconfig.json");
        public ConfigModel UserConfig { get; set; }
        public StaticConfig StaticConfig { get; set; }
        public bool Exists { get; set; }
        public ConfigService()
        {
            string json;
            if (File.Exists(_pathUser))
            {
                Exists = true;
                using (StreamReader sr = new StreamReader(_pathUser))
                {
                    json = sr.ReadToEnd();
                }
                UserConfig = JsonConvert.DeserializeObject<ConfigModel>(json);
            }
            else
            {
                Exists = false;
            }
            using (StreamReader sr = new StreamReader(this.GetType().Assembly.GetManifestResourceStream("JTSK.staticconfig.json")))
            {
                json = sr.ReadToEnd();
            }
            StaticConfig = JsonConvert.DeserializeObject<StaticConfig>(json);
        }


        public async Task Save(string email)
        {
            UserConfig = new ConfigModel
            {
                Email = email,
            };
            Exists = true;
            var json = JsonConvert.SerializeObject(UserConfig);

            using (StreamWriter sw = new StreamWriter(_pathUser))
            {
                await sw.WriteAsync(json);
            }
        }
    }
}
