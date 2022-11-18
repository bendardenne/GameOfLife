using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Media;

namespace GameOfLife.Models;

public delegate void UniverseChangedEvent();  


public class Universe
{
    public event UniverseChangedEvent? UniverseChanged; 
    
    private bool[,] _grid;

    public bool[,] Grid
    {
        get => _grid;
        set
        {
            _grid = value;
            UniverseChanged?.Invoke(); 
        }
    }

    public Universe(int width, int height)
    {
        _grid = new bool[width, height];
    }

    public void PassTime()
    {
        bool[,] newGrid = new bool[_grid.GetLength(0), _grid.GetLength(1)];

        for (int x = 0; x < _grid.GetLength(0); x++)
        {
            for (int y = 0; y < _grid.GetLength(1); y++)
            {
                newGrid[x, y] = NewState(x, y);
            }
        }

        Grid = newGrid;
    }

    private bool NewState(int x, int y)
    {
        int maxWidth = _grid.GetLength(0);
        int maxHeight = _grid.GetLength(1);

        int liveNeighbours = 0;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                // Dont coun't cell itself
                if (i == 0 && j == 0) continue;

                // Bounds check
                if (x + i < 0 || x + i >= maxWidth || y + j < 0 || y + j >= maxHeight) continue;

                liveNeighbours += _grid[x + i, y + j] ? 1 : 0;
            }
        }

        if (_grid[x, y])
        {
            //if cell is alive, it stays alive if 2 or 3 neighbours are alive
            return liveNeighbours == 2 || liveNeighbours == 3;
        }

        // dead cell becomes alive if exactly 3 neighbours are alive
        return liveNeighbours == 3;
    }
}