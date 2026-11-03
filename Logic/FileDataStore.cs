using System.Text.Json;
//REQ#1.4.3 (implementation)
namespace Logic
{                    
    public class FileDataStore : IStorage
    {
        private const string FilePath = "appdata.json";

        public AppData Load()        //REQ#4.1.2
        {
            if (!File.Exists(FilePath))
                return new AppData();

            return JsonSerializer.Deserialize<AppData>(File.ReadAllText(FilePath))
                   ?? new AppData();
        }

        public void Save(AppData data)       //REQ#4.1.1
        {
            File.WriteAllText(
                FilePath,
                JsonSerializer.Serialize(data)
            );
        }
    }
}
