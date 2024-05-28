using Microsoft.Maui.Handlers;

namespace SimpleToolkit.Core.Handlers;

public partial class ContentButtonHandler : BorderHandler
{
    public new IContentButton VirtualView => base.VirtualView as IContentButton;

    public ContentButtonHandler() : base(Mapper, CommandMapper)
    { }

    public ContentButtonHandler(IPropertyMapper mapper)
        : base(mapper ?? Mapper, CommandMapper)
    { }

    public ContentButtonHandler(IPropertyMapper mapper, CommandMapper commandMapper)
        : base(mapper ?? Mapper, commandMapper ?? CommandMapper)
    { }
}