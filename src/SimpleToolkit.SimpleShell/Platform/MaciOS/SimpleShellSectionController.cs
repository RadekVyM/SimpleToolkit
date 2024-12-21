using CoreGraphics;
using Foundation;
using UIKit;

namespace SimpleToolkit.SimpleShell.Platform;

public class SimpleShellSectionController : UINavigationController, IUIGestureRecognizerDelegate
{
    private CGPoint startingPoint = default;

    public event EventHandler? PopGestureRecognized;

    public bool ShouldRecognizePopGesture { get; set; } = true;


    public SimpleShellSectionController(UIViewController rootViewController) : base(rootViewController)
    {
        this.NavigationBarHidden = true;
    }


    protected virtual void OnPopGestureRecognized()
    {
        PopGestureRecognized?.Invoke(this, EventArgs.Empty);
    }

    public override void ViewWillLayoutSubviews()
    {
        base.ViewWillLayoutSubviews();

        foreach (var subview in View?.Subviews ?? [])
        {
            // Only resize a subview when it does not match the size of the parent
            if (View is not null && (subview.Bounds.Width != View.Bounds.Width || subview.Bounds.Height != View.Bounds.Height))
            {
                subview.Frame = new CoreGraphics.CGRect(subview.Frame.X, subview.Frame.Y, View.Bounds.Width, View.Bounds.Height);
            }
        }
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        InteractivePopGestureRecognizer.Delegate = this;
        InteractivePopGestureRecognizer.AddTarget((r) =>
        {
            if (r is not UIScreenEdgePanGestureRecognizer recognizer || ViewControllers is null)
                return;

            switch (recognizer.State)
            {
                case UIGestureRecognizerState.Recognized:
                    var point = recognizer.LocationInView(ViewControllers.FirstOrDefault()?.View);

                    if (point.X >= startingPoint.X)
                        OnPopGestureRecognized();
                    break;
                case UIGestureRecognizerState.Began:
                    startingPoint = recognizer.LocationInView(ViewControllers.FirstOrDefault()?.View);
                    break;
            }
        });
    }

    [Export("gestureRecognizerShouldBegin:")]
    public bool ShouldBegin(UIGestureRecognizer recognizer)
    {
        return ShouldRecognizePopGesture;
    }
}