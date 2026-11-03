using System.Collections.Generic;

namespace Logic
{
    //REQ#1.9.3 (implementation) 
    public class MemoryDataStore : IStorage
    {
        private AppData _data = new();

        public AppData Load()
        {
            return new AppData
            {
                CreatedGames = new List<string>(_data.CreatedGames),
                ScoreBoard = new List<(string PlayerName, int Score)>(_data.ScoreBoard)
            };
        }

        public void Save(AppData data)
        {
            _data = new AppData
            {
                CreatedGames = new List<string>(data.CreatedGames),
                ScoreBoard = new List<(string PlayerName, int Score)>(data.ScoreBoard)
            };
        }
    }
}
