﻿using SimpleToolkit.Core;

namespace Playground.Original.Views.Pages;

public partial class PopoverPage : ContentPage
{
    public PopoverPage()
    {
        InitializeComponent();

        popoverAlignmentPicker.ItemsSource = new List<PopoverAlignment>()
        {
            PopoverAlignment.Center, PopoverAlignment.Start, PopoverAlignment.End
        };
        popoverAlignmentPicker.SelectedItem = PopoverAlignment.Center;
    }

    private void ButtonClicked(object? sender, EventArgs e)
    {
        if (sender is not View button)
            return;

        button.ShowAttachedPopover();
    }
}