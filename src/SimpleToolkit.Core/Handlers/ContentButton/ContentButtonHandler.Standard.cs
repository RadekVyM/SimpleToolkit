#if !(ANDROID || WINDOWS || IOS || MACCATALYST)

namespace SimpleToolkit.Core.Handlers;

public partial class ContentButtonHandler : Microsoft.Maui.Handlers.ElementHandler<IContentButton, object>
{
    public ContentButtonHandler(IPropertyMapper mapper, CommandMapper commandMapper) : base(mapper, commandMapper)
    {
    }

    protected override object CreatePlatformElement() => throw new NotSupportedException();
}

#endif