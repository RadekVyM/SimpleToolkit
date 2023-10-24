namespace SimpleToolkit.SimpleShell.Controls.Extensions;

internal static class CanvasExtensions
{
    public static void FillVerticalMoreIcon(this ICanvas canvas, RectF rect, float dotRadius)
    {
        int numberOfDots = 3;
        float height = dotRadius * 2;
        float left = (rect.Width / 2f) - (dotRadius / 2f);
        float top = 0;

        for (int i = 0; i < numberOfDots; i++)
        {
            canvas.FillRoundedRectangle(rect.Left + left, rect.Top + top, height, height, dotRadius);
            top += height + ((rect.Height - (3f * height)) / 2f);
        }
    }

    public static void FillHorizontalMoreIcon(this ICanvas canvas, RectF rect, float dotRadius)
    {
        HorizontalMoreIcon(canvas, rect, dotRadius, canvas.FillRoundedRectangle);
    }

    public static void DrawHorizontalMoreIcon(this ICanvas canvas, RectF rect, float dotRadius)
    {
        HorizontalMoreIcon(canvas, rect, dotRadius, canvas.DrawRoundedRectangle);
    }

    private static void HorizontalMoreIcon(this ICanvas canvas, RectF rect, float dotRadius, Action<float, float, float, float, float> action)
    {
        int numberOfDots = 3;
        float width = dotRadius * 2;
        float left = 0;
        float top = (rect.Height / 2f) - (dotRadius / 2f);

        for (int i = 0; i < numberOfDots; i++)
        {
            action(rect.Left + left, rect.Top + top, width, width, dotRadius);
            left += width + ((rect.Width - (3f * width)) / 2f);
        }
    }
}