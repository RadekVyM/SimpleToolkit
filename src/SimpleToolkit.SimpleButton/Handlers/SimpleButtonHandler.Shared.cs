using Microsoft.Maui.Handlers;

namespace SimpleToolkit.SimpleButton.Handlers;

public partial class SimpleButtonHandler : BorderHandler
{
    public new ISimpleButton VirtualView => (ISimpleButton)base.VirtualView;

    public SimpleButtonHandler() : base(Mapper, CommandMapper)
    { }

    public SimpleButtonHandler(IPropertyMapper mapper)
        : base(mapper ?? Mapper, CommandMapper)
    { }

    public SimpleButtonHandler(IPropertyMapper mapper, CommandMapper commandMapper)
        : base(mapper ?? Mapper, commandMapper ?? CommandMapper)
    { }
}