#if WINDOWS

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml;
using Windows.System;

namespace SimpleToolkit.Core.Handlers
{
    public partial class ContentButtonHandler : ViewHandler<IContentButton, SimpleContentPanel>
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


        protected override SimpleContentPanel CreatePlatformView()
        {
            if (VirtualView == null)
            {
                throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a LayoutView");
            }

            var view = new SimpleContentPanel
            {
                CrossPlatformMeasure = VirtualView.CrossPlatformMeasure,
                CrossPlatformArrange = VirtualView.CrossPlatformArrange
            };

            view.IsTabStop = true;
            view.UseSystemFocusVisuals = true;
            view.FocusVisualMargin = new Microsoft.UI.Xaml.Thickness(-3);

            return view;
        }

        public override void SetVirtualView(IView view)
        {
            base.SetVirtualView(view);

            _ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
            _ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");

            PlatformView.CrossPlatformMeasure = VirtualView.CrossPlatformMeasure;
            PlatformView.CrossPlatformArrange = VirtualView.CrossPlatformArrange;
        }

        protected override void ConnectHandler(SimpleContentPanel platformView)
        {
            pointerPressedHandler = new PointerEventHandler(OnPointerPressed);
            pointerReleasedHandler = new PointerEventHandler(OnPointerReleased);
            pointerExitedHandler = new PointerEventHandler(OnPointerExited);
            keyDownHandler = new KeyEventHandler(OnKeyDown);

            platformView.Clicked = VirtualView.OnClicked;
            platformView.AddHandler(UIElement.PointerPressedEvent, pointerPressedHandler, true);
            platformView.AddHandler(UIElement.PointerReleasedEvent, pointerReleasedHandler, true);
            platformView.AddHandler(UIElement.PointerExitedEvent, pointerExitedHandler, true);
            platformView.AddHandler(UIElement.KeyDownEvent, keyDownHandler, true);

            base.ConnectHandler(platformView);
        }

        void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var position = e.GetCurrentPoint(PlatformView).Position;

            alreadyReleased = false;

            VirtualView.OnPressed(new Point(position.X, position.Y));
        }

        void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var position = e.GetCurrentPoint(PlatformView).Position;

            VirtualView.OnClicked();
            if (!alreadyReleased)
                VirtualView.OnReleased(new Point(position.X, position.Y));

            alreadyReleased = true;
        }

        void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            var position = e.GetCurrentPoint(PlatformView).Position;

            if (!alreadyReleased)
                VirtualView.OnReleased(new Point(position.X, position.Y));

            alreadyReleased = true;
        }

        private void OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter || e.Key == VirtualKey.Space)
            {
                var position = new Point(PlatformView.ActualWidth / 2d, PlatformView.ActualHeight / 2d);

                alreadyReleased = false;

                VirtualView.OnPressed(new Point(position.X, position.Y));
                VirtualView.OnClicked();
                if (!alreadyReleased)
                    VirtualView.OnReleased(new Point(position.X, position.Y));

                alreadyReleased = true;
            }
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
}


#endif