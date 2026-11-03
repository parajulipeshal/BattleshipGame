namespace Tests;

using Logic;

public class Test_Battleship
{
    //REQ#1.1.1
    [Fact]
    public void CanPlaceShip_OnBoard()
    {
        Board board = new Board();
        Ship ship1 = ShipFactory.CreateShip(3);
        Ship ship2 = ShipFactory.CreateShip(3);
        Ship ship3 = ShipFactory.CreateShip(2);
        Assert.True(board.CanPlaceShip(ship1, 0, 0, false)); //bound test
        Cell cell = board.Cells.First(c => c.X == 2 && c.Y == 3);
        cell.IsOccupied = true;
        Assert.True(board.CanPlaceShip(ship2, 5, 0, false)); //overlap testt
    }

    //REQ#1.1.2
    [Fact]
    public void FailingCanPlaceShip_OnBoard()
    {
        Board board = new Board();
        Ship ship1 = ShipFactory.CreateShip(3);

        // This should throw an exception when trying to place ship out of bounds
        var exception = Assert.Throws<CantPlaceShip>(() =>
            board.PlaceShip(ship1, 14, 4, false)
        );
        Assert.NotNull(exception);
    }

    //REQ#1.1.3
    [Fact]
    public void WorkingCanPlaceShip_OnBoard()
    {
        Board board = new Board();
        Ship ship1 = ShipFactory.CreateShip(3);
        Ship ship2 = ShipFactory.CreateShip(3);

        // Place first ship successfully
        board.PlaceShip(ship1, 0, 0, false);
        Assert.True(ship1.IsPlaced);

        // This should throw exception due to overlap
        var exception = Assert.Throws<CantPlaceShip>(() =>
            board.PlaceShip(ship2, 0, 0, false)
        );
        Assert.NotNull(exception);
    }

    // REQ#1.2.1
    [Fact]
    public void PassingPlaceShip_OnBoard()
    {
        Board board = new Board();
        Ship ship1 = ShipFactory.CreateShip(3);
        board.PlaceShip(ship1, 0, 0, false);
        Assert.True(ship1.IsPlaced);
    }

    //REQ#1.2.2
    [Fact]
    public void FailingPlaceShip_OnBoard()
    {
        Board board = new Board();
        Ship ship1 = ShipFactory.CreateShip(3);
        Ship ship2 = ShipFactory.CreateShip(3);
        board.PlaceShip(ship1, 0, 0, false);
        var exception = Assert.Throws<CantPlaceShip>(() =>
            board.PlaceShip(ship2, 0, 0, false)
        );
        Assert.NotNull(exception);
    }

    //REQ#1.3.1
    [Fact]
    public void Attack_HappyPath_ReturnsHit()
    {
        var board = new Board();
        var ship = ShipFactory.CreateShip(2);
        board.PlaceShip(ship, 0, 0, false);
        var result = board.ReceiveAttack(0, 0);
        Assert.True(result.Hit);
        Assert.False(result.SunkShip);
        Assert.False(result.GameOver);
    }

    //REQ#1.3.2
    [Fact]
    public void Attack_Fails_WhenOutOfBounds()
    {
        var board = new Board();

        var result = board.ReceiveAttack(99, 50);

        Assert.False(result.Hit);
        Assert.False(result.SunkShip);
        Assert.False(result.GameOver);
    }

    //REQ#1.4.1
    [Fact]
    public void TestOfStore()
    {
        var store = new FileDataStore();
        var data = new AppData();
        data.CreatedGames.Add("Game1");
        data.CreatedGames.Add("Game2");
        store.Save(data);
        var loaded = store.Load();
        Assert.Equal(2, loaded.CreatedGames.Count);
        Assert.Contains("Game1", loaded.CreatedGames);
        Assert.Contains("Game2", loaded.CreatedGames);
    }

