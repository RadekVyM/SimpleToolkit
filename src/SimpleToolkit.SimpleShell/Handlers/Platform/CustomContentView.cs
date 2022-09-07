#if IOS || MACCATALYST

namespace SimpleToolkit.SimpleShell.Handlers
{
    // TODO: Clipping to the Border shape does not work
    public class CustomContentView : Microsoft.Maui.Platform.ContentView
    {
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            foreach (var subview in Subviews)
            {
                subview.Frame = Bounds;
            }
        }
    }
}

#endif