namespace SimpleToolkit.SimpleShell.Controls
{
    public partial class ListPopover
    {
        private void UpdateValuesToMaterial3()
        {
            fontSize = 14;
            containerCornerRadius = 4;
            listItemHeight = 48;
            iconSize = new Size(20, 20);
            iconMargin = new Thickness(12, 0, 0, 0);
            labelMargin = new Thickness(12, 0, 12, 0);
            stackLayoutPadding = new Thickness(0, 8, 0, 8);
        }

        private void UpdateDrawableToMaterial3()
        {
            if (graphicsView is null)
                return;

            if (graphicsView.Drawable is not Material3Drawable drawable)
                graphicsView.Drawable = drawable = new Material3Drawable();

            drawable.PopoverBackground = Background;
            drawable.ScrollPosition = scrollPosition;
            drawable.Padding = stackLayoutPadding;
            drawable.ItemHeight = listItemHeight;
            drawable.ItemsCount = allItemViews.Count;
            drawable.SelectedItemIndex = GetSelectedItemIndex();
        }

        private class Material3Drawable : IDrawable
        {
            private float colorOffset = -0.04f;

            public Brush PopoverBackground { get; set; }
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
                RectF rect = new Rect(0, y, dirtyRect.Width, ItemHeight);

                canvas.FillRectangle(rect);

                canvas.RestoreState();
            }
        }
    }
}
