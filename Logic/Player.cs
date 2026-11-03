using Logic;

public class Player
{
    public string Name { get; set; } = "";
    public Board Board { get; set; } = new Board();

    public List<int> ShipsToPlace { get; } = new() { 2, 3 };

    public int ShipIndex { get; set; } = 0;

    public bool AllShipsPlaced => ShipIndex >= ShipsToPlace.Count;

    public bool MyTurn { get; set; }

    public int Score { get; set; } = 0;

    public Player(string name)
    {
        Name = name;
    }
}
