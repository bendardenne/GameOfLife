namespace GameOfLife.Models;

public delegate void UniverseChangedEvent();

/// <summary>
/// Model class which represents the space for our Game of Life.  
/// </summary>
public class Universe
{
    public event UniverseChangedEvent? UniverseChanged;

    private bool[,] _grid;
    private bool[,] _nextGrid;

    public bool[,] Grid
    {
        get => _grid;
        set
        {
            _grid = value;
            UniverseChanged?.Invoke();
        }
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="width">How many columns the game should have.</param>
    /// <param name="height">How many rows the game should have.</param>
    public Universe(int width, int height)
    {
        _grid = new bool[width, height];
        _nextGrid = new bool[width, height];
    }

    /// <summary>
    /// Perform one generation of the Game of Life.  
    /// </summary>
    public void PassTime()
    {
        for (int x = 0; x < _grid.GetLength(0); x++)
        {
            for (int y = 0; y < _grid.GetLength(1); y++)
            {
                _nextGrid[x, y] = NewState(x, y);
            }
        }

        (Grid, _nextGrid) = (_nextGrid, Grid);
    }

    private bool NewState(int x, int y)
    {
        var maxWidth = _grid.GetLength(0);
        var maxHeight = _grid.GetLength(1);

        var liveNeighbours = 0;
        for (var i = -1; i <= 1; i++)
        {
            for (var j = -1; j <= 1; j++)
            {
                // Dont count cell itself
                if (i == 0 && j == 0) continue;

                // Bounds check
                if (x + i < 0 || x + i >= maxWidth || y + j < 0 || y + j >= maxHeight) continue;

                liveNeighbours += _grid[x + i, y + j] ? 1 : 0;
            }
        }

        if (_grid[x, y])
        {
            //if cell is alive, it stays alive if 2 or 3 neighbours are alive
            return liveNeighbours is 2 or 3;
        }

        // dead cell becomes alive if exactly 3 neighbours are alive
        return liveNeighbours is 3;
    }
}