    //REQ#1.4.2
    [Fact]
    public void TestOfStore2()
    {
        var store = new FileDataStore();
        var data = new AppData();
        data.CreatedGames.Add("Game1");
        data.CreatedGames.Add("Game2");
        store.Save(data);
        var loaded = store.Load();
        Assert.Equal(2, loaded.CreatedGames.Count);
        Assert.Contains("Game1", loaded.CreatedGames);
        Assert.Contains("Game2", loaded.CreatedGames);
        Assert.DoesNotContain("Game3", loaded.CreatedGames);
    }

    //REQ#1.5.1
    [Fact]
    public void Fire_Hit_PlayerKeepsTurn()
    {
        var game = new Game("testgame");
        var p1 = game.Join("P1");
        var p2 = game.Join("P2");
        var ship = ShipFactory.CreateShip(2);
        p2.Board.PlaceShip(ship, 0, 0, false);
        game.CurrentTurn = p1;
        var result = game.Fire(p1, 0, 0);
        Assert.True(result.Hit);
        Assert.Equal(p1, game.CurrentTurn);
    }

    // REQ#1.5.2
    [Fact]
    public void Fire_Miss_SwitchesTurn()
    {
        var game = new Game("testgame");
        var p1 = game.Join("P1");
        var p2 = game.Join("P2");
        game.CurrentTurn = p1;
        var result = game.Fire(p1, 5, 5);
        Assert.False(result.Hit);
        Assert.Equal(p2, game.CurrentTurn);
    }

    // REQ#1.6.1
    [Fact]
    public void Game_GetPlayer_Test()
    {
        var game = new Game("test");
        var p1 = game.Join("peshal1");
        var p2 = game.Join("peshal2");

        var one = game.GetPlayer("peshal1");
        Assert.Equal(p1, one);
    }

    // REQ#1.6.2
    [Fact]
    public void Game_GetPlayer_Test_Error()
    {
        var game = new Game("test");
        var p1 = game.Join("peshal1");
        var p2 = game.Join("peshal2");

        var bring = game.GetPlayer("peshal3");
        Assert.Null(bring);
    }

    // REQ#1.7.1
    [Fact]
    public void Game_GetOpponent_Test()
    {
        var game = new Game("test");
        var p1 = game.Join("P1");
        var p2 = game.Join("P2");

        Assert.Equal(p2, game.GetOpponent(p1));
        Assert.Equal(p1, game.GetOpponent(p2));

    }
    // REQ#1.7.2
    [Fact]
    public void Game_GetOpponent_Test_Error()
    {
        var game = new Game("test");
        var p1 = game.Join("P1");

        Assert.Null(game.GetOpponent(p1));

    }

    // REQ#1.8.1 
    [Fact]
    public void AssignCells()
    {
        var ship = ShipFactory.CreateShip(3);
        var cells = new List<Cell>
        {
            new Cell(0, 0),
            new Cell(1, 0),
            new Cell(2, 0)
        };

        ship.AssignCellsToShip(cells);
        Assert.Equal(3, ship.Cells.Count);
        Assert.True(ship.IsPlaced);
    }

    //REQ#1.8.2 
    [Fact]
    public void AssignCellsEmpty()
    {
        var ship = ShipFactory.CreateShip(3);
        var emptyCells = new List<Cell>();
        ship.AssignCellsToShip(emptyCells);
        Assert.Empty(ship.Cells);
        Assert.True(ship.IsPlaced);
    }

    //REQ#1.9.1 
    [Fact]
    public void Test_MemoryDataStore()
    {
        var store = new MemoryDataStore();
        var data = new AppData();
        data.CreatedGames.Add("Game1");
        data.ScoreBoard.Add(("Peshal", 10));
        store.Save(data);
        var loaded = store.Load();
        Assert.Contains(("Peshal", 10), loaded.ScoreBoard);
    }
    //REQ#1.9.2 
    [Fact]
    public void Test_MemoryFailure()
    {
        var store = new MemoryDataStore();
        var data = new AppData();
        data.CreatedGames.Add("Game2");
        data.ScoreBoard.Add(("Peshal", 10));
        store.Save(data);
        var loaded = store.Load();
        Assert.DoesNotContain(("NotPeshal", 11), loaded.ScoreBoard);
    }

}
