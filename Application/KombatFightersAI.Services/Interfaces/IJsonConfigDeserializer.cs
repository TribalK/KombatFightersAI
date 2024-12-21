namespace KombatFightersAI.Services.Interfaces
{
    public interface IJsonConfigDeserializer
    {
        public string LoadDataFromJsonFiles(string fileBasePattern);
        public T? DeserializeJsonData<T>(string jsonContent);
    }
}
