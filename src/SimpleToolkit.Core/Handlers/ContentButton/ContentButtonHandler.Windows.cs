#if WINDOWS

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml;
using Windows.System;
using SimpleToolkit.Core.Platform;

namespace SimpleToolkit.Core.Handlers;

public partial class ContentButtonHandler : ViewHandler<IContentButton, ContentPanel>
{
    private bool alreadyReleased = true;
    private PointerEventHandler pointerPressedHandler;
    private PointerEventHandler pointerReleasedHandler;
    private PointerEventHandler pointerExitedHandler;
    private KeyEventHandler keyDownHandler;

    public static IPropertyMapper<IContentButton, ContentButtonHandler> Mapper = new PropertyMapper<IContentButton, ContentButtonHandler>(ViewMapper)
    {
        [nameof(IContentButton.Content)] = MapContent,
    };

    public static CommandMapper<IContentButton, ContentButtonHandler> CommandMapper = new(ViewCommandMapper)
    {
    };


    public ContentButtonHandler() : base(Mapper, CommandMapper)
    {
    }

    public ContentButtonHandler(IPropertyMapper mapper = null) : base(mapper ?? Mapper)
    {
    }

    protected ContentButtonHandler(IPropertyMapper mapper, CommandMapper commandMapper = null) : base(mapper, commandMapper ?? CommandMapper)
    {
    }


    protected override ContentPanel CreatePlatformView()
    {
        _ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a SimpleContentPanel");

        var view = new SimpleContentButtonPanel
        {
            CrossPlatformLayout = VirtualView,
            IsTabStop = true,
            UseSystemFocusVisuals = true,
            FocusVisualMargin = new Microsoft.UI.Xaml.Thickness(-3)
        };

        return view;
    }

    public override void SetVirtualView(IView view)
    {
        base.SetVirtualView(view);

        _ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
        _ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");

        PlatformView.CrossPlatformLayout = VirtualView;
    }

    protected override void ConnectHandler(ContentPanel platformView)
    {
        pointerPressedHandler = new PointerEventHandler(OnPointerPressed);
        pointerReleasedHandler = new PointerEventHandler(OnPointerReleased);
        pointerExitedHandler = new PointerEventHandler(OnPointerExited);
        keyDownHandler = new KeyEventHandler(OnKeyDown);

        if (platformView is SimpleContentButtonPanel buttonPanel)
        {
            //buttonPanel.Clicked = VirtualView.OnClicked;
        }
        platformView.AddHandler(UIElement.PointerPressedEvent, pointerPressedHandler, false);
        platformView.AddHandler(UIElement.PointerReleasedEvent, pointerReleasedHandler, false);
        platformView.AddHandler(UIElement.PointerExitedEvent, pointerExitedHandler, false);
        platformView.AddHandler(UIElement.KeyDownEvent, keyDownHandler, false);

        base.ConnectHandler(platformView);
    }

    protected override void DisconnectHandler(ContentPanel platformView)
    {
        platformView.RemoveHandler(UIElement.PointerPressedEvent, pointerPressedHandler);
        platformView.RemoveHandler(UIElement.PointerReleasedEvent, pointerReleasedHandler);
        platformView.RemoveHandler(UIElement.PointerExitedEvent, pointerExitedHandler);
        platformView.RemoveHandler(UIElement.KeyDownEvent, keyDownHandler);

        base.DisconnectHandler(platformView);
    }

    private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        alreadyReleased = false;

        VirtualView.OnPressed(GetPointerPosition(e));
    }

    private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
    {
        if (!alreadyReleased)
        {
            VirtualView.OnReleased(GetPointerPosition(e));
            VirtualView.OnClicked();
        }

        alreadyReleased = true;
    }

    private void OnPointerExited(object sender, PointerRoutedEventArgs e)
    {
        if (!alreadyReleased)
        {
            VirtualView.OnReleased(GetPointerPosition(e));
        }

        alreadyReleased = true;
    }

    private void OnKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Enter || e.Key == VirtualKey.Space)
        {
            var position = new Point(PlatformView.ActualWidth / 2d, PlatformView.ActualHeight / 2d);

            alreadyReleased = false;

            VirtualView.OnPressed(position);
            if (!alreadyReleased)
                VirtualView.OnReleased(position);
            VirtualView.OnClicked();

            alreadyReleased = true;
        }
    }

    private Point GetPointerPosition(PointerRoutedEventArgs e)
    {
        var position = e.GetCurrentPoint(PlatformView).Position;
        return new Point(position.X, position.Y);
    }

    private static void UpdateContent(ContentButtonHandler handler)
    {
        _ = handler.PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
        _ = handler.VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
        _ = handler.MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

        handler.PlatformView.Children.Clear();

        if (handler.VirtualView.PresentedContent is IView view)
            handler.PlatformView.Children.Add(view.ToPlatform(handler.MauiContext));
    }

    public static void MapContent(ContentButtonHandler handler, IContentView contentView)
    {
        UpdateContent(handler);
    }
}

#endif