using Microsoft.Maui.Layouts;

namespace SimpleToolkit.Core;

public class WaterfallGrid : Layout
{
    public int Span { get; set; } = 1;
    public double ColumnSpacing { get; set; } = 0;
    public double RowSpacing { get; set; } = 0;
    public LayoutOptions AlignContent { get ;set; } = LayoutOptions.Start;

    protected override ILayoutManager CreateLayoutManager()
    {
        return new WaterfallGridLayoutManager(this);
    }
}
