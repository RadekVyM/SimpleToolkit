﻿using CoreGraphics;
using Foundation;
using Microsoft.Maui.Platform;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using UIKit;

// Partially based on the .NET MAUI Community Toolkit Popup control (Thanks!):
// https://github.com/CommunityToolkit/Maui

namespace SimpleToolkit.Core.Platform;

public class PopoverViewController(IMauiContext mauiContext) : UIViewController
{
    private readonly IMauiContext mauiContext = mauiContext ?? throw new ArgumentNullException(nameof(mauiContext));
    private Grid? contentWrapper = null;
    private bool isAnimated = true;
    private WeakReference<IPopover>? virtualViewReference;

    internal UIViewController? ViewController { get; private set; }

    // See https://github.com/dotnet/maui/pull/14108
    public IPopover? VirtualView
    {
        get => virtualViewReference is not null && virtualViewReference.TryGetTarget(out var v) ? v : null;
        private set => virtualViewReference = value is null ? null : new(value);
    }

    public virtual UIPopoverArrowDirection PermittedArrowDirections 
    {
        get => (PresentationController as UIPopoverPresentationController)?.PermittedArrowDirections ?? UIPopoverArrowDirection.Any;
        set
        {
            if (PresentationController is UIPopoverPresentationController popoverPresentationController)
                popoverPresentationController.PermittedArrowDirections = value;
        }
    }

    public virtual bool IsAnimated 
    {
        get => isAnimated;
        set
        {
            isAnimated = value;
            if (PresentationController?.Delegate is PopoverDelegate popoverDelegate)
                popoverDelegate.IsAnimated = value;
        }
    }


    public override void ViewWillAppear(bool animated)
    {
        base.ViewWillAppear(animated);

        if (View?.Superview is null)
            return;

        if (VirtualView?.UseDefaultStyling is not true)
        {
            // Removes the default corner radius of the popover
            View.Superview.Layer.CornerRadius = 0f;
            View.Superview.Layer.MasksToBounds = false;

            if (IsAnimated)
                AnimateIn();
        }
    }

    public override void ViewDidLayoutSubviews()
    {
        base.ViewDidLayoutSubviews();

        if (VirtualView?.Content is null)
            return;

        if (!VirtualView.UseDefaultStyling && PresentationController is not null)
            RemoveShadow(PresentationController.ContainerView);

        var measure = (contentWrapper as IView)?.Measure(double.PositiveInfinity, double.PositiveInfinity);
        if (measure is {} notNullMeasure)
            PreferredContentSize = new CGSize(notNullMeasure.Width, notNullMeasure.Height);

        foreach (var subview in View?.Subviews ?? [])
        {
            subview.SizeToFit();
            // Make sure that the content is properly offset when arrow is displayed
            subview.Frame = new CGRect(GetContentOffset(VirtualView.UseDefaultStyling), subview.Frame.Size);
        }
    }

    public void SetElement(IPopover element)
    {
        VirtualView = element;
        ModalPresentationStyle = UIModalPresentationStyle.Popover;

        _ = View ?? throw new InvalidOperationException($"{nameof(View)} cannot be null");
        _ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} cannot be null.");

