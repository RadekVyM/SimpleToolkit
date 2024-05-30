using Microsoft.Maui.Layouts;
using Dimension = Microsoft.Maui.Primitives.Dimension;
using PrimitiveLayoutAlignment = Microsoft.Maui.Primitives.LayoutAlignment;

namespace SimpleToolkit.Core;

public class WaterfallGridLayoutManager(WaterfallGrid layout) : LayoutManager(layout)
{
    private readonly WaterfallGrid layout = layout;

    public override Size ArrangeChildren(Rect bounds)
    {
        var padding = layout.Padding;
        var availableWidth = bounds.Width - padding.HorizontalThickness;
        var availableHeight = bounds.Height - padding.VerticalThickness;
        var left = padding.Left + bounds.Left;
        var top = padding.Top + bounds.Top;
        WaterfallColumn[] columns = CalculateColumns(availableWidth, (child, width, height) => child.DesiredSize);

        for (var col = 0; col < columns.Length; col++)
        {
            var column = columns[col];
            var currentTop = top + layout.AlignContent.Alignment switch
            {
                LayoutAlignment.Center => (availableHeight - column.SecondarySize) / 2,
                LayoutAlignment.End => availableHeight - column.SecondarySize,
                _ => 0
            };

            foreach (var view in column.Views)
            {
                var width = view.HorizontalLayoutAlignment == PrimitiveLayoutAlignment.Fill ?
                    column.PrimarySize :
                    view.DesiredSize.Width;
                var height = view.DesiredSize.Height;
                var currentLeft = left + ((column.PrimarySize + layout.ColumnSpacing) * col) + view.HorizontalLayoutAlignment switch
                {
                    PrimitiveLayoutAlignment.Center => (column.PrimarySize - width) / 2,
                    PrimitiveLayoutAlignment.End => column.PrimarySize - width,
                    _ => 0
                };

                if (layout.AlignContent.Alignment == LayoutAlignment.Fill)
                {
                    var factor = height / column.SecondarySize;
                    height = availableHeight * factor;
                }

                view.Arrange(new Rect(currentLeft, currentTop, width, height));
                currentTop += height + layout.RowSpacing;
            }
        }

        return bounds.Size;
    }

    public override Size Measure(double widthConstraint, double heightConstraint)
    {
        var padding = layout.Padding;
        WaterfallColumn[] columns = CalculateColumns(widthConstraint - padding.HorizontalThickness, (child, width, height) => child.Measure(width, height));

        var explicitWidth = layout.WidthRequest >= 0 ? layout.WidthRequest : Dimension.Unset;
        var explicitHeight = layout.HeightRequest >= 0 ? layout.HeightRequest : Dimension.Unset;

        var finalWidth = ResolveConstraints(widthConstraint, explicitWidth, widthConstraint, layout.MinimumWidthRequest, layout.MaximumWidthRequest);
        var finalHeight = ResolveConstraints(heightConstraint, explicitHeight, columns.MaxBy((c) => c.SecondarySize).SecondarySize + padding.VerticalThickness, layout.MinimumHeightRequest, layout.MaximumHeightRequest);

        return new Size(finalWidth, finalHeight);
    }

    private WaterfallColumn[] CalculateColumns(double widthConstraint, Func<IView, double, double, Size> childSize)
    {
        var columnsCount = Math.Max(layout.Span, 1);
        var columnWidth = (widthConstraint - ((columnsCount - 1) * layout.ColumnSpacing)) / columnsCount;
        var columns = new WaterfallColumn[columnsCount];

        for (int i = 0; i < columns.Length; i++)
        {
            columns[i] = new WaterfallColumn
            {
                PrimarySize = columnWidth
            };
        }

        for (int i = 0; i < layout.Count; i++)
        {
            var child = layout[i];
            if (child.Visibility == Visibility.Collapsed)
                continue;

            var measure = childSize(child, columnWidth, double.PositiveInfinity);
            var currentColumn = FindShortestColumnIndex(columns);

            columns[currentColumn].SecondarySize += measure.Height + layout.RowSpacing;
            columns[currentColumn].Views.AddLast(child);
        }

        for (int i = 0; i < columns.Length; i++)
            columns[i].SecondarySize = Math.Max(0, columns[i].SecondarySize - layout.RowSpacing);

        return columns;
    }

    private int FindShortestColumnIndex(WaterfallColumn[] columns)
    {
        int index = 0;

        for (int i = 0; i < columns.Length; i++)
        {
            if (columns[i].SecondarySize < columns[index].SecondarySize)
                index = i;
        }

        return index;
    }

    class WaterfallColumn
    {
        public double PrimarySize { get; set; } = 0; 
        public double SecondarySize { get; set; } = 0; 
        public LinkedList<IView> Views { get; set; } = new();
    }
}