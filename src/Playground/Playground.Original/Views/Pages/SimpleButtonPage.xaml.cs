using System.ComponentModel;
using SimpleToolkit.SimpleButton;

namespace Playground.Original.Views.Pages;

public partial class SimpleButtonPage : ContentPage
{
    private const string WideText = "a wide content that should change the size of the button";
    private const string ShortText = "a short text";

    bool commandParameter => simpleButton.CommandParameter is true;


    public SimpleButtonPage()
    {
        InitializeComponent();

        simpleButton.Command = new Command(async (parameter) => await DisplayAlertAsync("Command", "Executed", "OK"), parameter => parameter is null || (bool)parameter);

        simpleButton.PropertyChanged += SimpleButtonPropertyChanged;

        UpdateIsEnabled(true);
        UpdateCommandParameter(true);
    }


    private void UpdateIsEnabled(bool value)
    {
        simpleButton.IsEnabled = value;
    }

    private void UpdateCommandParameter(bool value)
    {
        simpleButton.CommandParameter = value;
        if (simpleButton.Command is Command command)
            command.ChangeCanExecute();
    }

    private void StarButtonClicked(object? sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(ImagePage));
    }

    private void SignInClicked(object? sender, EventArgs e)
    {
        testBehavior.Test();
    }

    private void VariableButtonClicked(object? sender, EventArgs e)
    {
        variableContentLabel.Text = variableContentLabel.Text == ShortText ? WideText : ShortText;
    }

    private void ChangeButtonClicked(object sender, EventArgs e)
    {
        UpdateIsEnabled(!simpleButton.IsEnabled);
    }

    private void ChangeCommandParameterButtonClicked(object sender, EventArgs e)
    {
        UpdateCommandParameter(!commandParameter);
    }

    private void SimpleButtonPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
            isEnabledButton.Text = simpleButton.IsEnabled ? "Enabled" : "Disabled";
        else if (e.PropertyName == SimpleButton.CommandParameterProperty.PropertyName)
            commandParameterButton.Text = commandParameter ? "Can execute" : "Cannot execute";
    }
}