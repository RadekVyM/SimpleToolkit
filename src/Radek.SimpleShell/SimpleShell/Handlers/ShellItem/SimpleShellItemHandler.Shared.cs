using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
#if ANDROID
using Microsoft.Maui.Controls.Platform.Compatibility;
using Google.Android.Material.Navigation;
using SectionContainer = Microsoft.Maui.Controls.Platform.Compatibility.CustomFrameLayout;
#elif __IOS__ || MACCATALYST
using SectionContainer = UIKit.UIView;
#elif WINDOWS
using SectionContainer = Microsoft.UI.Xaml.Controls.NavigationView;
#else
using SectionContainer = System.Object;
#endif

namespace Radek.SimpleShell.Handlers
{
#if ANDROID || __IOS__ || MACCATALYST || WINDOWS

    public partial class SimpleShellItemHandler
    {
        protected SectionContainer shellSectionContainer;
        protected ShellSection currentShellSection;
        protected SimpleShellSectionHandler currentShellSectionHandler;

        public static PropertyMapper<ShellItem, SimpleShellItemHandler> Mapper =
                new PropertyMapper<ShellItem, SimpleShellItemHandler>(ElementMapper)
                {
                    [nameof(ShellItem.CurrentItem)] = MapCurrentItem,
                    [Shell.TabBarIsVisibleProperty.PropertyName] = MapTabBarIsVisible
                };

        public static CommandMapper<ShellItem, SimpleShellItemHandler> CommandMapper =
                new CommandMapper<ShellItem, SimpleShellItemHandler>(ElementCommandMapper);


        public SimpleShellItemHandler(IPropertyMapper mapper, CommandMapper commandMapper)
            : base(mapper ?? Mapper, commandMapper ?? CommandMapper)
        {
        }

        public SimpleShellItemHandler()
            : base(Mapper, CommandMapper)
        {
        }


        protected void UpdateCurrentItem()
        {
            if (currentShellSection == VirtualView.CurrentItem)
                return;

            if (currentShellSection != null)
            {
                currentShellSection.PropertyChanged -= OnCurrentShellSectionPropertyChanged;
            }

            currentShellSection = VirtualView.CurrentItem;

            if (VirtualView.CurrentItem != null)
            {
                currentShellSectionHandler ??= (SimpleShellSectionHandler)VirtualView.CurrentItem.ToHandler(MauiContext!);

#if ANDROID
                if (PlatformView != shellSectionContainer.GetChildAt(0))
                {
                    shellSectionContainer.RemoveAllViews();
                    shellSectionContainer.AddView(currentShellSectionHandler.PlatformView);
                }
#elif __IOS__ || MACCATALYST
                if (PlatformView != (UIKit.UIView)shellSectionContainer.Subviews[0])
                {
                    shellSectionContainer.ClearSubviews();
                    shellSectionContainer.AddSubview(currentShellSectionHandler.PlatformView);
                }
#elif WINDOWS
                if (PlatformView != (Microsoft.UI.Xaml.Controls.Frame)shellSectionContainer.Content)
                    shellSectionContainer.Content = currentShellSectionHandler.PlatformView;
#endif

                if (currentShellSectionHandler.VirtualView != VirtualView.CurrentItem)
                    currentShellSectionHandler.SetVirtualView(VirtualView.CurrentItem);
            }

            //UpdateSearchHandler();
            //MapMenuItems();

            if (currentShellSection != null)
            {
                currentShellSection.PropertyChanged += OnCurrentShellSectionPropertyChanged;
            }
        }

        void OnCurrentShellSectionPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        public void OnAppearanceChanged(ShellAppearance appearance)
        {
        }

        private static void MapTabBarIsVisible(SimpleShellItemHandler handler, ShellItem item)
        {
        }

        private static void MapCurrentItem(SimpleShellItemHandler handler, ShellItem item)
        {
            handler.UpdateCurrentItem();
        }
    }

#else

    public partial class SimpleShellItemHandler : ElementHandler<ShellItem, System.Object>
    {
        public SimpleShellItemHandler(IPropertyMapper mapper, CommandMapper commandMapper)
            : base(mapper, commandMapper)
        {
        }

        protected override System.Object CreatePlatformElement()
        {
            throw new NotImplementedException();
        }
    }
#endif
}
