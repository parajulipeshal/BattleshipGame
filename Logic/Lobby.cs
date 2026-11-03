using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic
{             //REQ#2.3.1
    public static class Lobby
    {
        public static List<string> OpenGames { get; } = new();
        public static List<(string Name, Game game)> ActiveGames { get; } = new();
        public static event Action? LobbyChanged;             //REQ#3.1.1

        private static Dictionary<string, Game> AllGames { get; } = new();

        public static IStorage DataStore { get; set; } = new FileDataStore();
        public static AppData Data { get; private set; }

        static Lobby()
        {
            Data = DataStore.Load();
        }
                          
                    
        public static Game CreateGame(string name, string? creatorName = null)
        {
            if (OpenGames.Contains(name) || ActiveGames.Any(g => g.Name == name) || AllGames.ContainsKey(name))
                throw new InvalidOperationException("Game already exists.");

            Game game = new Game(name);
            string effectiveCreator = string.IsNullOrWhiteSpace(creatorName) ? "Player1" : creatorName!;
            game.Join(effectiveCreator);

            OpenGames.Add(name);
            AllGames[name] = game;

            if (!Data.CreatedGames.Contains(name))
                Data.CreatedGames.Add(name);

            DataStore.Save(Data);

            LobbyChanged?.Invoke();
            return game;
        }

        public static Game JoinGame(string name, string? playerName = null)
        {
            if (!AllGames.TryGetValue(name, out var game))
                throw new InvalidOperationException("Game not found.");

            if (game.IsFull)
                throw new InvalidOperationException("Game is already full.");

            string goodName = string.IsNullOrWhiteSpace(playerName) ? "Player2" : playerName!;
            Player player = game.Join(goodName);

            if (game.IsFull)
            {
                OpenGames.Remove(name);
                ActiveGames.Add((name, game));
                game.State = GameState.PlacingShips;
            }

            LobbyChanged?.Invoke();
            return game;
        }

        public static Game StartGame(string name)
        {
            if (!AllGames.TryGetValue(name, out var game))
                throw new InvalidOperationException("Game not found.");

            if (!OpenGames.Contains(name))
                throw new InvalidOperationException("Game is not open or already started.");

            OpenGames.Remove(name);
            ActiveGames.Add((name, game));

            LobbyChanged?.Invoke();
            return game;
        }

        public static Game? GetGame(string name)
        {
            return AllGames.TryGetValue(name, out var game) ? game : null;
        }

        public static List<(string PlayerName, int Score)> GetLeaderboard()
        {
            List<(string, int)> leaderboard = new List<(string, int)>();

            foreach (Game game in AllGames.Values)
            {
                if (game.Player1 != null)
                    leaderboard.Add((game.Player1.Name, game.Player1.Score));
                
                if (game.Player2 != null)
                    leaderboard.Add((game.Player2.Name, game.Player2.Score));
            }

            return leaderboard
                .OrderByDescending(s => s.Item2)
                .ThenBy(s => s.Item1)
                .ToList();
        }
        public static void UpdateScoreBoard()
        {
            var lb = GetLeaderboard();
            Data.ScoreBoard = lb.Select(t => (t.PlayerName, t.Score)).ToList();
            DataStore.Save(Data);
        }

        public static void NotifyLobbyChanged()
        {
            LobbyChanged?.Invoke();
        }
    }
}
