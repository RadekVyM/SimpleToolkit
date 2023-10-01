#if WINDOWS

using Microsoft.Maui.Handlers;
using WGrid = Microsoft.UI.Xaml.Controls.Grid;
using WFrameworkElement = Microsoft.UI.Xaml.FrameworkElement;

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class SimpleNavigationHostHandler : ViewHandler<ISimpleNavigationHost, WGrid>
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


    protected override WGrid CreatePlatformView()
    {
        return new WGrid();
    }

    public virtual void SetContent(WFrameworkElement element)
    {
        PlatformView.Children.Clear();
        if (element is not null)
            PlatformView.Children.Add(element);
    }
}

#endif