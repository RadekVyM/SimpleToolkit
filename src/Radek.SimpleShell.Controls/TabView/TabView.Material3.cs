namespace Radek.SimpleShell.Controls
{
    public partial class TabView
    {
        private void UpdateValuesToMaterial3()
        {
            iconSize = new Size(24, 24);
            iconMargin = new Thickness(0, 0, 0, 15);
            buttonPadding = new Thickness(0, 52, 0, 0);
            tabViewHeight = 76;
            realMinimalItemWidth = 64;
            fontSize = 12;
            buttonTextTransform = TextTransform.None;
        }

        private void UpdateDrawableToMaterial3()
        {
            if (graphicsView.Drawable is not Material3Drawable drawable)
                graphicsView.Drawable = drawable = new Material3Drawable();

            drawable.PillBrush = PrimaryBrush;
            drawable.ScrollPosition = scrollPosition;
            drawable.Items = stackLayout.Children;
        }

        private class Material3Drawable : IDrawable
        {
            private double pillHeight = 32;
            private double pillWidth = 64;

            public double ScrollPosition { get; set; }
            public IEnumerable<IView> Items { get; set; }
            public Brush PillBrush { get; set; }
            public LayoutOptions Alignment { get; set; }

            public void Draw(ICanvas canvas, RectF dirtyRect)
            {
            }
        }
    }
}
