#if ANDROID

using Android.Views;
using AndroidX.DrawerLayout.Widget;
using Microsoft.Maui.Handlers;
using AView = Android.Views.View;

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class SimpleShellHandler : ViewHandler<ISimpleShell, ViewGroup>
{
    protected override ViewGroup CreatePlatformView()
    {
        // Shell platform view has to be a DrawerLayout because of a change in .NET 7:
        // When the app's root view implements IFlyoutView, this platform view has to be a DrawerLayout
        // See the Connect() method in src/Core/src/Platform/Android/Navigation/NavigationRootManager.cs
        var container = new DrawerLayout(MauiContext.Context)
        {
            Id = AView.GenerateViewId()
        };

        return container;
    }

    protected virtual AView GetNavigationHostContent()
    {
        return (navigationHost?.Handler as SimpleNavigationHostHandler)?.PlatformView?.GetChildAt(0);
    }
}

#endif