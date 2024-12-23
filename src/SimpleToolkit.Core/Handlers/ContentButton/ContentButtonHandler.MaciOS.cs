﻿using SimpleToolkit.Core.Platform;
using UIKit;
using PlatformView = Microsoft.Maui.Platform.ContentView;

namespace SimpleToolkit.Core.Handlers;

public partial class ContentButtonHandler
{
    private bool alreadyReleased = true;

    protected override PlatformView CreatePlatformView()
    {
        _ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a {nameof(ButtonContentView)}");
        _ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} cannot be null");

        var buttonPlatformView = new ButtonContentView
        {
            CrossPlatformLayout = VirtualView,
            AccessibilityTraits = UIAccessibilityTrait.Button
        };
        buttonPlatformView.AddGestureRecognizer(new UITapGestureRecognizer(OnViewTapped));

        return buttonPlatformView;
    }

    protected override void ConnectHandler(PlatformView platformView)
    {
        base.ConnectHandler(platformView);

        if (platformView is not ButtonContentView buttonContentView)
            return;

        buttonContentView.BeganTouching += OnBeganTouching;
        buttonContentView.EndedTouching += OnEndedTouching;
        buttonContentView.MovedTouching += OnMovedTouching;
        buttonContentView.CancelledTouching += OnEndedTouching;
    }

    protected override void DisconnectHandler(PlatformView platformView)
    {
        base.DisconnectHandler(platformView);

        if (platformView is not ButtonContentView buttonContentView)
            return;

        buttonContentView.BeganTouching -= OnBeganTouching;
        buttonContentView.EndedTouching -= OnEndedTouching;
        buttonContentView.MovedTouching -= OnMovedTouching;
        buttonContentView.CancelledTouching -= OnEndedTouching;
    }

    private void OnBeganTouching(object? sender, ContentButtonEventArgs e)
    {
        alreadyReleased = false;

        VirtualView.OnPressed(e.InteractionPosition);
    }

    private void OnEndedTouching(object? sender, ContentButtonEventArgs e)
    {
        if (!alreadyReleased)
            VirtualView.OnReleased(e.InteractionPosition);

        alreadyReleased = true;
    }

    private void OnMovedTouching(object? sender, ContentButtonEventArgs e)
    {
        if (!alreadyReleased && sender is ButtonContentView button && !button.Bounds.Contains(e.InteractionPosition.X, e.InteractionPosition.Y))
        {
            VirtualView.OnReleased(e.InteractionPosition);
            alreadyReleased = true;
        }
    }

    private void OnViewTapped(UITapGestureRecognizer g)
    {
        var location = g.LocationInView(g.View);

        switch (g.State)
        {
            case UIGestureRecognizerState.Ended:
                VirtualView.OnPressed(new Point(location.X, location.Y));
                VirtualView.OnReleased(new Point(location.X, location.Y));
                VirtualView.OnClicked();
                break;
            case UIGestureRecognizerState.Failed:
                VirtualView.OnPressed(new Point(location.X, location.Y));
                VirtualView.OnReleased(new Point(location.X, location.Y));
                break;
        }
    }
}