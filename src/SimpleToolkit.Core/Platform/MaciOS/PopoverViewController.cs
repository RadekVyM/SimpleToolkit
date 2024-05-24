#if IOS || MACCATALYST

using CoreGraphics;
using Foundation;
using Microsoft.Maui.Platform;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using UIKit;
using ContentView = Microsoft.Maui.Controls.ContentView;

// Partially based on the .NET MAUI Community Toolkit Popup control (Thanks!):
// https://github.com/CommunityToolkit/Maui

namespace SimpleToolkit.Core.Platform;

public class PopoverViewController(IMauiContext mauiContext) : UIViewController
{
    private readonly IMauiContext mauiContext = mauiContext ?? throw new ArgumentNullException(nameof(mauiContext));
    private ContentView contentWrapper = null;
    private WeakReference<IPopover> virtualViewReference;

    internal UIViewController ViewController { get; private set; }

    // See https://github.com/dotnet/maui/pull/14108
    public IPopover VirtualView
    {
        get => virtualViewReference is not null && virtualViewReference.TryGetTarget(out var v) ? v : null;
        private set => virtualViewReference = value is null ? null : new(value);
    }

    public override void ViewWillAppear(bool animated)
    {
        base.ViewWillAppear(animated);

        if (View.Superview is null)
            return;

        // Removes the default corner radius of the popover
		View.Superview.Layer.CornerRadius = 0f;
		View.Superview.Layer.MasksToBounds = false;
    }

    public override void ViewDidLayoutSubviews()
    {
        base.ViewDidLayoutSubviews();

        if (VirtualView?.Content is null)
            return;

        if (PresentationController is not null)
            RemoveShadow(PresentationController.ContainerView);

        var measure = (contentWrapper as IView).Measure(double.PositiveInfinity, double.PositiveInfinity);
        PreferredContentSize = new CGSize(measure.Width, measure.Height);

        foreach (var subview in View.Subviews)
            subview.SizeToFit();
    }

    [MemberNotNull(nameof(VirtualView), nameof(ViewController))]
    public void SetElement(IPopover element)
    {
        VirtualView = element;
        ModalPresentationStyle = UIModalPresentationStyle.Popover;

        _ = View ?? throw new InvalidOperationException($"{nameof(View)} cannot be null");
        _ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} cannot be null.");

        var rootViewController = WindowStateManager.Default.GetCurrentUIViewController();
        ViewController ??= rootViewController;
    }

    public void CleanUp()
    {
        if (VirtualView is null)
            return;

        VirtualView = null;

        View.ClearSubviews();

        if (PresentationController is UIPopoverPresentationController presentationController)
            presentationController.Delegate = null;
    }

    [MemberNotNull(nameof(ViewController))]
    public void InitializeView(in IPopover virtualView, in IElement anchor)
    {
        SetUpPresentationController();
        UpdateContent(virtualView, View);

        _ = ViewController ?? throw new InvalidOperationException($"{nameof(ViewController)} cannot be null");
        PresentInViewController(ViewController);

        SetLayout(virtualView, anchor);
    }

    public void SetLayout(IPopover popover, IElement anchor)
    {
        if (View is null)
            return;

        var view = anchor.ToPlatform(popover.Handler?.MauiContext ?? throw new NullReferenceException());
        PopoverPresentationController.SourceView = view;
        PopoverPresentationController.SourceRect = view.Bounds;
    }

    public void UpdateContent() => UpdateContent(VirtualView, View);

    private void UpdateContent(IPopover virtualView, UIView containerView)
    {
        _ = View ?? throw new InvalidOperationException($"{nameof(View)} cannot be null");
        
        UpdateContentWrapper(virtualView);
        AddContentToView(containerView);
    }

    private void UpdateContentWrapper(IPopover virtualView)
    {
        if (contentWrapper is not null)
            contentWrapper.Content = null;

        // This ContentView wrapper ensures that everything is sized correctly
        contentWrapper = new ContentView
        {
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Start,
            Content = virtualView.Content
        };
    }

    private void AddContentToView(UIView containerView)
    {
        containerView.ClearSubviews();
        var wrapperView = contentWrapper?.ToPlatform(mauiContext);

        if (wrapperView is not null)
            containerView.AddSubview(wrapperView);
    }

    private void SetUpPresentationController()
    {
        var presentationController = (UIPopoverPresentationController)PresentationController;

        presentationController.SourceView = ViewController?.View ?? throw new InvalidOperationException($"{nameof(ViewController.View)} cannot be null");
        presentationController.Delegate = new PopoverDelegate();
        presentationController.BackgroundColor = Colors.Transparent.ToPlatform();
        presentationController.PopoverBackgroundViewType = typeof(PopoverBackgroundView);
        presentationController.PermittedArrowDirections = UIPopoverArrowDirection.Up; // Because of this the popover is bellow the anchor
    }

    private void PresentInViewController(UIViewController viewController)
    {
        if (!IsBeingPresented && !IsBeingDismissed)
            viewController.PresentViewController(this, true, null);
    }

    // Inspired by (Thanks!):
    // https://github.com/CommunityToolkit/Maui/blob/main/src/CommunityToolkit.Maui.Core/Views/Popup/MauiPopup.macios.cs#L156
    private static void RemoveShadow(UIView containerView)
    {
        if (containerView?.Class?.Name is "_UICutoutShadowView")
            containerView?.RemoveFromSuperview();

        foreach (var view in containerView?.Subviews)
            RemoveShadow(view);
    }

    private class PopoverDelegate : UIPopoverPresentationControllerDelegate
    {
        public override UIModalPresentationStyle GetAdaptivePresentationStyle(UIPresentationController forPresentationController) =>
            UIModalPresentationStyle.None;

        public override UIModalPresentationStyle GetAdaptivePresentationStyle(UIPresentationController forPresentationController, UITraitCollection traitCollection) =>
            UIModalPresentationStyle.None;
    }

    // Helps to remove default styling of the popover container
    // Based on (Thanks!):
    // https://github.com/CommunityToolkit/Maui/blob/main/src/CommunityToolkit.Maui.Core/Views/Popup/PopupExtensions.macios.cs#L221
    private class PopoverBackgroundView : UIPopoverBackgroundView
    {
        public PopoverBackgroundView(IntPtr handle) : base(handle)
        {
            BackgroundColor = Colors.Transparent.ToPlatform();
            Alpha = 0.0f;
        }

        public override NFloat ArrowOffset { get; set; }

        public override UIPopoverArrowDirection ArrowDirection { get; set; }

        [Export("arrowHeight")]
        static new float GetArrowHeight() => 0f;

        [Export("arrowBase")]
        static new float GetArrowBase() => 0f;

        [Export("contentViewInsets")]
        static new UIEdgeInsets GetContentViewInsets() => UIEdgeInsets.Zero;
    }
}

#endif