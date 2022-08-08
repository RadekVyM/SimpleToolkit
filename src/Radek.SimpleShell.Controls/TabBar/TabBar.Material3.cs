namespace Radek.SimpleShell.Controls
{
    public partial class TabBar
    {
        private void UpdateValuesToMaterial3()
        {
            itemStackLayoutPadding = new Thickness(0);
            iconSize = new Size(20, 20);
            iconMargin = new Thickness(0, 12 + ((32 - iconSize.Height) / 2d), 0, 0);
            itemStackLayoutSpacing = 8;
            tabBarHeight = 76;
            realMinimumItemWidth = 70;
            fontSize = 12;
            labelTextTransform = TextTransform.None;
            labelAttributes = FontAttributes.None;
            labelSelectionAttributes = FontAttributes.Bold;
            itemStackLayoutOrientation = StackOrientation.Vertical;
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
            drawable.ContainerViewWidth = IsScrollable ? scrollView.Width : border.Width;
            drawable.SelectedItemRelativePosition = GetSelectedItemIndex();
            drawable.Views = stackLayout.Children;
        }

        private class Material3Drawable : IDrawable
        {
            private float pillHeight = 32;
            private float pillWidth = 64;
            private float topPadding = 12;

            public double ContainerViewWidth { get; set; }
            public double ScrollPosition { get; set; }
            public double SelectedItemRelativePosition { get; set; }
            public Brush PillBrush { get; set; }
            public LayoutAlignment Alignment { get; set; }
            public IList<IView> Views { get; set; }

            public void Draw(ICanvas canvas, RectF dirtyRect)
            {
                canvas.SaveState();

                if (SelectedItemRelativePosition > -1)
                {
                    float leftPadding = 0;

                    if (ContainerViewWidth < dirtyRect.Width)
                    {
                        leftPadding = Alignment switch
                        {
                            LayoutAlignment.Center => (float)((dirtyRect.Width - ContainerViewWidth) / 2f),
                            LayoutAlignment.End => (float)(dirtyRect.Width - ContainerViewWidth),
                            _ => 0
                        };
                    }

                    double leftItemsWidth = 0;

                    var flooredPosition = (int)Math.Floor(SelectedItemRelativePosition);

                    if (flooredPosition >= Views.Count)
                        return;

                    for (int i = 0; i < flooredPosition; i++)
                    {
                        var view = Views[i] as Grid;
                        leftItemsWidth += view.Width;
                    }

                    var selectedView = Views[flooredPosition] as Grid;
                    var itemWidth = double.IsFinite(selectedView.Width) ? selectedView.Width : 0;
                    var left = (float)(leftItemsWidth + ((itemWidth - pillWidth) / 2) - ScrollPosition + leftPadding);

                    var pillRect = new RectF(left, topPadding, pillWidth, pillHeight);

                    System.Diagnostics.Debug.WriteLine(pillRect.ToString());

                    canvas.SetFillPaint(PillBrush ?? Colors.Gray, pillRect);

                    canvas.FillRoundedRectangle(pillRect, pillHeight / 2);
                    canvas.ClipRectangle(dirtyRect);
                }

                canvas.RestoreState();
            }
        }
    }
}
