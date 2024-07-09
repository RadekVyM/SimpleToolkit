using UIKit;

namespace SimpleToolkit.SimpleShell.Platform;

public class SimpleContentView : UIView
{
    public override void AddSubview(UIView view)
    {
        view.Frame = Bounds;
        view.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
        base.AddSubview(view);
    }
}