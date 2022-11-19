using System;
using System.Windows.Input;
using GameOfLife.Models;
using ReactiveUI;
using Splat;

namespace GameOfLife.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private double _crowdedness;
    private bool[,] _grid;
    private Universe _universe;

    public MainWindowViewModel()
    {
        _universe = Locator.GetLocator().GetService<Universe>()!;
        _universe.UniverseChanged += () =>
        {
            Grid = _universe.Grid;
            Crowdedness = _universe.Crowdedness;
        };
        _grid = _universe.Grid;

        var timeService = Locator.GetLocator().GetService<ITimeService>()!;
        ControlPanel = new ControlPanelViewModel(_universe, timeService);

        ToggleCellCommand = ReactiveCommand.Create<Tuple<int, int>>((a) => _universe.Toggle(a.Item1, a.Item2));
    }

    public bool[,] Grid
    {
        get => _grid;
        set => this.RaiseAndSetIfChanged(ref _grid, value);
    }

    public double Crowdedness
    {
        get => _crowdedness;
        set => this.RaiseAndSetIfChanged(ref _crowdedness, value);
    }

    public ControlPanelViewModel ControlPanel { get; }

    public ICommand ToggleCellCommand { get; }

    public void GridCellClicked(int x, int y)
    {
        _universe.Toggle(x, y);
    }
}