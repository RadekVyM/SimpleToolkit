#if WINDOWS
// || (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)

using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml.Controls;

namespace Radek.SimpleShell.Handlers
{
    public partial class SimpleShellItemHandler : ElementHandler<ShellItem, NavigationView>, IAppearanceObserver
    {
        protected override NavigationView CreatePlatformElement()
        {
            shellSectionContainer = new NavigationView()
            {
                PaneDisplayMode = NavigationViewPaneDisplayMode.Top,
                IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed,
                IsSettingsVisible = false,
                IsPaneToggleButtonVisible = false
            };

            return shellSectionContainer;
        }
    }
}

#endif