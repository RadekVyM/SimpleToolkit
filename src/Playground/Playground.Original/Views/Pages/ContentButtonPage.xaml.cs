using System.ComponentModel;
using System.Windows.Input;
using SimpleToolkit.Core;

namespace Playground.Original.Views.Pages
{
    public partial class ContentButtonPage : ContentPage
    {
        private const string WideText = "a wide content that should change the size of the button";
        private const string ShortText = "a short text";

        bool commandParameter => (bool)contentButton.CommandParameter;


        public ContentButtonPage()
        {
            InitializeComponent();

            contentButton.Command = new Command(parameter => DisplayAlert("Command", "Executed", "OK"), parameter => parameter is null || (bool)parameter);

            contentButton.PropertyChanged += ContentButtonPropertyChanged;

            UpdateIsEnabled(true);
            UpdateCommandParameter(true);
        }


        private void UpdateIsEnabled(bool value)
        {
            contentButton.IsEnabled = value;
        }

        private void UpdateCommandParameter(bool value)
        {
            contentButton.CommandParameter = value;
            (contentButton.Command as Command).ChangeCanExecute();
        }

        private void StarButtonClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync(nameof(ImagePage));
        }

        private void SignInClicked(object sender, EventArgs e)
        {
            testBehavior.Test();
        }

        private void VariableButtonClicked(object sender, EventArgs e)
        {
            variableContentLabel.Text = variableContentLabel.Text == ShortText ? WideText : ShortText;
        }

        private void ChangeButtonClicked(object sender, EventArgs e)
        {
            UpdateIsEnabled(!contentButton.IsEnabled);
        }

        private void ChangeCommandParameterButtonClicked(object sender, EventArgs e)
        {
            UpdateCommandParameter(!commandParameter);
        }

        private void ContentButtonPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
                isEnabledButton.Text = contentButton.IsEnabled ? "Enabled" : "Disabled";
            else if (e.PropertyName == ContentButton.CommandParameterProperty.PropertyName)
                commandParameterButton.Text = commandParameter ? "Can execute" : "Cannot execute";
        }
    }
}
