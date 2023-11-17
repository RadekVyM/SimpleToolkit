using Microsoft.Maui.Platform;

// TODO: Popover placement - this should be possible on Android and Windows

namespace SimpleToolkit.Core;

/// <summary>
/// Popover that can be anchored to a view.
/// </summary>
[ContentProperty(nameof(Content))]
public class Popover : Element, IPopover
{
    public static readonly BindableProperty ContentProperty =
        BindableProperty.Create(nameof(Content), typeof(View), typeof(Popover), propertyChanged: OnContentChanged);

    public static readonly BindableProperty AttachedPopoverProperty =
        BindableProperty.CreateAttached("AttachedPopover", typeof(Popover), typeof(View), null);

    public virtual View Content
    {
        get => (View)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    /// <summary>
    /// Returns a popover that is attached to the view.
    /// </summary>
    /// <param name="view">The view to which the popover is attached.</param>
    /// <returns>The popover that is attached to the view.</returns>
    public static Popover GetAttachedPopover(BindableObject view)
    {
        _ = view ?? throw new ArgumentNullException(nameof(view));
        return (Popover)view.GetValue(AttachedPopoverProperty);
    }

    /// <summary>
    /// Attaches the popover to the view.
    /// </summary>
    /// <param name="view">The view to which the popover will be attached.</param>
    /// <param name="popover">The popover that will be attached to the view.</param>
    public static void SetAttachedPopover(BindableObject view, Popover popover)
    {
        _ = view ?? throw new ArgumentNullException(nameof(view));
        view.SetValue(AttachedPopoverProperty, popover);
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
        if (Handler?.PlatformView is SimpleToolkit.Core.Platform.SimpleFlyout platformFlyout)
            platformFlyout.SetUpPlatformView();
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