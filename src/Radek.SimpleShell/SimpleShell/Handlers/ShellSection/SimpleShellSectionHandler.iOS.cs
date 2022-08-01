#if __IOS__ || MACCATALYST

using Microsoft.Maui.Handlers;
using UIKit;

namespace Radek.SimpleShell.Handlers
{
    public partial class SimpleShellSectionHandler : ElementHandler<ShellSection, UIView>, IAppearanceObserver
    {
        protected override UIView CreatePlatformElement()
        {
            CreateNavigationManager();
            
            throw new NotImplementedException();

            //var container = new CustomFrameLayout(MauiContext.Context)
            //{
            //    Id = AView.GenerateViewId()
            //};

            //return container;
        }
    }
}

#endif