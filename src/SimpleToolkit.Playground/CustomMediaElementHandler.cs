#if IOS || MACCATALYST

using System;
using AVKit;
using CommunityToolkit.Maui.Core.Handlers;
using CommunityToolkit.Maui.Core.Views;
using GameController;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using SimpleToolkit.SimpleShell.Handlers;
using UIKit;

namespace SimpleToolkit.SimpleShell.Playground;

public class CustomMediaElementHandler : MediaElementHandler
{
    public CustomMediaElementHandler() : base()
    {
    }

    public CustomMediaElementHandler(IPropertyMapper mapper, CommandMapper commandMapper)
        : base(mapper, commandMapper)
    {
    }


    protected override MauiMediaElement CreatePlatformView()
    {
        if (MauiContext is null)
        {
            throw new InvalidOperationException($"{nameof(MauiContext)} cannot be null");
        }

        mediaManager ??= new(MauiContext,
                                VirtualView);

        // Retrieve the parenting page so we can provide that to the platform control
        var parent = VirtualView.Parent;
        while (parent is not null)
        {
            if (parent is Page)
            {
                break;
            }

            parent = parent.Parent;
        }

        var parentPage = (parent as Page)?.ToHandler(MauiContext);

        var (_, playerViewController) = mediaManager.CreatePlatformView();

        // It looks like that parent view controller has to be a controller that is currently presented in a UINavigationController
        //var view = new CustomMauiMediaElement(playerViewController, VirtualView);
        var view = new CustomMauiMediaElement(playerViewController, VirtualView);

        return view;
    }


    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        PlatformView.Dispose();
    }
}

public class CustomMauiMediaElement : MauiMediaElement
{
    AVPlayerViewController playerViewController = null;
    Element virtualView = null;
    Element rootElement = null;


    public CustomMauiMediaElement(AVPlayerViewController playerViewController, Element virtualView) : base(playerViewController, null)
    {
        ArgumentNullException.ThrowIfNull(playerViewController.View);

        this.playerViewController = playerViewController;
        this.virtualView = virtualView;

        // Null original implementation
        RemoveFromParents();

        var controller = FindParentController(virtualView);
        AddToView(controller);

        if (controller is null)
        {
            // If no ViewController was found, try to wait for adding the view to a parent
            // This is needed when MediaElement is part of a cell in CollectionView or CarouselView
            rootElement = FindRootElement(virtualView);
            rootElement.ParentChanging += ElementParentChanging;
        }
    }


    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (rootElement is not null)
            rootElement.ParentChanging -= ElementParentChanging;

        playerViewController = null;
        virtualView = null;
        rootElement = null;
    }

    private void AddToView(UIViewController viewController)
    {
        RemoveFromParents();

        playerViewController.View.Frame = Bounds;

#if IOS16_0_OR_GREATER || MACCATALYST16_1_OR_GREATER
        // On iOS 16+ and macOS 13+ the AVPlayerViewController has to be added to a parent ViewController, otherwise the transport controls won't be displayed.

        viewController ??= WindowStateManager.Default.GetCurrentUIViewController();

        if (viewController?.View is not null)
        {
            // Zero out the safe area insets of the AVPlayerViewController
            UIEdgeInsets insets = viewController.View.SafeAreaInsets;
            playerViewController.AdditionalSafeAreaInsets =
                new UIEdgeInsets(insets.Top * -1, insets.Left, insets.Bottom * -1, insets.Right);
            // Add the View from the AVPlayerViewController to the parent ViewController
            viewController.AddChildViewController(playerViewController);
            viewController.View.AddSubview(playerViewController.View);

            playerViewController.DidMoveToParentViewController(viewController);
        }
#endif

        AddSubview(playerViewController.View);
    }

    private void RemoveFromParents()
    {
        foreach (var subview in Subviews)
            subview.RemoveFromSuperview();

        playerViewController.RemoveFromParentViewController();
        playerViewController.View.RemoveFromSuperview();
    }

    private UIViewController FindParentController(Element element)
    {
        if (element is null)
            return null;

        // Can this element have a ViewController?
        // If not, try to find the controller in the parent element
        if (element.Handler?.PlatformView is not UIView view)
            return FindParentController(element.Parent);

        // Does this element have a ViewController?
        UIViewController controller = null;

        if (element.Handler is PageHandler pageHandler)
            controller = pageHandler.ViewController;

        if (view.NextResponder is UIViewController viewController)
            controller = viewController;

        // Is it the right ViewController?
        if (controller is not null)
        {
            if (controller is UICollectionViewController)
                return controller;

            if (controller is UINavigationController navigationController)
                return navigationController.VisibleViewController;
        }

        // Try to find the controller in the parent element
        if (element.Parent is not null)
            return FindParentController(element.Parent);

        return null;
    }

    private static Element FindRootElement(Element element)
    {
        while (element.Parent is not null)
            element = element.Parent;

        return element;
    }

    private void ElementParentChanging(object sender, ParentChangingEventArgs e)
    {
        var element = sender as Element;
        var controller = FindParentController(e.NewParent);

        AddToView(controller);

        element.ParentChanging -= ElementParentChanging;
        rootElement = null;
    }
}

#endif