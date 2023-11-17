namespace SimpleToolkit.SimpleShell.Controls;

public partial class ListPopover
{
    private void UpdateValuesToFluent()
    {
        fontSize = 14;
        containerCornerRadius = 10;
        listItemHeight = 46;
        iconSize = new Size(16, 16);
        iconMargin = new Thickness(16, 0, 0, 0);
        labelMargin = new Thickness(16, 0, 16, 0);
        stackLayoutPadding = new Thickness(0);
    }

    private void UpdateDrawableToFluent()
    {
        if (graphicsView is null)
            return;

        if (graphicsView.Drawable is not FluentDrawable drawable)
            graphicsView.Drawable = drawable = new FluentDrawable();

        drawable.PopoverBackground = Background;
        drawable.SelectionBrush = SelectionBrush;
        drawable.ScrollPosition = scrollPosition;
        drawable.Padding = stackLayoutPadding;
        drawable.ItemHeight = listItemHeight;
        drawable.ItemsCount = allItemViews.Count;
        drawable.SelectedItemIndex = GetSelectedItemIndex();
    }

    private class FluentDrawable : IDrawable
    {
        private float lineThickness = 3f;
        private Thickness selectionPadding = new Thickness(4);
        private float selectionCornerRadius = 6f;
        private float colorOffset = -0.04f; // -0.024f

        public Brush PopoverBackground { get; set; }
        public Brush SelectionBrush { get; set; }
        public Thickness Padding { get; set; }
        public double ItemHeight { get; set; }
        public double ScrollPosition { get; set; }
        public int ItemsCount { get; set; }
        public int SelectedItemIndex { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (SelectedItemIndex == -1)
                return;
            
            canvas.SaveState();

            float y = (float)(Padding.Top + (ItemHeight * SelectedItemIndex) - ScrollPosition);

            if (y >= dirtyRect.Height || y <= -ItemHeight)
                return;

            canvas.SetFillPaint(PopoverBackground.OffsetBrushColorValue(colorOffset), dirtyRect);
            RectF rect = new Rect(selectionPadding.Left, y + selectionPadding.Top, dirtyRect.Width - selectionPadding.HorizontalThickness, ItemHeight - selectionPadding.VerticalThickness);

            canvas.FillRoundedRectangle(rect, selectionCornerRadius);

            RectF pillRect = new Rect(rect.X, rect.Y + (rect.Height / 4), lineThickness, rect.Height / 2);
            canvas.SetFillPaint(SelectionBrush, pillRect);

            canvas.FillRoundedRectangle(pillRect, lineThickness / 2);

            canvas.RestoreState();
        }
    }
}