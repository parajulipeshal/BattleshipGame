namespace Logic
{
    public class Board
    {
        public int BoardSize { get; set; } = 10;
        public List<Cell> Cells { get; set; } = new();
        public List<Ship> Ships { get; set; } = new();
        public bool AllShipsPlaced => Ships.All(s => s.IsPlaced);
        public bool AllShipsSunk => Ships.All(s => s.IsSunk);

        public Board(int size = 10)
        {
            BoardSize = size;


            for (int x = 0; x < BoardSize; x++)
            {
                for (int y = 0; y < BoardSize; y++)
                {
                    Cells.Add(new Cell(x, y));
                }
            }
        }
        //REQ#1.1.3 (implementation)
        public bool CanPlaceShip(Ship ship, int startX, int startY, bool isVertical)
        {
            for (int i = 0; i < ship.Length; i++)
            {
                int x = isVertical ? startX : startX + i;
                int y = isVertical ? startY + i : startY;
                if (x < 0 || x >= BoardSize || y < 0 || y >= BoardSize)
                    return false;

                if (Cells.Any(c => c.X == x && c.Y == y && c.IsOccupied))
                    return false;
            }
            return true;
        }

        //REQ#1.2.3 (implementation)
        public void PlaceShip(Ship ship, int startX, int startY, bool isVertical)
        {
            System.Console.WriteLine("Placing ship...");//debugging
            if (!CanPlaceShip(ship, startX, startY, isVertical))
                throw new CantPlaceShip();

            List<Cell> shipCells = new List<Cell>();

            for (int i = 0; i < ship.Length; i++)
            {
                int x = isVertical ? startX : startX + i;
                int y = isVertical ? startY + i : startY;
                Cell cell = Cells.First(c => c.X == x && c.Y == y);

                cell.IsOccupied = true;
                cell.OccupyingShip = ship;

                shipCells.Add(cell);
            }

            ship.AssignCellsToShip(shipCells);
            Ships.Add(ship);
        }

        //REQ#1.3.3 (implementation)
        public MoveResult ReceiveAttack(int x, int y)

        {
            MoveResult result = new MoveResult();
            Cell? cell = Cells.FirstOrDefault(c => c.X == x && c.Y == y);

            if (cell == null || cell.IsHit)
            {
                result.Hit = false;
                return result;
            }

            cell.IsHit = true;

            if (!cell.IsOccupied)
            {
                result.Hit = false;
                return result;
            }
            result.Hit = true;
            Ship ship = cell.OccupyingShip!;
            result.SunkShip = ship.IsSunk;
            result.GameOver = Ships.All(s => s.IsSunk);
            return result;
        }

    }
}
