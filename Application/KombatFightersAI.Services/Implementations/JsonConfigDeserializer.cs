using KombatFightersAI.Domain.Configuration;
using KombatFightersAI.Services.Interfaces;
using Newtonsoft.Json;

namespace KombatFightersAI.Services.Implementations
{
    public class JsonConfigDeserializer : IJsonConfigDeserializer
    {
        private readonly string _configDirectory;

        public JsonConfigDeserializer(AppSettings appSettings)
        {
            _configDirectory = appSettings.ConfigDirectory;
        }

        public string LoadDataFromJsonFiles(string fileName)
        {
            var fileToSearch = Path.Combine(Directory.GetCurrentDirectory(), _configDirectory, fileName);

            if (File.Exists(fileToSearch))
            {
                try
                {
                    return File.ReadAllText(fileToSearch);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to open json file: ", ex.Message);
                    return string.Empty;
                }
            }

            return string.Empty;
        }

        public T? DeserializeJsonData<T>(string jsonContent)
        {
            if (string.IsNullOrEmpty(jsonContent))
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(jsonContent);
        }
    }
}
