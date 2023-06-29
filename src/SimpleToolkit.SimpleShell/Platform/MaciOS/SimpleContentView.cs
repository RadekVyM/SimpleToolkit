#if IOS || MACCATALYST

namespace SimpleToolkit.SimpleShell.Platform
{
    // TODO: Clipping to the Border shape does not work
    public class SimpleContentView : Microsoft.Maui.Platform.ContentView
    {
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            foreach (var subview in Subviews)
            {
                // Only resize a subview when it does not match the size of the parent
                if (subview.Bounds.Width != Bounds.Width || subview.Bounds.Height != Bounds.Height)
                {
                    subview.Frame = new CoreGraphics.CGRect(subview.Frame.X, subview.Frame.Y, Bounds.Width, Bounds.Height);
                }
            }
        }
    }
}

#endif