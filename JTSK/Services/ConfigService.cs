using JTSK.Models;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace JTSK.Services
{
    class ConfigService
    {
        string _path = Path.Combine(FileSystem.AppDataDirectory, "config.json");
        public ConfigModel Config { get; set; }
        public bool Exists { get; set; }
        public ConfigService()
        {
            if (File.Exists(_path))
            {
                Exists = true;
                string json;
                using (StreamReader sr = new StreamReader(_path))
                {
                    json = sr.ReadToEnd();
                }
                Config = JsonConvert.DeserializeObject<ConfigModel>(json);
            }
            else
            {
                Exists = false;
            }
        }
       

        public async Task Save(string email, string url)
        {
            if (!url.Contains("http://") &&s !url.Contains("https://"))
            {
                url = $"http://{url}";
            }
            Config = new ConfigModel
            {
                Email = email,
                Url = url
            };
            Exists = true;
            var json = JsonConvert.SerializeObject(Config);

            using (StreamWriter sw = new StreamWriter(_path))
            {
                await sw.WriteAsync(json);
            }
        }
    }
}
