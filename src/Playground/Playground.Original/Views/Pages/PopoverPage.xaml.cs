using SimpleToolkit.Core;

namespace Playground.Original.Views.Pages;

public partial class PopoverPage : ContentPage
{
    public PopoverPage()
    {
        InitializeComponent();

        popoverAlignmentPicker.ItemsSource = new List<HorizontalAlignment>()
        {
            HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Left
        };
        popoverAlignmentPicker.SelectedItem = HorizontalAlignment.Center;
    }

    private void ButtonClicked(object sender, EventArgs e)
    {
        var button = sender as View;
        button.ShowAttachedPopover();
    }
}
