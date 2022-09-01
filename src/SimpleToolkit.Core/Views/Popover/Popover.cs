using Microsoft.Maui.Platform;

// TODO: Popover placement - this should be possible on Android and Windows

namespace SimpleToolkit.Core
{
    [ContentProperty(nameof(Content))]
    public class Popover : Element, IPopover
    {
        public static readonly BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(View), typeof(Popover), propertyChanged: OnContentChanged);

        public static readonly BindableProperty AttachedPopoverProperty =
            BindableProperty.CreateAttached("AttachedPopover", typeof(Popover), typeof(View), null);

        public virtual View Content
        {
            get => (View)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static Popover GetAttachedPopover(BindableObject view)
        {
            return (Popover)view.GetValue(AttachedPopoverProperty);
        }

        public static void SetAttachedPopover(BindableObject view, Popover value)
        {
            view.SetValue(AttachedPopoverProperty, value);
        }

        public virtual void Show(View parentView)
        {
            var mauiContext = parentView.Handler.MauiContext;
            var platformPopup = this.ToHandler(mauiContext);

            Parent = parentView;

            platformPopup.Invoke(nameof(IPopover.Show), parentView);
        }

        public virtual void Hide()
        {
            Handler.Invoke(nameof(IPopover.Hide));
        }

        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();

#if WINDOWS
            if (Handler?.PlatformView is null)
            {
                return;
            }

            ((SimpleToolkit.Core.Handlers.SimpleFlyout)Handler.PlatformView).SetUpPlatformView(CleanUp, CreateWrapperContent);

            static void CleanUp(Microsoft.UI.Xaml.Controls.Panel wrapper)
            {
                ((SimpleToolkit.Core.Handlers.WrapperPanel)wrapper).CleanUp();
            }

            static Microsoft.UI.Xaml.Controls.Panel CreateWrapperContent(Handlers.PopoverHandler handler)
            {
                if (handler.VirtualView.Content is null || handler.MauiContext is null)
                {
                    return null;
                }

                return new SimpleToolkit.Core.Handlers.WrapperPanel(handler.VirtualView.Content, handler.MauiContext);
            }
#endif
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (Content is not null)
            {
                SetInheritedBindingContext(Content, BindingContext);
            }
        }

        private static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var popover = bindable as Popover;
            popover.OnBindingContextChanged();
        }
    }
}
