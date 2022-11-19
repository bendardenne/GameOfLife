using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using ReactiveUI;
using Splat;

namespace GameOfLife.Controls;

public delegate void CellClickedEvent(int x, int y);

/// <summary>
/// A custom Control that displays a grid of booleans.
/// </summary>
public class BooleanGrid : Control, IEnableLogger
{
    private const int CellSpacing = 16;

    public static readonly StyledProperty<bool[,]> GridProperty =
        AvaloniaProperty.Register<BooleanGrid, bool[,]>(nameof(Grid));

    private Tuple<int, int>? _hoverCell;

    public BooleanGrid()
    {
        AffectsRender<BooleanGrid>(GridProperty);

        this.WhenAnyValue(x => x.Grid).Subscribe(g =>
        {
            Width = (g?.GetLength(0) ?? 0) * CellSpacing;
            Height = (g?.GetLength(1) ?? 0) * CellSpacing;
        });

        PointerMoved += (sender, args) =>
        {
            var position = args.GetPosition(this);
            _hoverCell = new Tuple<int, int>((int)(position.X / CellSpacing), (int)(position.Y / CellSpacing));
            InvalidateVisual();
        };

        PointerLeave += (sender, args) =>
        {
            _hoverCell = null;
            InvalidateVisual();
        };

        PointerPressed += (sender, args) =>
        {
            CellClicked?.Invoke(_hoverCell!.Item1, _hoverCell!.Item2);
            InvalidateVisual();
        };
    }

    public bool[,] Grid
    {
        get => GetValue(GridProperty);
        set => SetValue(GridProperty, value);
    }

    public event CellClickedEvent? CellClicked;

    public override void Render(DrawingContext context)
    {
        // Some background is needed in order to receive events everywhere.
        context.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, Width, Height));

        var gridPen = new Pen(Brushes.Gray);

        for (var i = 0; i <= Grid.GetLength(0); i++)
        {
            var from = new Point(i * CellSpacing, 0);
            var to = new Point(i * CellSpacing, Height);
            context.DrawLine(gridPen, from, to);
        }

        for (var i = 0; i <= Grid.GetLength(1); i++)
        {
            var from = new Point(0, i * CellSpacing);
            var to = new Point(Width, i * CellSpacing);
            context.DrawLine(gridPen, from, to);
        }

        for (var i = 0; i < Grid.GetLength(0); i++)
        {
            for (var j = 0; j < Grid.GetLength(1); j++)
            {
                if (Grid[i, j])
                {
                    context.DrawRectangle(Brushes.Yellow, gridPen, AreaFromCoordinates(i, j));
                }
            }
        }

        if (_hoverCell != null)
        {
            var hoveredCellArea = AreaFromCoordinates(_hoverCell.Item1, _hoverCell.Item2);
            context.DrawRectangle(Brushes.LightSeaGreen, null, hoveredCellArea);
        }
    }

    private Rect AreaFromCoordinates(int x, int y)
    {
        return new Rect(x * CellSpacing + 1,
            y * CellSpacing + 1,
            CellSpacing - 2,
            CellSpacing - 2);
    }
}