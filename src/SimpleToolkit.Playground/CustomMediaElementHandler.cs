#if IOS || MACCATALYST

using System;
using AVKit;
using CommunityToolkit.Maui.Core.Handlers;
using CommunityToolkit.Maui.Core.Views;
using Microsoft.Maui.Platform;
using SimpleToolkit.SimpleShell.Handlers;
using UIKit;

namespace SimpleToolkit.SimpleShell.Playground
{
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

            //var controller = (SimpleShell.Current.Handler as SimpleShellHandler).ContentController;
            //var controller = WindowStateManager.Default.GetCurrentUIViewController();
            //var controller = parentPage.ViewController;
            var controller = GetRootController();

            var view = new MauiMediaElement(playerViewController, controller);

            playerViewController.DidMoveToParentViewController(controller);

            return view;
        }

        private UIViewController GetRootController()
        {
            UIViewController controller = null;
            var window = WindowStateManager.Default.GetCurrentUIWindow();
            if (window != null && window.WindowLevel == UIWindowLevel.Normal)
                controller = window.RootViewController;

            return controller;
        }
    }
}

#endif