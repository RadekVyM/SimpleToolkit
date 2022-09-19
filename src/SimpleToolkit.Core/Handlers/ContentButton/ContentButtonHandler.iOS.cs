#if IOS || MACCATALYST

using Microsoft.Maui.Handlers;
using SimpleToolkit.Core.Handlers.Platform;
using UIKit;

namespace SimpleToolkit.Core.Handlers
{
    public partial class ContentButtonHandler : ContentViewHandler
    {
        private bool alreadyReleased = true;
        private IContentButton virtualView => VirtualView as IContentButton;

        protected override Microsoft.Maui.Platform.ContentView CreatePlatformView()
        {
            _ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a {nameof(ButtonContentView)}");
            _ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} cannot be null");

            var buttonPlatformView = new ButtonContentView
            {
                CrossPlatformMeasure = VirtualView.CrossPlatformMeasure,
                CrossPlatformArrange = VirtualView.CrossPlatformArrange
            };

            buttonPlatformView.AccessibilityTraits = UIAccessibilityTrait.Button;

            buttonPlatformView.AddGestureRecognizer(new UITapGestureRecognizer(g =>
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

            buttonPlatformView.BeganTouching += OnBeganTouching;
            buttonPlatformView.EndedTouching += OnEndedTouching;
            buttonPlatformView.MovedTouching += OnMovedTouching;
            buttonPlatformView.CancelledTouching += OnEndedTouching;

            return buttonPlatformView;
        }

        private void OnBeganTouching(object sender, ContentButtonEventArgs e)
        {
            alreadyReleased = false;

            virtualView.OnPressed(e.InteractionPosition);
        }

        private void OnEndedTouching(object sender, ContentButtonEventArgs e)
        {
            if (!alreadyReleased)
            {
                virtualView.OnReleased(e.InteractionPosition);
            }

            alreadyReleased = true;
        }

        private void OnMovedTouching(object sender, ContentButtonEventArgs e)
        {
            if (!alreadyReleased && sender is ButtonContentView button && !button.Bounds.Contains(e.InteractionPosition.X, e.InteractionPosition.Y))
            {
                virtualView.OnReleased(e.InteractionPosition);
                alreadyReleased = true;
            }
        }
    }
}


#endif