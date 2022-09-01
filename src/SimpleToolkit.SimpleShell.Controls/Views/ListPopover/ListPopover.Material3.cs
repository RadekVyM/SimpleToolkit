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
        }

        private class Material3Drawable : IDrawable
        {
            public void Draw(ICanvas canvas, RectF dirtyRect)
            {
                canvas.SaveState();

                canvas.RestoreState();
            }
        }
    }
}
