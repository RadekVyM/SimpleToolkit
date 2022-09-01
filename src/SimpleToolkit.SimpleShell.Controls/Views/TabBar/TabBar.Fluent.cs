using SimpleToolkit.SimpleShell.Controls.Extensions;

namespace SimpleToolkit.SimpleShell.Controls
{
    public partial class TabBar
    {
        private void UpdateValuesToFluent()
        {
            itemStackLayoutPadding = new Thickness(12, 0);
            iconSize = new Size(18, 18);
            iconMargin = new Thickness(0, 0, 0, 0);
            itemStackLayoutSpacing = 8;
            tabBarHeight = 46;
            realMinimumItemWidth = 64;
            fontSize = 14;
            labelTextTransform = TextTransform.None;
            labelAttributes = FontAttributes.None;
            labelSelectionAttributes = FontAttributes.None;
            itemStackLayoutOrientation = StackOrientation.Horizontal;
            isMoreLabelVisible = false;
        }

        private void UpdateDrawableToFluent()
        {
            if (graphicsView is null)
                return;

            if (graphicsView.Drawable is not FluentDrawable drawable)
                graphicsView.Drawable = drawable = new FluentDrawable();

            var selectedIndex = GetSelectedItemIndex();

            drawable.LineBrush = PrimaryBrush;
            drawable.IconColor = IconColor;
            drawable.Alignment = ItemsAlignment;
            drawable.ScrollPosition = IsScrollable ? scrollPosition : 0;
            drawable.ContainerViewWidth = IsScrollable ? scrollView.Width : border.Width;
            drawable.SelectedItemRelativePosition = selectedIndex;
            drawable.Views = stackLayout.Children;
            drawable.DrawMoreIcon = isMoreButtonShown && (MoreIconProperty.DefaultValue == MoreIcon || MoreIcon == null);
            drawable.IsSelectedHiddenItem = isMoreButtonShown && selectedIndex == stackLayout.Count - 1;
            drawable.IconSize = iconSize;
            drawable.IconMargin = iconMargin;
        }

        private async Task AnimateFluentToSelected()
        {
            uint animationLength = 250;

            if (graphicsView.Drawable is not FluentDrawable drawable)
                return;

            var fromPosition = drawable.SelectedItemRelativePosition;
            var toPosition = GetSelectedItemIndex();

            if (toPosition >= stackLayout.Count - 1)
            {
                toPosition = stackLayout.Count - 1;

                if (fromPosition >= toPosition)
                {
                    return;
                }
            }

            fromPosition = fromPosition <= stackLayout.Count - 1 ? fromPosition : stackLayout.Count - 1;

            if (toPosition < stackLayout.Count - 1)
                drawable.IsSelectedHiddenItem = false;

            var animation = new Animation(v =>
            {
                drawable.SelectedItemRelativePosition = v;
                drawable.AnimationProgress = Math.Abs((toPosition - fromPosition) / v);
                graphicsView.Invalidate();
            }, fromPosition, toPosition);

            animation.Commit(graphicsView, "FluentLineAnimation", length: animationLength, easing: Easing.SinInOut);

            await Task.Delay((int)animationLength);
        }

        private class FluentDrawable : IDrawable
        {
            private float dotRadius = 1.2f;
            private float bottomPadding = 4f;
            private float lineThickness = 4f;
            private float defaultLineWidth = 16f;

            public double AnimationProgress { get; set; }
            public Color IconColor { get; set; }
            public Brush LineBrush { get; set; }
            public IList<IView> Views { get; set; }
            public double ContainerViewWidth { get; set; }
            public double ScrollPosition { get; set; }
            public double SelectedItemRelativePosition { get; set; }
            public LayoutAlignment Alignment { get; set; }
            public bool DrawMoreIcon { get; set; }
            public bool IsSelectedHiddenItem { get; set; }
            public Size IconSize { get; set; }
            public Thickness IconMargin { get; set; }

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
                    DrawLine(canvas, dirtyRect, leftPadding);
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

                    RectF rect = new Rect(left, (dirtyRect.Height - IconSize.Height) / 2, IconSize.Width, IconSize.Height);

                    canvas.SetFillPaint(new SolidColorBrush(IconColor), rect);
                    canvas.FillHorizontalMoreIcon(rect, dotRadius);
                }

                canvas.RestoreState();
            }

            private void DrawLine(ICanvas canvas, RectF dirtyRect, float leftPadding)
            {
                double leftItemsWidth = 0;

                var flooredPosition = (int)Math.Floor(SelectedItemRelativePosition);

                if (flooredPosition >= Views.Count)
                    return;

                for (int i = 0; i < flooredPosition; i++)
                {
                    // IView.Width does not return proper current width of the control
                    var view = Views[i] as View;
                    leftItemsWidth += view.Width;
                }

                // TODO: Animation does not look as it should

                var selectedView = Views[flooredPosition] as View;
                var itemWidth = selectedView.Width;
                var left = (float)(leftItemsWidth + ((itemWidth - defaultLineWidth) / 2) - ScrollPosition + leftPadding + ((SelectedItemRelativePosition - flooredPosition) * itemWidth));

                var lineRect = new RectF(left, dirtyRect.Height - bottomPadding - lineThickness, defaultLineWidth, lineThickness);

                canvas.SetFillPaint(LineBrush ?? Colors.Black, lineRect);

                canvas.FillRoundedRectangle(lineRect, lineThickness / 2);
            }
        }
    }
}
