using Android.Views;
using Android.Views.Accessibility;
using static Android.Views.View;
using PlatformView = Microsoft.Maui.Platform.ContentViewGroup;

namespace SimpleToolkit.Core.Handlers;

public partial class ContentButtonHandler
{
    private Rect viewTapRect;
    private bool alreadyReleased;

    protected override PlatformView CreatePlatformView()
    {
        var platformView = base.CreatePlatformView();

        platformView.SetAccessibilityDelegate(new ButtonAccessibilityDelegate());
        platformView.Clickable = true;

        return platformView;
    }

    protected override void ConnectHandler(PlatformView platformView)
    {
        platformView.Touch += OnPlatformViewTouch;
        platformView.Click += OnPlatformViewClick;

        base.ConnectHandler(platformView);
    }

    protected override void DisconnectHandler(PlatformView platformView)
    {
        platformView.Touch -= OnPlatformViewTouch;
        platformView.Click -= OnPlatformViewClick;

        base.DisconnectHandler(platformView);
    }

    private void OnPlatformViewClick(object? sender, EventArgs e)
    {
        if (sender is not Android.Views.View view)
            return;

        var position = new Point(view.Width / 2f, view.Height / 2f);

        VirtualView.OnPressed(position);
        VirtualView.OnReleased(position);
        VirtualView.OnClicked();
    }

    private void OnPlatformViewTouch(object? sender, Android.Views.View.TouchEventArgs e)
    {
        if (sender is not Android.Views.View view || e.Event is not {} touchEvent)
            return;

        var density = DeviceDisplay.Current.MainDisplayInfo.Density;
        var position = new Point(touchEvent.GetX() / density, touchEvent.GetY() / density);

        switch (touchEvent.Action)
        {
            case MotionEventActions.Down:
                viewTapRect = new Rect(view.Left, view.Top, view.Right, view.Bottom);
                alreadyReleased = false;
                VirtualView.OnPressed(position);
                break;
            case MotionEventActions.Up:
                if (IsTouchInViewTapRect())
                {
                    if (!alreadyReleased)
                        VirtualView.OnReleased(position);
                    VirtualView.OnClicked();
                    alreadyReleased = true;
                }
                else
                {
                    if (!alreadyReleased)
                        VirtualView.OnReleased(position);
                    alreadyReleased = true;
                }
                break;
            case MotionEventActions.Cancel:
                if (!alreadyReleased)
                    VirtualView.OnReleased(position);
                alreadyReleased = true;
                break;
            case MotionEventActions.Move:
                if (!IsTouchInViewTapRect())
                {
                    if (!alreadyReleased)
                        VirtualView.OnReleased(position);
                    alreadyReleased = true;
                }
                break;
        }

        bool IsTouchInViewTapRect()
        {
            return viewTapRect.Contains(view.Left + touchEvent.GetX(), view.Top + touchEvent.GetY());
        }
    }

    private class ButtonAccessibilityDelegate : AccessibilityDelegate
    {
        public override void OnInitializeAccessibilityNodeInfo(Android.Views.View host, AccessibilityNodeInfo info)
        {
            base.OnInitializeAccessibilityNodeInfo(host, info);

            if (info is null)
                return;

            info.ClassName = "android.widget.Button";
            info.Clickable = true;
        }
    }
}