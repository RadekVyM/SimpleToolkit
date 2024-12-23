﻿using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using SimpleToolkit.Core.Handlers;
using WindowsThickness = Microsoft.UI.Xaml.Thickness;
using WGrid = Microsoft.UI.Xaml.Controls.Grid;
using XamlStyle = Microsoft.UI.Xaml.Style;

namespace SimpleToolkit.Core.Platform;

public partial class SimpleFlyout : Flyout
{
    private readonly IMauiContext mauiContext;

    internal WGrid PanelContent => (WGrid)Content;
    public IPopover? VirtualView { get; private set; }
    
    
    public SimpleFlyout(IMauiContext mauiContext)
    {
        this.mauiContext = mauiContext ?? throw new ArgumentNullException(nameof(mauiContext));

        LightDismissOverlayMode = LightDismissOverlayMode.Off;
    }


    public void SetElement(IPopover element)
    {
        VirtualView = element ?? throw new ArgumentNullException(nameof(element));
    }

    public void UpdateContent()
    {
        if (PanelContent is null && VirtualView?.Content is not null && VirtualView.Handler is PopoverHandler handler)
        {
            PanelContent?.Children.Clear();

            var content = handler.VirtualView.Content?.ToPlatform(mauiContext);
            var grid = new WGrid();
            grid.Children.Add(content);
            Content = grid;
        }
    }

    public void Show(IElement anchor)
    {
        if (VirtualView is null)
            return;

        ApplyStyles();
        AreOpenCloseAnimationsEnabled = VirtualView.IsAnimated;

        var platformAnchor = anchor?.ToPlatform(mauiContext) ?? GetDefaultAnchor();

        SetPlacement(VirtualView.Alignment);
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
        ArgumentNullException.ThrowIfNull(VirtualView?.Parent);
        var frameworkElement = VirtualView.Parent.ToPlatform(mauiContext);
        frameworkElement.ContextFlyout = this;

        return frameworkElement;
    }

    private void SetPlacement(PopoverAlignment alignment)
    {
        Placement = alignment switch
        {
            PopoverAlignment.Start => FlyoutPlacementMode.BottomEdgeAlignedLeft,
            PopoverAlignment.End => FlyoutPlacementMode.BottomEdgeAlignedRight,
            _ => FlyoutPlacementMode.Bottom
        };
    }

    private XamlStyle CreateFlyoutStyle()
    {
        var flyoutStyle = new XamlStyle(typeof(FlyoutPresenter));

        if (VirtualView?.UseDefaultStyling is not true)
        {
            flyoutStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(FlyoutPresenter.BackgroundProperty, Colors.Transparent.ToWindowsColor()));
            flyoutStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(FlyoutPresenter.IsDefaultShadowEnabledProperty, false));
            flyoutStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(FlyoutPresenter.BorderThicknessProperty, new WindowsThickness(0)));
            flyoutStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(FlyoutPresenter.BorderBrushProperty, Colors.Transparent.ToWindowsColor()));
        }
        else
        {
            flyoutStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(FlyoutPresenter.IsDefaultShadowEnabledProperty, true));
        }

        flyoutStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(FlyoutPresenter.PaddingProperty, 0));
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