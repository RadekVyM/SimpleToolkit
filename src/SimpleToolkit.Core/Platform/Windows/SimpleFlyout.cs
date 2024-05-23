#if WINDOWS

using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using SimpleToolkit.Core.Handlers;
using WindowsThickness = Microsoft.UI.Xaml.Thickness;
using WGrid = Microsoft.UI.Xaml.Controls.Grid;
using XamlStyle = Microsoft.UI.Xaml.Style;
using HorizontalAlignment = Microsoft.Maui.Graphics.HorizontalAlignment;

namespace SimpleToolkit.Core.Platform;

public class SimpleFlyout : Flyout
{
    private readonly IMauiContext mauiContext;

    internal WGrid PanelContent => Content as WGrid;
    public IPopover VirtualView { get; private set; }
    
    
    public SimpleFlyout(IMauiContext mauiContext)
    {
        this.mauiContext = mauiContext ?? throw new ArgumentNullException(nameof(mauiContext));
    }


    public void SetElement(IPopover element)
    {
        VirtualView = element ?? throw new ArgumentNullException(nameof(element));
    }

    public void SetUpPlatformView()
    {
        SetContent();
        ConfigureControl();
    }

    public void ConfigureControl()
    {
        if (VirtualView is null)
            return;

        LightDismissOverlayMode = LightDismissOverlayMode.Off;
        ApplyStyles();
    }

    public void Show(IElement anchor)
    {
        if (VirtualView is null)
            return;

        var platformAnchor = anchor?.ToPlatform(mauiContext) ?? GetDefaultAnchor();

        SetPlacement(VirtualView.HorizontalAlignment);
        SetAttachedFlyout(platformAnchor, this);
        ShowAttachedFlyout(platformAnchor);
    }

    public void CleanUp()
    {
        Hide();

        PanelContent?.Children.Clear();
        VirtualView = null;
        Content = null;
    }

    private FrameworkElement GetDefaultAnchor()
    {
        ArgumentNullException.ThrowIfNull(VirtualView.Parent);
        var frameworkElement = VirtualView.Parent.ToPlatform(mauiContext);
        frameworkElement.ContextFlyout = this;

        return frameworkElement;
    }

    private void SetPlacement(HorizontalAlignment horizontalAlignment)
    {
        Placement = horizontalAlignment switch
        {
            HorizontalAlignment.Left => FlyoutPlacementMode.BottomEdgeAlignedLeft,
            HorizontalAlignment.Right => FlyoutPlacementMode.BottomEdgeAlignedRight,
            _ => FlyoutPlacementMode.Bottom
        };
    }

    private void SetContent()
    {
        if (PanelContent is null && VirtualView?.Content is not null && VirtualView.Handler is PopoverHandler handler)
        {
            var content = handler.VirtualView.Content.ToPlatform(handler.MauiContext);
            var grid = new WGrid();
            grid.Children.Add(content);
            Content = grid;
        }
    }

    private XamlStyle CreateFlyoutStyle()
    {
        var flyoutStyle = new XamlStyle(typeof(FlyoutPresenter));

        flyoutStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(FlyoutPresenter.BackgroundProperty, Colors.Transparent.ToWindowsColor()));
        flyoutStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(FlyoutPresenter.IsDefaultShadowEnabledProperty, false));
        flyoutStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(FlyoutPresenter.PaddingProperty, 0));
        flyoutStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(FlyoutPresenter.BorderThicknessProperty, new WindowsThickness(0)));
        flyoutStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(FlyoutPresenter.BorderBrushProperty, Colors.Transparent.ToWindowsColor()));

        flyoutStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(FlyoutPresenter.MinHeightProperty, 0));
        flyoutStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(FlyoutPresenter.MinWidthProperty, 0));

        return flyoutStyle;
    }

    private void ApplyStyles()
    {
        if (PanelContent is null)
            return;

        FlyoutPresenterStyle = CreateFlyoutStyle();
    }
}

#endif