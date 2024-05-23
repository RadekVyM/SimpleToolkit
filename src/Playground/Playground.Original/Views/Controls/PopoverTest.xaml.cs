using SimpleToolkit.Core;

namespace Playground.Original.Views.Controls;

public partial class PopoverTest : Button
{
    public static readonly BindableProperty PopoverHorizontalAlignmentProperty =
        BindableProperty.Create(nameof(PopoverTest), typeof(HorizontalAlignment), typeof(PopoverTest), propertyChanged: static (sender, oldValue, newValue) =>
        {
            var button = sender as PopoverTest;
            button.popover.HorizontalAlignment = (HorizontalAlignment)newValue;
        });

    public virtual HorizontalAlignment PopoverHorizontalAlignment
    {
        get => (HorizontalAlignment)GetValue(PopoverHorizontalAlignmentProperty);
        set => SetValue(PopoverHorizontalAlignmentProperty, value);
    }



    public PopoverTest()
	{
		InitializeComponent();
	}


    private void ButtonClicked(object sender, EventArgs e)
    {
        var button = sender as View;
        button.ShowAttachedPopover();
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {
        var isNormal = resizablePopoverContent.WidthRequest == 100;
        resizablePopoverContent.WidthRequest = isNormal ? 150 : 100;
        resizablePopoverContent.HeightRequest = isNormal ? 120 : 80;
    }
}