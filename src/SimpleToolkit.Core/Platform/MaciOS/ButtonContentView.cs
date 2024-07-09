using CoreGraphics;
using Foundation;
using UIKit;
using PlatformContentView = Microsoft.Maui.Platform.ContentView;

namespace SimpleToolkit.Core.Platform;

public class ButtonContentView : PlatformContentView
{
    public event EventHandler<ContentButtonEventArgs> BeganTouching;
    public event EventHandler<ContentButtonEventArgs> EndedTouching;
    public event EventHandler<ContentButtonEventArgs> CancelledTouching;
    public event EventHandler<ContentButtonEventArgs> MovedTouching;

    public override void SetNeedsLayout()
    {
        base.SetNeedsLayout();
        Superview?.SetNeedsLayout();
    }

    public override void TouchesBegan(NSSet touches, UIEvent evt)
    {
        base.TouchesBegan(touches, evt);
        BeganTouching?.Invoke(this, GetContentButtonEventArgs(touches));
    }

    public override void TouchesEnded(NSSet touches, UIEvent evt)
    {
        base.TouchesEnded(touches, evt);
        EndedTouching?.Invoke(this, GetContentButtonEventArgs(touches));
    }

    public override void TouchesCancelled(NSSet touches, UIEvent evt)
    {
        base.TouchesCancelled(touches, evt);
        CancelledTouching?.Invoke(this, GetContentButtonEventArgs(touches));
    }

    public override void TouchesMoved(NSSet touches, UIEvent evt)
    {
        base.TouchesMoved(touches, evt);
        MovedTouching?.Invoke(this, GetContentButtonEventArgs(touches));
    }

    private ContentButtonEventArgs GetContentButtonEventArgs(NSSet touches)
    {
        return new ContentButtonEventArgs { InteractionPosition = GetPosition(touches) };
    }

    private Point GetPosition(NSSet touches)
    {
        var first = touches.FirstOrDefault() as UITouch;

        var location = first is not null ? first.LocationInView(this) : new CGPoint(0, 0);

        return new Point(location.X, location.Y);
    }
}