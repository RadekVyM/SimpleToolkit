#if WINDOWS

using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Radek.SimpleShell.Controls.Handlers;
using LayoutAlignment = Microsoft.Maui.Primitives.LayoutAlignment;
using WindowsThickness = Microsoft.UI.Xaml.Thickness;
using XamlStyle = Microsoft.UI.Xaml.Style;

namespace Radek.SimpleShell.Controls.Platform
{
    public class SimpleFlyout : Flyout
    {
        const double defaultSize = 600;
        readonly IMauiContext mauiContext;

        public SimpleFlyout(IMauiContext mauiContext)
        {
            this.mauiContext = mauiContext ?? throw new ArgumentNullException(nameof(mauiContext));
        }

        public IPopover VirtualView { get; private set; }

        internal Panel Control { get; set; }
        internal XamlStyle FlyoutStyle { get; private set; } = new(typeof(FlyoutPresenter));

        Action<Panel> panelCleanUp;
        Func<PopoverHandler, Panel> createControl;

        public void SetElement(IPopover element)
        {
            VirtualView = element;
        }

        public void SetUpPlatformView(Action<Panel> panelCleanUp, Func<PopoverHandler, Panel> createControl)
        {
            ArgumentNullException.ThrowIfNull(panelCleanUp);
            ArgumentNullException.ThrowIfNull(createControl);

            this.panelCleanUp = panelCleanUp;
            this.createControl = createControl;

            CreateControl();
            ConfigureControl();
        }

        public void ConfigureControl()
        {
            if (VirtualView is null)
            {
                return;
            }
            FlyoutStyle = new(typeof(FlyoutPresenter));
            SetFlyoutColor();
            SetLayout();
            ApplyStyles();
        }

        public void Show(IElement anchor)
        {
            if (VirtualView is null)
            {
                return;
            }

            if (anchor is not null)
            {
                var frameworkElement = anchor.ToPlatform(mauiContext);

                SetAttachedFlyout(frameworkElement, this);
                ShowAttachedFlyout(frameworkElement);
            }
            else
            {
                ArgumentNullException.ThrowIfNull(VirtualView.Parent);
                var frameworkElement = VirtualView.Parent.ToPlatform(mauiContext);
                frameworkElement.ContextFlyout = this;
                SetAttachedFlyout(frameworkElement, this);
                ShowAttachedFlyout(frameworkElement);
            }
        }

        public void CleanUp()
        {
            Hide();

            if (Control is not null)
            {
                panelCleanUp?.Invoke(Control);
            }

            VirtualView = null;
            Control = null;
        }

        void CreateControl()
        {
            if (Control is null && VirtualView?.Content is not null && createControl is not null && VirtualView.Handler is PopoverHandler handler)
            {
                Control = createControl(handler);
                Content = Control;
            }
        }

        void SetLayout()
        {
            LightDismissOverlayMode = LightDismissOverlayMode.Off;

            if (VirtualView is not null)
            {
                Placement = FlyoutPlacementMode.Bottom;
            }
        }

        void SetFlyoutColor()
        {
            _ = VirtualView?.Content ?? throw new NullReferenceException(nameof(IPopover.Content));

            FlyoutStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(FlyoutPresenter.BackgroundProperty, Colors.Transparent.ToWindowsColor()));
            FlyoutStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(FlyoutPresenter.IsDefaultShadowEnabledProperty, false));
            FlyoutStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(FlyoutPresenter.PaddingProperty, 0));
            FlyoutStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(FlyoutPresenter.BorderThicknessProperty, new WindowsThickness(0)));
            FlyoutStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(FlyoutPresenter.BorderBrushProperty, Colors.Transparent.ToWindowsColor()));
        }

        void ApplyStyles()
        {
            if (Control is null)
            {
                return;
            }

            FlyoutPresenterStyle = FlyoutStyle;
        }
    }
}

#endif