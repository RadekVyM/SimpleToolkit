namespace SimpleToolkit.SimpleShell.Controls;

public partial class ListPopover
{
    private void UpdateValuesToCupertino()
    {
        fontSize = 14;
        containerCornerRadius = 12;
        listItemHeight = 44;
        iconSize = new Size(20, 20);
        iconMargin = new Thickness(12, 0, 0, 0);
        labelMargin = new Thickness(12, 0, 12, 0);
        stackLayoutPadding = new Thickness(0);
    }

    private void UpdateDrawableToCupertino()
    {
        if (graphicsView is null)
            return;

        if (graphicsView.Drawable is not CupertinoDrawable drawable)
            graphicsView.Drawable = drawable = new CupertinoDrawable();

        drawable.PopoverBackground = Background;
        drawable.Padding = stackLayoutPadding;
        drawable.ScrollPosition = scrollPosition;
        drawable.ItemHeight = listItemHeight;
        drawable.ItemsCount = allItemViews.Count;
    }

    private class CupertinoDrawable : IDrawable
    {
        private float lineHeight = 1f;
        private float colorOffset = -0.1f;

        public Brush PopoverBackground { get; set; }
        public Thickness Padding { get; set; }
        public double ScrollPosition { get; set; }
        public double ItemHeight { get; set; }
        public int ItemsCount { get; set; }


        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();

            canvas.SetFillPaint(PopoverBackground.OffsetBrushColorValue(colorOffset), dirtyRect);

            float y = (float)(Padding.Top + ItemHeight - (ScrollPosition % dirtyRect.Height));
            int i = 1;

            while(i < ItemsCount && y <= dirtyRect.Height)
            {
                canvas.FillRectangle(0, y, dirtyRect.Width, lineHeight);
                y += (float)ItemHeight;
                i++;
            }

            canvas.RestoreState();
        }
    }
}