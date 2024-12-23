using SimpleToolkit.Core;

namespace Playground.Original.Views.Controls;

public partial class PopoverTest : Button
{
    public static readonly BindableProperty PopoverAlignmentProperty =
        BindableProperty.Create(nameof(PopoverAlignment), typeof(PopoverAlignment), typeof(PopoverTest), propertyChanged: static (sender, oldValue, newValue) =>
        {
            if (sender is not PopoverTest button)
                return;

            button.popover.Alignment = (PopoverAlignment)newValue;
        });

    public static readonly BindableProperty UseDefaultStylingProperty =
        BindableProperty.Create(nameof(UseDefaultStyling), typeof(bool), typeof(PopoverTest), defaultValue: true, propertyChanged: static (sender, oldValue, newValue) =>
        {
            if (sender is not PopoverTest button)
                return;

            button.popover.UseDefaultStyling = (bool)newValue;
        });

    public static readonly BindableProperty IsAnimatedProperty =
        BindableProperty.Create(nameof(IsAnimated), typeof(bool), typeof(PopoverTest), defaultValue: true, propertyChanged: static (sender, oldValue, newValue) =>
        {
            if (sender is not PopoverTest button)
                return;

            button.popover.IsAnimated = (bool)newValue;
        });

    public virtual PopoverAlignment PopoverAlignment
    {
        get => (PopoverAlignment)GetValue(PopoverAlignmentProperty);
        set => SetValue(PopoverAlignmentProperty, value);
    }

    public virtual bool UseDefaultStyling
    {
        get => (bool)GetValue(UseDefaultStylingProperty);
        set => SetValue(UseDefaultStylingProperty, value);
    }

    public virtual bool IsAnimated
    {
        get => (bool)GetValue(IsAnimatedProperty);
        set => SetValue(IsAnimatedProperty, value);
    }


    public PopoverTest()
	{
		InitializeComponent();
	}


    private void ButtonClicked(object? sender, EventArgs e)
    {
        if (sender is not View button)
            return;

        button.ShowAttachedPopover();
    }

    private void ResizeButtonClicked(object sender, EventArgs e)
    {
        var isNormal = resizablePopoverContent.WidthRequest == 100;
        resizablePopoverContent.WidthRequest = isNormal ? 150 : 100;
        resizablePopoverContent.HeightRequest = isNormal ? 120 : 80;
    }

    private void HideButtonClicked(object sender, EventArgs e)
    {
        popover.Hide();
    }
}