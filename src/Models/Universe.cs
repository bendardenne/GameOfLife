using System;
using Splat;

namespace GameOfLife.Models;

public delegate void UniverseChangedEvent();

/// <summary>
/// Model class which represents the space for our Game of Life.  
/// </summary>
public class Universe : IEnableLogger
{
    private double _filled;
    private bool[,] _grid;
    private bool[,] _nextGrid;
    private Random _random;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="width">How many columns the game should have.</param>
    /// <param name="height">How many rows the game should have.</param>
    public Universe(int width, int height)
    {
        _grid = new bool[width, height];
        _nextGrid = new bool[width, height];
        _random = new Random();
    }

    public bool[,] Grid
    {
        get => _grid;
        set
        {
            _grid = value;
            UniverseChanged?.Invoke();
        }
    }

    public double Crowdedness
    {
        get => _filled / Grid.Length;
    }

    public event UniverseChangedEvent? UniverseChanged;

    /// <summary>
    /// Perform one generation of the Game of Life.  
    /// </summary>
    public void PassTime()
    {
        double filled = 0;
        for (int x = 0; x < _grid.GetLength(0); x++)
        {
            for (int y = 0; y < _grid.GetLength(1); y++)
            {
                _nextGrid[x, y] = NewState(x, y);
                filled += _nextGrid[x, y] ? 1 : 0;
            }
        }

        _filled = filled;
        (Grid, _nextGrid) = (_nextGrid, Grid);
    }

    /// <summary>
    /// Allows to manually toggle the state of an individual cell. 
    /// </summary>
    /// <param name="x">x coordinate</param>
    /// <param name="y">y coordinate</param>
    public void Toggle(int x, int y)
    {
        Grid[x, y] = !Grid[x, y];
        _filled += Grid[x, y] ? 1 : -1;
        UniverseChanged?.Invoke();
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

        // if cell is alive, it stays alive if 2 or 3 neighbours are alive
        if (_grid[x, y])
            return liveNeighbours is 2 or 3;

        // dead cell becomes alive if exactly 3 neighbours are alive
        return liveNeighbours is 3;
    }

    public void Clear()
    {
        _filled = 0;
        Grid = new bool[Grid.GetLength(0), Grid.GetLength(1)];
    }

    public void Reseed(double fillRatio = 0.6)
    {
        double filled = 0;
        for (var i = 0; i < Grid.GetLength(0); i++)
        {
            for (var j = 0; j < Grid.GetLength(0); j++)
            {
                _nextGrid[i, j] = _random.NextDouble() > (1 - fillRatio);
                filled += _nextGrid[i, j] ? 1 : 0;
            }
        }

        _filled = filled;
        (Grid, _nextGrid) = (_nextGrid, Grid);
    }
}