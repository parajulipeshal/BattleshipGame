namespace Logic;

public class Cell
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsOccupied { get; set; } = false;
    public bool IsHit { get; set; }
    public Ship? OccupyingShip { get; set; }
    
    public Cell(int x, int y, bool isOccupied=false)
    {
        X = x;
        Y = y;
        IsOccupied = isOccupied;
        IsHit = false;
    }
    public void PlaceShip()
    {
        IsOccupied = true;
    }
    public void Hit()
    {
        IsHit = true;
    }
     public override string ToString() =>
        $"{(IsOccupied ? "S" : "~")} {(IsHit ? "(Hit)" : "")}";
}
