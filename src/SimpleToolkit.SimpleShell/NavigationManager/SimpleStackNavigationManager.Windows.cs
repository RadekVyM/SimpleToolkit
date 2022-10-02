#if WINDOWS

using Microsoft.Maui.Platform;
using PlatformPage = Microsoft.UI.Xaml.FrameworkElement;

namespace SimpleToolkit.SimpleShell.NavigationManager
{
    public partial class SimpleStackNavigationManager
    {
        protected virtual void AddPlatformPage(PlatformPage newPageView, bool onTop = true)
        {
            var overlay = this.rootPageOverlay?.ToHandler(mauiContext).PlatformView;

            if (onTop)
            {
                if (isCurrentPageRoot && overlay is not null)
                {
                    if (!navigationFrame.Children.Contains(overlay))
                    {
                        navigationFrame.Children.Add(newPageView);
                        navigationFrame.Children.Add(overlay);
                    }
                    else
                    {
                        navigationFrame.Children.Insert(navigationFrame.Children.IndexOf(overlay), newPageView);
                    }
                }
                else
                {
                    navigationFrame.Children.Add(newPageView);
                }
            }
            else
            {
                navigationFrame.Children.Insert(0, newPageView);

                if (isCurrentPageRoot && overlay is not null && !navigationFrame.Children.Contains(overlay))
                    navigationFrame.Children.Insert(1, overlay);
            }
        }
        
        protected virtual void RemovePlatformPage(PlatformPage oldPageView)
        {
            var overlay = this.rootPageOverlay?.ToHandler(mauiContext).PlatformView;

            if (oldPageView is not null)
                navigationFrame.Children.Remove(oldPageView);
            if (!isCurrentPageRoot && isPreviousPageRoot && overlay is not null)
                navigationFrame.Children.Remove(overlay);
        }

        protected virtual void ReplaceRootPageOverlay(IView rootPageOverlay)
        {
            var oldOverlay = this.rootPageOverlay?.ToHandler(mauiContext).PlatformView;
            var newOverlay = rootPageOverlay?.ToHandler(mauiContext).PlatformView;

            if (oldOverlay is not null)
                navigationFrame.Children.Remove(oldOverlay);
            if (newOverlay is not null && isCurrentPageRoot)
                navigationFrame.Children.Add(newOverlay);
        }
    }
}

#endif