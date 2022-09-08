#if ANDROID

using Android.Views;
using Android.Views.Accessibility;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using static Android.Views.View;

namespace SimpleToolkit.Core.Handlers
{
    public partial class ContentButtonHandler : ContentViewHandler
    {
        private Rect viewTapRect;
        private bool alreadyReleased;
        private IContentButton virtualView => VirtualView as IContentButton;

        protected override ContentViewGroup CreatePlatformView()
        {
            var platformView = base.CreatePlatformView();

            platformView.SetAccessibilityDelegate(new ButtonAccessibilityDelegate());
            platformView.Clickable = true;
            var v = platformView.NextFocusDownId;

            platformView.Touch += PlatformViewTouch;
            platformView.Click += PlatformViewClick;

            return platformView;
        }

        private void PlatformViewClick(object sender, EventArgs e)
        {
            if (sender is not Android.Views.View view)
                return;

            var position = new Point(view.Width / 2f, view.Height / 2f);

            virtualView.OnPressed(position);
            virtualView.OnReleased(position);
            virtualView.OnClicked();
        }

        private void PlatformViewTouch(object sender, Android.Views.View.TouchEventArgs e)
        {
            if (sender is not Android.Views.View view)
                return;

            var density = DeviceDisplay.Current.MainDisplayInfo.Density;
            var position = new Point(e.Event.GetX() / density, e.Event.GetY() / density);

            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    viewTapRect = new Rect(view.Left, view.Top, view.Right, view.Bottom);
                    alreadyReleased = false;
                    virtualView.OnPressed(position);
                    break;
                case MotionEventActions.Up:
                    if (IsTouchInViewTapRect())
                    {
                        if (!alreadyReleased)
                            virtualView.OnReleased(position);
                        virtualView.OnClicked();
                        alreadyReleased = true;
                    }
                    else
                    {
                        if (!alreadyReleased)
                            virtualView.OnReleased(position);
                        alreadyReleased = true;
                    }
                    break;
                case MotionEventActions.Cancel:
                    if (!alreadyReleased)
                        virtualView.OnReleased(position);
                    alreadyReleased = true;
                    break;
                case MotionEventActions.Move:
                    if (!IsTouchInViewTapRect())
                    {
                        if (!alreadyReleased)
                            virtualView.OnReleased(position);
                        alreadyReleased = true;
                    }
                    break;
            }

            bool IsTouchInViewTapRect()
            {
                return viewTapRect.Contains(view.Left + e.Event.GetX(), view.Top + e.Event.GetY());
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
}

#endif