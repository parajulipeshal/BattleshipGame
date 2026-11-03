using Logic;          //REQ#2.2.2           //REQ#2.1.1
public abstract class Ship : IShip
{
    public List<Cell> Cells { get; private set; } = new();
    public bool IsSunk => Cells.Any() && Cells.All(c => c.IsHit);
    public bool IsPlaced { get; set; } = false;
    public int Length { get; set; }

    public Ship(int Length)
    {
        this.Length = Length;
    }
    
    //REQ#1.8.3 (implementation) 
    public void AssignCellsToShip(IEnumerable<Cell> cells)
    {
        Cells = cells.ToList();
        IsPlaced = true;
    }
   
}
