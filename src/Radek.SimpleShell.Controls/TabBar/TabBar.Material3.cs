namespace Radek.SimpleShell.Controls
{
    public partial class TabBar
    {
        private void UpdateValuesToMaterial3()
        {
            iconSize = new Size(20, 20);
            iconMargin = new Thickness(0, 12 + ((32 - iconSize.Height) / 2d), 0, 0);
            buttonPadding = new Thickness(0, 32, 0, 0);
            tabBarHeight = 76;
            realMinimalItemWidth = 64;
            fontSize = 12;
            buttonTextTransform = TextTransform.None;
        }

        private void UpdateDrawableToMaterial3()
        {
            if (graphicsView is null)
                return;

            if (graphicsView.Drawable is not Material3Drawable drawable)
                graphicsView.Drawable = drawable = new Material3Drawable();

            drawable.Alignment = ItemsAlignment;
            drawable.PillBrush = PrimaryBrush;
            drawable.ScrollPosition = IsScrollable ? scrollPosition : 0;
            drawable.ItemWidth = itemWidth;
            drawable.ScrollViewWidth = IsScrollable ? scrollView.Width : border.Width;
            drawable.ItemsCount = Items?.Count() ?? 0;
            drawable.SelectedItemRelativePosition = GetSelectedItemIndex();
        }

        private class Material3Drawable : IDrawable
        {
            private float pillHeight = 32;
            private float pillWidth = 64;
            private float topPadding = 12;

            public double ItemWidth { get; set; }
            public double ScrollViewWidth { get; set; }
            public double ItemsCount { get; set; }
            public double ScrollPosition { get; set; }
            public double SelectedItemRelativePosition { get; set; }
            public Brush PillBrush { get; set; }
            public LayoutAlignment Alignment { get; set; }

            public void Draw(ICanvas canvas, RectF dirtyRect)
            {
                if (SelectedItemRelativePosition > -1)
                {
                    canvas.SaveState();

                    canvas.ClipRectangle(dirtyRect);

                    float leftPadding = 0;

                    if (ScrollViewWidth < dirtyRect.Width)
                    {
                        leftPadding = Alignment switch
                        {
                            LayoutAlignment.Center => (float)((dirtyRect.Width - ScrollViewWidth) / 2f),
                            LayoutAlignment.End => (float)(dirtyRect.Width - ScrollViewWidth),
                            _ => 0
                        };
                    }

                    var itemWidth = double.IsNaN(ItemWidth) ? ScrollViewWidth / ItemsCount : ItemWidth;
                    var left = (float)((SelectedItemRelativePosition * itemWidth) + ((itemWidth - pillWidth) / 2) - ScrollPosition + leftPadding);

                    var pillRect = new RectF(left, topPadding, pillWidth, pillHeight);

                    canvas.SetFillPaint(PillBrush ?? Colors.Black, pillRect);

                    canvas.FillRoundedRectangle(pillRect, pillHeight / 2);

                    canvas.ResetState();
                }
            }
        }
    }
}
