using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Threading;
using DynamicData.Binding;
using ReactiveUI;
using Splat;

namespace GameOfLife.Controls;

/// <summary>
/// A custom Control that displays a grid of booleans.
/// </summary>
public class BooleanGrid : Control, IEnableLogger
{
    private const int CellSpacing = 16;

    public static readonly StyledProperty<bool[,]> GridProperty =
        AvaloniaProperty.Register<BooleanGrid, bool[,]>(nameof(Grid));

    public bool[,] Grid
    {
        get => GetValue(GridProperty);
        set => SetValue(GridProperty, value);
    }

    public BooleanGrid()
    {
        AffectsRender<BooleanGrid>(GridProperty);
        
        this.WhenAnyValue(x => x.Grid).Subscribe(g =>
        {
            Width = (g?.GetLength(0) ?? 0 ) * CellSpacing;
            Height = (g?.GetLength(1) ?? 0) * CellSpacing;
        });
    }

    public override void Render(DrawingContext context)
    {
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
                    var cellArea = new Rect(i * CellSpacing + 1,
                        j * CellSpacing + 1,
                        CellSpacing - 2,
                        CellSpacing - 2);

                    context.DrawRectangle(Brushes.Yellow, null, cellArea);
                }
            }
        }
    }
}