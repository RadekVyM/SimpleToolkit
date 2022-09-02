using SimpleToolkit.SimpleShell.Controls.Extensions;

namespace SimpleToolkit.SimpleShell.Controls
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
            isMoreLabelVisible = true;
        }

        private void UpdateDrawableToMaterial3()
        {
            if (graphicsView is null)
                return;

            if (graphicsView.Drawable is not Material3Drawable drawable)
            {
                if (graphicsView.Drawable is IDisposable disposable)
                    disposable.Dispose();
                graphicsView.Drawable = drawable = new Material3Drawable();
            }

            var selectedIndex = GetSelectedItemIndex();

            drawable.Alignment = ItemsAlignment;
            drawable.IconColor = IconColor;
            drawable.PillBrush = PrimaryBrush;
            drawable.ScrollPosition = IsScrollable ? scrollPosition : 0;
            drawable.ContainerViewWidth = IsScrollable ? scrollView.Width : border.Width;
            drawable.SelectedItemRelativePosition = selectedIndex;
            drawable.Views = stackLayout.Children;
            drawable.DrawMoreIcon = isMoreButtonShown && (MoreIconProperty.DefaultValue == MoreIcon || MoreIcon == null);
            drawable.IsSelectedHiddenItem = isMoreButtonShown && selectedIndex == stackLayout.Count - 1;
            drawable.IconSize = iconSize;
            drawable.IconMargin = iconMargin;
        }

        private class Material3Drawable : IDrawable, IDisposable
        {
            private float dotRadius = 1.5f;
            private float pillHeight = 32;
            private float pillWidth = 64;
            private float topPadding = 12;

            public double ContainerViewWidth { get; set; }
            public double ScrollPosition { get; set; }
            public double SelectedItemRelativePosition { get; set; }
            public Color IconColor { get; set; }
            public Brush PillBrush { get; set; }
            public LayoutAlignment Alignment { get; set; }
            public IList<IView> Views { get; set; }
            public bool DrawMoreIcon { get; set; }
            public bool IsSelectedHiddenItem { get; set; }
            public Size IconSize { get; set; }
            public Thickness IconMargin { get; set; }

            public void Dispose()
            {
                Views = null;
                IconColor = null;
                PillBrush = null;
            }

            public void Draw(ICanvas canvas, RectF dirtyRect)
            {
                if (SelectedItemRelativePosition < 0)
                    return;

                canvas.SaveState();

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

                if (!IsSelectedHiddenItem)
                {
                    DrawPill(canvas, dirtyRect, leftPadding);
                }
                if (DrawMoreIcon)
                {
                    double left = 0;
                    double lastItemWidth = (Views.LastOrDefault() as View)?.Width ?? 0;

                    for (int i = 0; i < Views.Count - 1; i++)
                    {
                        var view = Views[i] as View;
                        left += view.Width;
                    }

                    left += ((lastItemWidth - IconSize.Width) / 2) - ScrollPosition + leftPadding;

                    RectF rect = new Rect(left, IconMargin.Top, IconSize.Width, IconSize.Height);

                    canvas.SetFillPaint(new SolidColorBrush(IconColor), rect);
                    canvas.FillHorizontalMoreIcon(rect, dotRadius);
                }
                
                canvas.ClipRectangle(dirtyRect);

                canvas.RestoreState();
            }

            private void DrawPill(ICanvas canvas, RectF dirtyRect, float leftPadding)
            {
                double leftItemsWidth = 0;

                var flooredPosition = (int)Math.Floor(SelectedItemRelativePosition);

                if (flooredPosition >= Views.Count)
                    return;

                for (int i = 0; i < flooredPosition; i++)
                {
                    var view = Views[i] as View;
                    leftItemsWidth += view.Width;
                }

                var selectedView = Views[flooredPosition] as View;
                var itemWidth = double.IsFinite(selectedView.Width) ? selectedView.Width : 0;
                var left = (float)(leftItemsWidth + ((itemWidth - pillWidth) / 2) - ScrollPosition + leftPadding);

                var pillRect = new RectF(left, topPadding, pillWidth, pillHeight);

                canvas.SetFillPaint(PillBrush ?? Colors.Gray, pillRect);

                canvas.FillRoundedRectangle(pillRect, pillHeight / 2);
            }
        }
    }
}
