using Avalonia.ReactiveUI;
using GameOfLife.ViewModels;

namespace GameOfLife.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void GridCellClicked(int x, int y)
    {
        ViewModel?.GridCellClicked(x, y);
    }
}