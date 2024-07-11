using Android.Widget;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Handlers;
using AView = Android.Views.View;

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class SimpleNavigationHostHandler : ViewHandler<ISimpleNavigationHost, FrameLayout>
{
    public static IPropertyMapper<ISimpleNavigationHost, SimpleNavigationHostHandler> Mapper = new PropertyMapper<ISimpleNavigationHost, SimpleNavigationHostHandler>(ViewHandler.ViewMapper)
    {
    };

    public static CommandMapper<ISimpleNavigationHost, SimpleNavigationHostHandler> CommandMapper = new(ViewHandler.ViewCommandMapper)
    {
    };


    public SimpleNavigationHostHandler(IPropertyMapper mapper, CommandMapper commandMapper)
        : base(mapper ?? Mapper, commandMapper ?? CommandMapper)
    {
    }

    public SimpleNavigationHostHandler()
        : base(Mapper, CommandMapper)
    {
    }


    protected override FrameLayout CreatePlatformView()
    {
        return new FrameLayout(MauiContext.Context);
    }

    public virtual void SetContent(AView view)
    {
        PlatformView.RemoveAllViews();
        if (view is not null)
            PlatformView.AddView(view);
    }
}