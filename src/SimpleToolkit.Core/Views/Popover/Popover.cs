using Microsoft.Maui.Platform;

namespace SimpleToolkit.Core;

/// <summary>
/// Popover that can be anchored to a view.
/// </summary>
[ContentProperty(nameof(Content))]
public class Popover : Element, IPopover
{
    public static readonly BindableProperty ContentProperty =
        BindableProperty.Create(nameof(Content), typeof(View), typeof(Popover), propertyChanged: OnContentChanged);

    public static readonly BindableProperty IsAnimatedProperty =
        BindableProperty.Create(nameof(IsAnimated), typeof(bool), typeof(Popover), defaultValue: true);

    public static readonly BindableProperty AlignmentProperty =
        BindableProperty.Create(nameof(Alignment), typeof(PopoverAlignment), typeof(Popover), defaultValue: PopoverAlignment.Center);

    public static readonly BindableProperty UseDefaultStylingProperty =
        BindableProperty.Create(nameof(UseDefaultStyling), typeof(bool), typeof(Popover), defaultValue: false);

    public static readonly BindableProperty PermittedArrowDirectionsProperty =
        BindableProperty.Create(nameof(PermittedArrowDirections), typeof(PopoverArrowDirection), typeof(Popover), defaultValue: PopoverArrowDirection.Any);

    public static readonly BindableProperty AttachedPopoverProperty =
        BindableProperty.CreateAttached("AttachedPopover", typeof(Popover), typeof(View), null);

    public virtual View? Content
    {
        get => GetValue(ContentProperty) as View;
        set => SetValue(ContentProperty, value);
    }

    public virtual bool IsAnimated
    {
        get => (bool)GetValue(IsAnimatedProperty);
        set => SetValue(IsAnimatedProperty, value);
    }

    public virtual PopoverAlignment Alignment
    {
        get => (PopoverAlignment)GetValue(AlignmentProperty);
        set => SetValue(AlignmentProperty, value);
    }

    public virtual bool UseDefaultStyling
    {
        get => (bool)GetValue(UseDefaultStylingProperty);
        set => SetValue(UseDefaultStylingProperty, value);
    }

    public virtual PopoverArrowDirection PermittedArrowDirections
    {
        get => (PopoverArrowDirection)GetValue(PermittedArrowDirectionsProperty);
        set => SetValue(PermittedArrowDirectionsProperty, value);
    }

    /// <summary>
    /// Returns a popover that is attached to the view.
    /// </summary>
    /// <param name="view">The view to which the popover is attached.</param>
    /// <returns>The popover that is attached to the view.</returns>
    public static Popover? GetAttachedPopover(BindableObject view)
    {
        _ = view ?? throw new ArgumentNullException(nameof(view));
        return (Popover)view.GetValue(AttachedPopoverProperty);
    }

    /// <summary>
    /// Attaches the popover to the view.
    /// </summary>
    /// <param name="view">The view to which the popover will be attached.</param>
    /// <param name="popover">The popover that will be attached to the view.</param>
    public static void SetAttachedPopover(BindableObject view, Popover? popover)
    {
        _ = view ?? throw new ArgumentNullException(nameof(view));
        view.SetValue(AttachedPopoverProperty, popover);
    }

    public virtual void Show(View parentView)
    {
        var mauiContext = parentView.Handler?.MauiContext ?? throw new NullReferenceException("MauiContext should not be null here.");
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
            platformFlyout.UpdateContent();
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
        if (bindable is not Popover popover)
            return;

        popover.OnBindingContextChanged();
    }
}