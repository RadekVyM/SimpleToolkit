#if __IOS__ || MACCATALYST

using Microsoft.Maui.Handlers;
using UIKit;

namespace Radek.SimpleShell.Handlers
{
    public partial class SimpleShellItemHandler : ElementHandler<ShellItem, UIView>, IAppearanceObserver
    {
        protected override UIView CreatePlatformElement()
        {
            throw new NotImplementedException();

            //shellSectionContainer = new CustomFrameLayout(MauiContext.Context)
            //{
            //    Id = AView.GenerateViewId()
            //};

            //return shellSectionContainer;
        }
    }
}

#endif