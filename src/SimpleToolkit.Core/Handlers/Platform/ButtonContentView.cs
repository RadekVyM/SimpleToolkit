#if IOS || MACCATALYST

using CoreGraphics;
using Foundation;
using Microsoft.Maui.Platform;
using UIKit;

namespace SimpleToolkit.Core.Handlers.Platform
{
    public class ButtonContentView : Microsoft.Maui.Platform.ContentView
    {
        public Func<double, double, Size> CrossPlatformMeasure { get; set; }
        public Func<Rect, Size> CrossPlatformArrange { get; set; }

        public event EventHandler<ContentButtonEventArgs> BeganTouching;
        public event EventHandler<ContentButtonEventArgs> EndedTouching;
        public event EventHandler<ContentButtonEventArgs> CancelledTouching;
        public event EventHandler<ContentButtonEventArgs> MovedTouching;

        public override CGSize SizeThatFits(CGSize size)
        {
            if (CrossPlatformMeasure == null)
            {
                return base.SizeThatFits(size);
            }

            var width = size.Width;
            var height = size.Height;

            var crossPlatformSize = CrossPlatformMeasure(width, height);

            return crossPlatformSize.ToCGSize();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var bounds = AdjustForSafeArea(Bounds).ToRectangle();

            CrossPlatformMeasure?.Invoke(bounds.Width, bounds.Height);
            CrossPlatformArrange?.Invoke(bounds);
        }

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
}

#endif