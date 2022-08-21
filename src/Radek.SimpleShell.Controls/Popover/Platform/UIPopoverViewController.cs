#if IOS || MACCATALYST

using CoreGraphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using System.Diagnostics.CodeAnalysis;
using UIKit;

namespace Radek.SimpleShell.Controls.Platform
{
    public class UIPopoverViewController : UIViewController
    {
        readonly IMauiContext mauiContext;

        public UIPopoverViewController(IMauiContext mauiContext)
        {
            this.mauiContext = mauiContext ?? throw new ArgumentNullException(nameof(mauiContext));
        }

        public PageHandler Control { get; private set; }
        public IPopover VirtualView { get; private set; }

        internal UIViewController ViewController { get; private set; }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            //PreferredContentSize = View.SystemLayoutSizeFittingSize(UIView.UILayoutFittingCompressedSize);

            var content = VirtualView.Content;
            var measure = VirtualView.Content.Measure(double.PositiveInfinity, double.PositiveInfinity).Request;
            PreferredContentSize = new CGSize(measure.Width, measure.Height);

            // TODO: Remove corner radius - this does not work:
            View.Superview.Layer.CornerRadius = new System.Runtime.InteropServices.NFloat(0.0f);

            if (Control?.PlatformView is not null)
                Control.PlatformView.SizeThatFits(new Size(View.Bounds.Width, View.Bounds.Height));

            _ = View ?? throw new InvalidOperationException($"{nameof(View)} cannot be null");
        }

        /// <inheritdoc/>
        public override void ViewWillDisappear(bool animated)
        {
            if (ViewController?.View is UIView view)
            {
                view.Alpha = 1f;
            }
            base.ViewWillDisappear(animated);
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
            SetDimmingBackgroundEffect();
        }

        public void CleanUp()
        {
            if (VirtualView is null)
            {
                return;
            }

            VirtualView = null;

            if (PresentationController is UIPopoverPresentationController presentationController)
            {
                presentationController.Delegate = null;
            }
        }

        [MemberNotNull(nameof(Control), nameof(ViewController))]
        public void CreateControl(Func<IPopover, PageHandler> func, in IPopover virtualView, in IElement anchor)
        {
            Control = func(virtualView);

            SetPresentationController();

            _ = View ?? throw new InvalidOperationException($"{nameof(View)} cannot be null");
            SetView(View, Control);

            _ = ViewController ?? throw new InvalidOperationException($"{nameof(ViewController)} cannot be null");
            AddToCurrentPageViewController(ViewController);

            SetLayout(virtualView, anchor);
        }

        public void SetLayout(IPopover popover, IElement anchor)
        {
            if (View is null)
            {
                return;
            }

            var view = anchor.ToPlatform(popover.Handler?.MauiContext ?? throw new NullReferenceException());
            PopoverPresentationController.SourceView = view;
            PopoverPresentationController.SourceRect = view.Bounds;
        }

        void SetDimmingBackgroundEffect()
        {
            if (ViewController?.View is UIView view)
            {
                view.Alpha = 1f;
            }
        }

        void SetView(UIView view, PageHandler control)
        {
            view.AddSubview(control.ViewController?.View ?? throw new InvalidOperationException($"{nameof(control.ViewController.View)} cannot be null"));
            view.Bounds = new(0, 0, PreferredContentSize.Width, PreferredContentSize.Height);
            AddChildViewController(control.ViewController);

            if (VirtualView is not null)
            {
                if (Control is null)
                {
                    return;
                }

                var color = Colors.Transparent.ToPlatform();
                Control.PlatformView.BackgroundColor = color;

                if (Control.ViewController?.View is UIView controlView)
                {
                    controlView.BackgroundColor = color;
                }
            }
        }

        void SetPresentationController()
        {
            var popOverDelegate = new PopoverDelegate();

            var presentationController = ((UIPopoverPresentationController)PresentationController);
            presentationController.SourceView = ViewController?.View ?? throw new InvalidOperationException($"{nameof(ViewController.View)} cannot be null");
            presentationController.Delegate = popOverDelegate;
            //presentationController.PermittedArrowDirections = UIPopoverArrowDirection.Any; // Because of this the popover is above the anchor
            presentationController.PermittedArrowDirections = 0; // Because of this the popover is above the anchor
        }

        void AddToCurrentPageViewController(UIViewController viewController)
        {
            viewController.PresentViewController(this, true, null);
        }

        private class PopoverDelegate : UIPopoverPresentationControllerDelegate
        {
            public override UIModalPresentationStyle GetAdaptivePresentationStyle(UIPresentationController forPresentationController) =>
                UIModalPresentationStyle.None;
        }
    }
}

#endif