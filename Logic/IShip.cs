using Logic;             //REQ#2.2.1
public interface IShip
{
    List<Cell> Cells { get; }
    bool IsSunk { get; }
    bool IsPlaced { get; set; }
    int Length { get; set; }

    void AssignCellsToShip(IEnumerable<Cell> cells);
}
