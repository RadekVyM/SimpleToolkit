using Microsoft.Maui.Platform;
#if ANDROID
using SectionContainer = Microsoft.Maui.Controls.Platform.Compatibility.CustomFrameLayout;
#elif IOS || MACCATALYST
using SectionContainer = UIKit.UIView;
#elif WINDOWS
using SectionContainer = Microsoft.UI.Xaml.Controls.Border;
#else
using SectionContainer = System.Object;
#endif

namespace SimpleToolkit.SimpleShell.Handlers
{
    public partial class SimpleShellItemHandler : IAppearanceObserver
    {
        public static PropertyMapper<ShellItem, SimpleShellItemHandler> Mapper =
            new PropertyMapper<ShellItem, SimpleShellItemHandler>(ElementMapper)
            {
                [nameof(ShellItem.CurrentItem)] = MapCurrentItem,
                [Shell.TabBarIsVisibleProperty.PropertyName] = MapTabBarIsVisible
            };

        public static CommandMapper<ShellItem, SimpleShellItemHandler> CommandMapper =
            new CommandMapper<ShellItem, SimpleShellItemHandler>(ElementCommandMapper);

        protected IView rootPageOverlay;
        protected SectionContainer shellSectionContainer;
        protected ShellSection currentShellSection;
        protected SimpleShellSectionHandler currentShellSectionHandler;

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

        public virtual void SetRootPageOverlay(IView rootPageOverlay)
        {
            this.rootPageOverlay = rootPageOverlay;

            currentShellSectionHandler?.SetRootPageOverlay(rootPageOverlay);
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
                currentShellSectionHandler ??= (SimpleShellSectionHandler)VirtualView.CurrentItem.ToHandler(MauiContext);

                UpdateShellSectionContainerContent();

                if (currentShellSectionHandler.VirtualView != VirtualView.CurrentItem)
                    currentShellSectionHandler.SetVirtualView(VirtualView.CurrentItem);
            }

            currentShellSectionHandler?.SetRootPageOverlay(rootPageOverlay);

            //UpdateSearchHandler();
            //MapMenuItems();

            if (currentShellSection is not null)
                currentShellSection.PropertyChanged += OnCurrentShellSectionPropertyChanged;
        }

        private void UpdateShellSectionContainerContent()
        {
#if ANDROID
            if (PlatformView != shellSectionContainer.GetChildAt(0))
            {
                shellSectionContainer.RemoveAllViews();
                if (currentShellSectionHandler.PlatformView is not null)
                    shellSectionContainer.AddView(currentShellSectionHandler.PlatformView);
            }
#elif IOS || MACCATALYST
            if (PlatformView != (UIKit.UIView)shellSectionContainer.Subviews.FirstOrDefault())
            {
                shellSectionContainer.ClearSubviews();
                shellSectionContainer.AddSubview(currentShellSectionHandler.PlatformView);
            }
#elif WINDOWS
            if (PlatformView != (Microsoft.UI.Xaml.Controls.Grid)shellSectionContainer.Child)
                shellSectionContainer.Child = currentShellSectionHandler.PlatformView;
#endif
        }

        private void OnCurrentShellSectionPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }

        protected override void DisconnectHandler(SectionContainer platformView)
        {
            base.DisconnectHandler(platformView);

            shellSectionContainer = null;
            currentShellSection = null;
            currentShellSectionHandler = null;
            rootPageOverlay = null;
        }

        public static void MapTabBarIsVisible(SimpleShellItemHandler handler, ShellItem item)
        {
        }

        public static void MapCurrentItem(SimpleShellItemHandler handler, ShellItem item)
        {
            handler.UpdateCurrentItem();
        }
    }
}
