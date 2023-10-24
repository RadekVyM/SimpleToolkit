#if IOS || MACCATALYST

using CoreAnimation;
using CoreGraphics;
using Foundation;
using Microsoft.Maui.Platform;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using UIKit;

// Partially based on the .NET MAUI Community Toolkit Popup control - https://github.com/CommunityToolkit/Maui

namespace SimpleToolkit.Core.Platform;

public class PopoverViewController : UIViewController
{
    private readonly IMauiContext mauiContext;

    private Grid virtualContentWrapper = null;
    private WeakReference<IPopover> virtualViewReference;

    internal UIViewController ViewController { get; private set; }

    // See https://github.com/dotnet/maui/pull/14108
    public IPopover VirtualView
    {
        get => virtualViewReference is not null && virtualViewReference.TryGetTarget(out var v) ? v : null;
        set => virtualViewReference = value is null ? null : new(value);
    }


    public PopoverViewController(IMauiContext mauiContext)
    {
        this.mauiContext = mauiContext ?? throw new ArgumentNullException(nameof(mauiContext));
    }


    public override void ViewWillAppear(bool animated)
    {
        base.ViewWillAppear(animated);

        if (View.Superview is null)
            return;

        View.Superview.ClipsToBounds = false;
        View.Superview.Layer.CornerRadius = 0f;
        View.Superview.Layer.BackgroundColor = Colors.Transparent.ToCGColor();
        View.Superview.Layer.ShadowColor = null;
        View.Superview.Layer.ShadowOpacity = 0f;
        View.Layer.ShadowColor = null;
        View.Layer.ShadowOpacity = 0f;
    }

    public override void ViewDidLayoutSubviews()
    {
        base.ViewDidLayoutSubviews();

        if (VirtualView?.Content is null)
            return;

        var measure = (virtualContentWrapper as IView).Measure(double.PositiveInfinity, double.PositiveInfinity);
        PreferredContentSize = new CGSize(measure.Width, measure.Height);

        foreach (var subview in View.Subviews)
        {
            subview.SizeToFit();
        }
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
        UpdateVirtualContentWrapper(virtualView);

        SetPresentationController();

        _ = View ?? throw new InvalidOperationException($"{nameof(View)} cannot be null");
        SetView(View);

        _ = ViewController ?? throw new InvalidOperationException($"{nameof(ViewController)} cannot be null");
        AddToCurrentPageViewController(ViewController);

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

    public void UpdateContent()
    {
        UpdateVirtualContentWrapper(VirtualView);
        SetView(View);
    }

    private void UpdateVirtualContentWrapper(IPopover virtualView)
    {
        if (virtualContentWrapper?.Children.Any() == true)
            virtualContentWrapper.Children.Clear();

        // I do not understand how sizing on iOS works. This is the only hopefully working solution I came up with
        virtualContentWrapper = new Grid
        {
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Start,
            RowDefinitions = new RowDefinitionCollection(new RowDefinition(GridLength.Auto)),
            ColumnDefinitions = new ColumnDefinitionCollection(new ColumnDefinition(GridLength.Auto)),
        };

        virtualContentWrapper.Children.Add(virtualView.Content);
    }

    private void SetView(UIView view)
    {
        view.ClearSubviews();
        var subview = virtualContentWrapper?.ToPlatform(mauiContext);

        if (subview is not null)
            view.AddSubview(subview);
    }

    private void SetPresentationController()
    {
        var popOverDelegate = new PopoverDelegate();
        var presentationController = ((UIPopoverPresentationController)PresentationController);

        presentationController.SourceView = ViewController?.View ?? throw new InvalidOperationException($"{nameof(ViewController.View)} cannot be null");
        presentationController.Delegate = popOverDelegate;
        presentationController.PermittedArrowDirections = 0; // Because of this the popover is above the anchor
        presentationController.BackgroundColor = Colors.Transparent.ToPlatform();
        presentationController.PopoverBackgroundViewType = typeof(PopoverBackgroundView);
    }

    private void AddToCurrentPageViewController(UIViewController viewController)
    {
        viewController.PresentViewController(this, true, null);
    }

    private class PopoverDelegate : UIPopoverPresentationControllerDelegate
    {
        public override UIModalPresentationStyle GetAdaptivePresentationStyle(UIPresentationController forPresentationController) =>
            UIModalPresentationStyle.None;
    }

    // Helps to remove default styling of the popover container
    private class PopoverBackgroundView : UIPopoverBackgroundView
    {
        [Export("arrowHeight")]
        static new NFloat GetArrowHeight()
        {
            return 0f;
        }

        [Export("arrowBase")]
        static new NFloat GetArrowBase()
        {
            return 0f;
        }

        [Export("contentViewInsets")]
        static new UIEdgeInsets GetContentViewInsets()
        {
            return UIEdgeInsets.Zero;
        }

        [Export("wantsDefaultContentAppearance")]
        static new bool WantsDefaultContentAppearance
        {
            get => false;
        }

        public override UIPopoverArrowDirection ArrowDirection { get; set; }

        public override NFloat ArrowOffset { get; set; }


        public PopoverBackgroundView(IntPtr handle) : base(handle)
        {
            ArrowOffset = 0f;
            ArrowDirection = 0;
            BackgroundColor = UIColor.Clear;

            Layer.ShadowColor = Colors.Transparent.ToCGColor();
            Layer.ShadowOpacity = 0f;
            Layer.CornerRadius = 0f;
            Layer.BackgroundColor = Colors.Transparent.ToCGColor();
            Layer.MasksToBounds = false;
        }

        public override void DrawLayer(CALayer layer, CGContext context)
        {
            layer.ShadowColor = Colors.Transparent.ToCGColor();
            layer.ShadowOpacity = 0f;
            layer.BackgroundColor = Colors.Transparent.ToCGColor();
            layer.CornerRadius = 0f;
            layer.MasksToBounds = false;

            base.DrawLayer(layer, context);
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
        }
    }
}

#endif