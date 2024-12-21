using Microsoft.Maui.Platform;
#if __IOS__ || MACCATALYST
using PlatformImage = UIKit.UIImage;
#elif ANDROID
using PlatformImage = Android.Graphics.Drawables.Drawable;
#elif WINDOWS
using PlatformImage = Microsoft.UI.Xaml.Media.ImageSource;
#elif TIZEN
using PlatformImage = Microsoft.Maui.Platform.MauiImageSource;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformImage = System.Object;
#endif

// Based on: https://github.com/dotnet/maui/blob/main/src/Core/src/Platform/ImageSourcePartSetter.cs

namespace SimpleToolkit.Core.Handlers;

internal abstract class SimpleIconSourcePartSetter : IImageSourcePartSetter
{
    readonly WeakReference<IconHandler> _handler;

    public SimpleIconSourcePartSetter(IconHandler handler) =>
        _handler = new(handler);

    public IImageSourcePart? ImageSourcePart =>
        Handler?.VirtualView as IImageSourcePart ?? Handler?.VirtualView;

    public IconHandler? Handler
    {
        get
        {
            _handler.TryGetTarget(out var handler);
            return handler;
        }
    }

    IElementHandler? IImageSourcePartSetter.Handler => Handler;

    public abstract void SetImageSource(PlatformImage? platformImage);
}