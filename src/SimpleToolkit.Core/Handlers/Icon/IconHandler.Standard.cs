#if !(WINDOWS || ANDROID || IOS || MACCATALYST)
namespace SimpleToolkit.Core.Handlers
{
    public partial class IconHandler : Microsoft.Maui.Handlers.ElementHandler<IIcon, object>
    {
        public IconHandler(IPropertyMapper mapper, CommandMapper commandMapper) : base(mapper, commandMapper)
        {
        }

        protected override object CreatePlatformElement() => throw new NotSupportedException();

        public static void MapSource(IconHandler handler, IIcon icon)
        {
        }

        public static Task MapSourceAsync(IconHandler handler, IIcon icon)
        {
            return Task.CompletedTask;
        }

        public static void MapTintColor(IconHandler handler, IIcon icon)
        {
        }
    }

}
#endif