        var rootViewController = WindowStateManager.Default.GetCurrentUIViewController();
        ViewController ??= rootViewController;
    }

    public async Task Show(IPopover virtualView, IElement anchor)
    {
        if (IsBeingPresented || IsBeingDismissed)
            return;

        SetUpPresentationController(virtualView);
        UpdateContent(virtualView, View ?? throw new NullReferenceException("UIViewController's View should not be null here."));

        _ = ViewController ?? throw new NullReferenceException($"{nameof(ViewController)} cannot be null");

        SetAnchor(virtualView, anchor);
        await PresentInViewController(ViewController);
    }

    public async Task Hide()
    {
        _ = ViewController ?? throw new NullReferenceException($"{nameof(ViewController)} cannot be null");
        await ViewController.DismissViewControllerAsync(IsAnimated);
    }

    public void CleanUp()
    {
        if (VirtualView is null)
            return;

        VirtualView = null;

        View?.ClearSubviews();

        if (PresentationController is UIPopoverPresentationController presentationController)
            presentationController.Delegate = null;
    }

    public void UpdateContent() => UpdateContent(
        VirtualView ?? throw new NullReferenceException("VirtualView should not be null here."),
        View ?? throw new NullReferenceException("UIViewController's View should not be null here."));

    protected virtual void AnimateIn()
    {
        _ = View ?? throw new NullReferenceException("UIViewController's View should not be null here.");
        _ = PresentationController ?? throw new NullReferenceException("PresentationController should not be null here.");

        var arrowDirection = ((UIPopoverPresentationController)PresentationController).ArrowDirection;
        var x = arrowDirection switch
        {
            UIPopoverArrowDirection.Left => 0,
            UIPopoverArrowDirection.Right => 1,
            _ => 0.5f
        };
        var y = arrowDirection switch
        {
            UIPopoverArrowDirection.Up => 0,
            UIPopoverArrowDirection.Down => 1,
            _ => 0.5f
        };
        var oldAnchorPoint = View.Layer.AnchorPoint;
        
        View.Layer.AnchorPoint = new CGPoint(x, y);
        View.Transform = CGAffineTransform.MakeScale(0.4f, 0.4f);
        View.Alpha = 0;

        UIView.Animate(0.18, 0, UIViewAnimationOptions.CurveEaseInOut, () =>
        {
            View.Transform = CGAffineTransform.MakeIdentity();
            View.Alpha = 1;
        }, null!);
    }

    private void UpdateContent(IPopover virtualView, UIView containerView)
    {
        _ = View ?? throw new NullReferenceException($"{nameof(View)} cannot be null");
        
        UpdateContentWrapper(virtualView);
        AddContentToView(containerView);
    }

    private void UpdateContentWrapper(IPopover virtualView)
    {
        contentWrapper?.Children.Clear();

        // This Grid wrapper ensures that everything is sized correctly
        contentWrapper = new Grid
        {
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Start,
            RowDefinitions = new RowDefinitionCollection(new RowDefinition(GridLength.Auto)),
            ColumnDefinitions = new ColumnDefinitionCollection(new ColumnDefinition(GridLength.Auto)),
        };

        contentWrapper.Add(virtualView.Content);
    }

    private void AddContentToView(UIView containerView)
    {
        containerView.ClearSubviews();
        var wrapperView = contentWrapper?.ToPlatform(mauiContext);

        if (wrapperView is not null)
            containerView.AddSubview(wrapperView);
    }

    private void SetAnchor(IPopover popover, IElement anchor)
    {
        if (View is null || PopoverPresentationController is null)
            return;

        var anchorView = anchor.ToPlatform(popover.Handler?.MauiContext ?? throw new NullReferenceException("MauiContext should not be null here."));
        PopoverPresentationController.SourceView = anchorView;
        PopoverPresentationController.SourceRect = anchorView.Bounds;
    }

    private void SetUpPresentationController(IPopover virtualView)
    {
        _ = PresentationController ?? throw new NullReferenceException("PresentationController should not be null here.");
        var presentationController = (UIPopoverPresentationController)PresentationController;
        
        presentationController.Delegate = new PopoverDelegate
        {
            IsAnimated = IsAnimated
        };
        presentationController.PermittedArrowDirections = virtualView.PermittedArrowDirections.ToUIPopoverArrowDirection();
        
        if (!virtualView.UseDefaultStyling)
        {
            presentationController.BackgroundColor = Colors.Transparent.ToPlatform();
            presentationController.PopoverBackgroundViewType = typeof(PopoverBackgroundView);
        }
    }

    private async Task PresentInViewController(UIViewController viewController)
    {
        await viewController.PresentViewControllerAsync(this, IsAnimated);
    }

    private CGPoint GetContentOffset(bool useDefaultStyling)
    {
        if (!useDefaultStyling)
            return new CGPoint(0, 0);

        _ = PresentationController ?? throw new NullReferenceException("PresentationController should not be null here.");

        const float arrowSize = 13;
        var presentationController = (UIPopoverPresentationController)PresentationController;
        var arrowDirection = presentationController.ArrowDirection;

        return arrowDirection switch
        {
            UIPopoverArrowDirection.Up => new CGPoint(0, arrowSize),
            UIPopoverArrowDirection.Left => new CGPoint(arrowSize, 0),
            _ => new CGPoint(0, 0)
        };
    }

    // Inspired by (Thanks!):
    // https://github.com/CommunityToolkit/Maui/blob/main/src/CommunityToolkit.Maui.Core/Views/Popup/MauiPopup.macios.cs#L156
    private static void RemoveShadow(UIView containerView)
    {
        if (containerView?.Class?.Name is "_UICutoutShadowView")
            containerView?.RemoveFromSuperview();

        foreach (var view in containerView?.Subviews ?? [])
            RemoveShadow(view);
    }

    private class PopoverDelegate : UIPopoverPresentationControllerDelegate
    {
        public bool IsAnimated { get; set; } = true;

        public override bool ShouldDismiss(UIPresentationController presentationController)
        {
            presentationController.PresentingViewController.DismissViewController(IsAnimated, null);
            return true;
        }

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