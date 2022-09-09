#if IOS || MACCATALYST

using Foundation;
using Microsoft.Maui.Handlers;
using UIKit;

namespace SimpleToolkit.Core.Handlers
{
    public partial class ContentButtonHandler : ContentViewHandler
    {
        private IContentButton virtualView => VirtualView as IContentButton;

        protected override Microsoft.Maui.Platform.ContentView CreatePlatformView()
        {
            var platformView = base.CreatePlatformView();

            platformView.AccessibilityTraits = UIAccessibilityTrait.Button;

            //platformView.AddGestureRecognizer(new UICustomGestureRecognizer(g =>
            //{
            //    var location = g.LocationInView(g.View);

            //    switch (g.State)
            //    {
            //        case UIGestureRecognizerState.Began:
            //            virtualView.OnPressed(new Point(location.X, location.Y));
            //            break;
            //        case UIGestureRecognizerState.Ended:
            //            virtualView.OnReleased(new Point(location.X, location.Y));
            //            virtualView.OnClicked();
            //            break;
            //        case UIGestureRecognizerState.Failed:
            //            virtualView.OnReleased(new Point(location.X, location.Y));
            //            break;
            //    }
            //}));

            platformView.AddGestureRecognizer(new UITapGestureRecognizer(g =>
            {
                var location = g.LocationInView(g.View);

                switch (g.State)
                {
                    case UIGestureRecognizerState.Ended:
                        virtualView.OnPressed(new Point(location.X, location.Y));
                        virtualView.OnReleased(new Point(location.X, location.Y));
                        virtualView.OnClicked();
                        break;
                    case UIGestureRecognizerState.Failed:
                        virtualView.OnPressed(new Point(location.X, location.Y));
                        virtualView.OnReleased(new Point(location.X, location.Y));
                        break;
                }
            }));

            return platformView;
        }

        // TODO: Find out why my gesture recognizer conflicts with scrolling in ScrollView
        protected class UICustomGestureRecognizer : UITapGestureRecognizer
        {
            private Action<UICustomGestureRecognizer> action;

            public UICustomGestureRecognizer(Action<UICustomGestureRecognizer> value)
            {
                this.action = value;
            }

            public override void TouchesBegan(NSSet touches, UIEvent evt)
            {
                base.TouchesBegan(touches, evt);

                State = UIGestureRecognizerState.Began;

                action?.Invoke(this);
            }

            public override void TouchesEnded(NSSet touches, UIEvent evt)
            {
                base.TouchesEnded(touches, evt);

                State = UIGestureRecognizerState.Ended;

                action?.Invoke(this);

                State = UIGestureRecognizerState.Possible;
            }

            public override void TouchesMoved(NSSet touches, UIEvent evt)
            {
                base.TouchesMoved(touches, evt);

                UITouch touch = touches.AnyObject as UITouch;

                if (touch is not null && !View.Frame.Contains(touch.LocationInView(View)))
                {
                    State = UIGestureRecognizerState.Cancelled;
                }

                action?.Invoke(this);
            }

            public override void TouchesCancelled(NSSet touches, UIEvent evt)
            {
                base.TouchesCancelled(touches, evt);

                State = UIGestureRecognizerState.Cancelled;

                action?.Invoke(this);
            }

            public override void PressesBegan(NSSet<UIPress> presses, UIPressesEvent evt)
            {
                base.PressesBegan(presses, evt);

                State = UIGestureRecognizerState.Began;

                action?.Invoke(this);
            }

            public override void PressesEnded(NSSet<UIPress> presses, UIPressesEvent evt)
            {
                base.PressesEnded(presses, evt);

                State = UIGestureRecognizerState.Ended;

                action?.Invoke(this);
            }

            public override void PressesCancelled(NSSet<UIPress> presses, UIPressesEvent evt)
            {
                base.PressesCancelled(presses, evt);

                State = UIGestureRecognizerState.Cancelled;

                action?.Invoke(this);
            }
        }
    }
}


#endif