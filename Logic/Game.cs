namespace Logic
{
    public class Game
    {
        public string Name { get; }
        public Player? Player1 { get; private set; }
        public Player? Player2 { get; private set; }
        public GameState State { get; set; } = GameState.WaitingForPlayers;
        public Player? CurrentTurn { get; set; }

        public bool IsFull => Player1 != null && Player2 != null;

        public Game(string name)
        {
            Name = name;
        }

        public Player Join(string playerName)
        {
            if (Player1 == null)
            {
                Player1 = new Player(playerName);
                return Player1;
            }
            else if (Player2 == null)
            {
                Player2 = new Player(playerName);
                return Player2;
            }
            else
            {
                throw new InvalidOperationException("Game is full.");
            }
        }

                // REQ#1.6.3 (implementation)
        public Player? GetPlayer(string name)
        {
            if (Player1?.Name.Equals(name, StringComparison.OrdinalIgnoreCase) == true)
                return Player1;

            if (Player2?.Name.Equals(name, StringComparison.OrdinalIgnoreCase) == true)
                return Player2;

            return null;
        }


                 // REQ#1.7.3 (implementation)
        public Player? GetOpponent(Player player)
        {
            if (Player1 == player) return Player2;
            if (Player2 == player) return Player1;
            return null;
        }
        

        public bool IsPlayersTurn(Player? p)
        {
            return CurrentTurn != null && p != null && CurrentTurn == p;
        }

        public void SwitchTurn()
        {
            if (Player1 == null && Player2 == null)
                return;

            if (CurrentTurn == null)
            {
                CurrentTurn = Player1 ?? Player2;
                return;
            }

            if (CurrentTurn == Player1)
                CurrentTurn = Player2 ?? Player1;
            else
                CurrentTurn = Player1 ?? Player2;
        }

    // REQ#1.5.3(implementation)
        public MoveResult Fire(Player attacker, int x, int y)
        {
            if (attacker == null)
                throw new ArgumentNullException(nameof(attacker));

            Player? opponent = GetOpponent(attacker);
            if (opponent == null)
                throw new InvalidOperationException("Opponent not found.");

            MoveResult result = opponent.Board.ReceiveAttack(x, y);

            if (result.SunkShip)
            {
                attacker.Score += 5;
                Lobby.UpdateScoreBoard();    //REQ#3.1.3
            }

            if (result.GameOver)
            {
                State = GameState.GameOver;
            }

            if (!result.Hit)
            {
                SwitchTurn();
            }

            Lobby.NotifyLobbyChanged();

            return result;
        }

    }
}