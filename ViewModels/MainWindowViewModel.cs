using GameOfLife.Models;
using ReactiveUI;
using Splat;

namespace GameOfLife.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private bool[,] _grid;
    private Universe _universe;

    public MainWindowViewModel()
    {
        _universe = Locator.GetLocator().GetService<Universe>()!;
        _universe.UniverseChanged += () => { Grid = _universe.Grid; };
        _grid = _universe.Grid;

        var timeService = Locator.GetLocator().GetService<ITimeService>()!;
        ControlPanel = new ControlPanelViewModel(timeService);
    }

    public bool[,] Grid
    {
        get => _grid;
        set => this.RaiseAndSetIfChanged(ref _grid, value);
    }

    public ControlPanelViewModel ControlPanel { get; }
}