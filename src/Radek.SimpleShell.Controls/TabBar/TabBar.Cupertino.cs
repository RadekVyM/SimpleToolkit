namespace Radek.SimpleShell.Controls
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
            realMinimumItemWidth = 64;
            fontSize = 12;
            labelTextTransform = TextTransform.None;
            labelAttributes = FontAttributes.None;
            labelSelectionAttributes = FontAttributes.None;
            itemStackLayoutOrientation = isCompact ? StackOrientation.Horizontal : StackOrientation.Vertical;
        }

        private void UpdateDrawableToCupertino()
        {
            if (graphicsView is null)
                return;

            if (graphicsView.Drawable is not CupertinoDrawable drawable)
                graphicsView.Drawable = drawable = new CupertinoDrawable();

            drawable.LineBrush = PrimaryBrush;
        }

        private class CupertinoDrawable : IDrawable
        {
            private float lineHeight = 1.5f;

            public Brush LineBrush { get; set; }

            public void Draw(ICanvas canvas, RectF dirtyRect)
            {
                canvas.SaveState();

                canvas.SetFillPaint(LineBrush ?? Colors.LightGray, dirtyRect);

                canvas.FillRectangle(0, 0, dirtyRect.Width, lineHeight);

                canvas.RestoreState();
            }
        }
    }
}
