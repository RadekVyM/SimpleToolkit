namespace Radek.SimpleShell.Controls
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
        }

        private void UpdateDrawableToFluent()
        {
            if (graphicsView is null)
                return;

            if (graphicsView.Drawable is not FluentDrawable drawable)
                graphicsView.Drawable = drawable = new FluentDrawable();

            drawable.LineBrush = PrimaryBrush;
            drawable.Alignment = ItemsAlignment;
            drawable.ScrollPosition = IsScrollable ? scrollPosition : 0;
            drawable.ContainerViewWidth = IsScrollable ? scrollView.Width : border.Width;
            drawable.SelectedItemRelativePosition = GetSelectedItemIndex();
            drawable.Views = stackLayout.Children;
        }

        private async Task AnimateFluentToSelected()
        {
            uint animationLength = 250;

            if (graphicsView.Drawable is not FluentDrawable drawable)
                return;

            var fromPosition = drawable.SelectedItemRelativePosition;
            var toPosition = GetSelectedItemIndex();

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
            private float bottomPadding = 4f;
            private float lineHeight = 4f;
            private float defaultLineWidth = 16f;

            public double AnimationProgress { get; set; }
            public Brush LineBrush { get; set; }
            public IList<IView> Views { get; set; }
            public double ContainerViewWidth { get; set; }
            public double ScrollPosition { get; set; }
            public double SelectedItemRelativePosition { get; set; }
            public LayoutAlignment Alignment { get; set; }

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

                double leftItemsWidth = 0;

                var flooredPosition = (int)Math.Floor(SelectedItemRelativePosition);

                if (flooredPosition >= Views.Count)
                    return;

                for (int i = 0; i < flooredPosition; i++)
                {
                    // IView.Width does not return proper current width of the control
                    var view = Views[i] as Grid;
                    leftItemsWidth += view.Width;
                }

                // TODO: Animation does not look as it should

                var selectedView = Views[flooredPosition] as Grid;
                var itemWidth = selectedView.Width;
                var left = (float)(leftItemsWidth + ((itemWidth - defaultLineWidth) / 2) - ScrollPosition + leftPadding + ((SelectedItemRelativePosition - flooredPosition) * itemWidth));

                var lineRect = new RectF(left, dirtyRect.Height - bottomPadding - lineHeight, defaultLineWidth, lineHeight);

                canvas.SetFillPaint(LineBrush ?? Colors.Black, lineRect);

                canvas.FillRoundedRectangle(lineRect, lineHeight / 2);

                canvas.RestoreState();
            }
        }
    }
}
