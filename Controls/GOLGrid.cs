using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using DynamicData.Binding;
using ReactiveUI;

namespace GameOfLife.Controls;

/**
 * A custom Control that paints a grid of booleans.  
 */
public class GOLGrid : Control
{
    private const int CELL_SPACING = 16;

    public bool[,] Grid
    {
        get => GetValue(GridProperty);
        set { SetValue(GridProperty, value); }
    }

    public static readonly StyledProperty<bool[,]> GridProperty =
        AvaloniaProperty.Register<GOLGrid, bool[,]>(nameof(Grid), defaultBindingMode: BindingMode.TwoWay);

    public GOLGrid()
    {
        AffectsRender<GOLGrid>(GridProperty);
        
        this.WhenAnyValue(x => x.Grid).Subscribe(g =>
        {
            Width = (g?.GetLength(0) ?? 0 ) * CELL_SPACING;
            Height = (g?.GetLength(1) ?? 0) * CELL_SPACING;
        });
    }

    public override void Render(DrawingContext context)
    {
        var gridPen = new Pen(Brushes.Gray);
        var cellPen = new Pen(Brushes.Yellow);

        for (int i = 0; i <= Grid.GetLength(0); i++)
        {
            var from = new Point(i * CELL_SPACING, 0);
            var to = new Point(i * CELL_SPACING, Height);
            context.DrawLine(gridPen, from, to);
        }

        for (int i = 0; i <= Grid.GetLength(1); i++)
        {
            var from = new Point(0, i * CELL_SPACING);
            var to = new Point(Width, i * CELL_SPACING);
            context.DrawLine(gridPen, from, to);
        }

        for (int i = 0; i < Grid.GetLength(0); i++)
        {
            for (int j = 0; j < Grid.GetLength(1); j++)
            {
                if (Grid[i, j])
                {
                    var cellArea = new Rect(i * CELL_SPACING + 1,
                        j * CELL_SPACING + 1,
                        CELL_SPACING - 2,
                        CELL_SPACING - 2);
                    context.DrawRectangle(Brushes.Yellow, cellPen, cellArea);
                }
            }
        }
    }
}