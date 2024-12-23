﻿using Microsoft.Maui.Platform;
#if ANDROID
using SectionContainer = Android.Widget.FrameLayout;
#elif IOS || MACCATALYST
using SectionContainer = UIKit.UIView;
#elif WINDOWS
using SectionContainer = Microsoft.UI.Xaml.Controls.Border;
#else
using SectionContainer = System.Object;
#endif

// Partially based on https://github.com/dotnet/maui/blob/main/src/Controls/src/Core/Handlers/Shell/ShellItemHandler.Windows.cs

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class SimpleShellItemHandler : IAppearanceObserver
{
    public static PropertyMapper<ShellItem, SimpleShellItemHandler> Mapper =
        new(ElementMapper)
        {
            [nameof(ShellItem.CurrentItem)] = MapCurrentItem,
            [Shell.TabBarIsVisibleProperty.PropertyName] = MapTabBarIsVisible,
            [SimpleShell.ShellGroupContainerProperty.PropertyName] = MapShellGroupContainer
        };

    public static CommandMapper<ShellItem, SimpleShellItemHandler> CommandMapper =
        new(ElementCommandMapper);

    protected IView? rootPageContainer;
    protected ShellSection? currentShellSection;
    protected IBaseSimpleShellSectionHandler? currentShellSectionHandler;


    public SimpleShellItemHandler(IPropertyMapper mapper, CommandMapper commandMapper)
        : base(mapper ?? Mapper, commandMapper ?? CommandMapper)
    {
    }

    public SimpleShellItemHandler()
        : base(Mapper, CommandMapper)
    {
    }


    public virtual void OnAppearanceChanged(ShellAppearance appearance)
    {
    }

    public virtual void SetRootPageContainer(IView? view)
    {
        rootPageContainer = view;
        currentShellSectionHandler?.SetRootPageContainer(view);
    }

    protected void UpdateCurrentItem()
    {
        if (currentShellSection == VirtualView.CurrentItem)
            return;

        if (currentShellSection is not null)
            currentShellSection.PropertyChanged -= OnCurrentShellSectionPropertyChanged;

        currentShellSection = VirtualView.CurrentItem;

        if (VirtualView.CurrentItem is not null)
        {
            // One handler is reused for all ShellSections
            currentShellSectionHandler ??=
                (IBaseSimpleShellSectionHandler)VirtualView.CurrentItem.ToHandler(MauiContext ?? throw new NullReferenceException("MauiContext cannot be null here."));

            UpdatePlatformViewContent();

            if (currentShellSectionHandler.VirtualView != VirtualView.CurrentItem)
                currentShellSectionHandler.SetVirtualView(VirtualView.CurrentItem);
        }

        currentShellSectionHandler?.SetRootPageContainer(rootPageContainer);

        if (currentShellSection is not null)
            currentShellSection.PropertyChanged += OnCurrentShellSectionPropertyChanged;
    }

    private static void OnCurrentShellSectionPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
    }

    protected override void DisconnectHandler(SectionContainer platformView)
    {
        base.DisconnectHandler(platformView);

        currentShellSection = null;
        currentShellSectionHandler = null;
        rootPageContainer = null;
    }

    public static void MapTabBarIsVisible(SimpleShellItemHandler handler, ShellItem item)
    {
    }

    public static void MapCurrentItem(SimpleShellItemHandler handler, ShellItem item)
    {
        handler.UpdateCurrentItem();
    }

    public static void MapShellGroupContainer(SimpleShellItemHandler handler, ShellItem item)
    {
        handler.currentShellSectionHandler?.RefreshGroupContainers();
    }
}