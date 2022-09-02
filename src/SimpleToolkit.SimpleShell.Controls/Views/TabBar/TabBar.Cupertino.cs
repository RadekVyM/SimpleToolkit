using SimpleToolkit.SimpleShell.Controls.Extensions;

namespace SimpleToolkit.SimpleShell.Controls
{
    public partial class TabBar
    {
        private void UpdateValuesToCupertino()
        {
            var isCompact = itemWidth > 180;

            itemStackLayoutPadding = new Thickness(0);
            iconSize = isCompact ? new Size(18, 18) : new Size(25, 25);
            iconMargin = new Thickness(0, isCompact ? 0 : 8, 0, 0);
            itemStackLayoutSpacing = isCompact ? 8 : 4;
            tabBarHeight = isCompact ? 56 : 64;
            realMinimumItemWidth = 60;
            fontSize = 12;
            labelTextTransform = TextTransform.None;
            labelAttributes = FontAttributes.None;
            labelSelectionAttributes = FontAttributes.None;
            itemStackLayoutOrientation = isCompact ? StackOrientation.Horizontal : StackOrientation.Vertical;
            isMoreLabelVisible = true;
        }

        private void UpdateDrawableToCupertino()
        {
            if (graphicsView is null)
                return;

            if (graphicsView.Drawable is not CupertinoDrawable drawable)
            {
                if (graphicsView.Drawable is IDisposable disposable)
                    disposable.Dispose();
                graphicsView.Drawable = drawable = new CupertinoDrawable();
            }

            drawable.LineBrush = PrimaryBrush;
            drawable.Alignment = ItemsAlignment;
            drawable.IconColor = IconColor;
            drawable.ScrollPosition = IsScrollable ? scrollPosition : 0;
            drawable.ContainerViewWidth = IsScrollable ? scrollView.Width : border.Width;
            drawable.Views = stackLayout.Children;
            drawable.DrawMoreIcon = isMoreButtonShown && (MoreIconProperty.DefaultValue == MoreIcon || MoreIcon == null);
            drawable.IconSize = iconSize;
            drawable.IconMargin = iconMargin;
            drawable.StackOrientation = itemStackLayoutOrientation;
        }

        private class CupertinoDrawable : IDrawable, IDisposable
        {
            private float dotRadius = 2.5f;
            private float lineHeight = 1.5f;

            public Color IconColor { get; set; }
            public Brush LineBrush { get; set; }
            public LayoutAlignment Alignment { get; set; }
            public double ContainerViewWidth { get; set; }
            public double ScrollPosition { get; set; }
            public IList<IView> Views { get; set; }
            public bool DrawMoreIcon { get; set; }
            public Size IconSize { get; set; }
            public Thickness IconMargin { get; set; }
            public StackOrientation StackOrientation { get; set; }

            public void Dispose()
            {
                Views = null;
                IconColor = null;
                LineBrush = null;
            }

            public void Draw(ICanvas canvas, RectF dirtyRect)
            {
                canvas.SaveState();

                canvas.SetFillPaint(LineBrush ?? Colors.LightGray, dirtyRect);

                canvas.FillRectangle(0, 0, dirtyRect.Width, lineHeight);

                if (!DrawMoreIcon)
                {
                    canvas.RestoreState();
                    return;
                }

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

                double left = 0;
                double lastItemWidth = (Views.LastOrDefault() as View)?.Width ?? 0;

                for (int i = 0; i < Views.Count - 1; i++)
                {
                    var view = Views[i] as View;
                    left += view.Width;
                }

                left += ((lastItemWidth - IconSize.Width) / 2) - ScrollPosition + leftPadding;

                RectF rect = StackOrientation is StackOrientation.Horizontal ?
                    new Rect(left, (dirtyRect.Height - IconSize.Height) / 2, IconSize.Width, IconSize.Height) :
                    new Rect(left, IconMargin.Top, IconSize.Width, IconSize.Height);

                canvas.StrokeSize = 1.5f;
                canvas.StrokeColor = IconColor;
                canvas.DrawHorizontalMoreIcon(rect, dotRadius);

                canvas.RestoreState();
            }
        }
    }
}
