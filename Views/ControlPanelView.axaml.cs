using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GameOfLife.Views;

public partial class ControlPanelView : UserControl
{
    public ControlPanelView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}