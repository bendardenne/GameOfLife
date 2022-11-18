using GameOfLife.Models;
using ReactiveUI;
using Splat;

namespace GameOfLife.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private Universe _universe;

    private bool[,] _grid;

    public bool[,] Grid
    {
        get => _grid;
        set => this.RaiseAndSetIfChanged(ref _grid, value);
    }

    public MainWindowViewModel()
    {
        _universe = Locator.GetLocator().GetService<Universe>()!;
        _universe.UniverseChanged += () => { Grid = _universe.Grid; };
        _grid = _universe.Grid;
    }
}