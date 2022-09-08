using SimpleToolkit.Core;
using SimpleToolkit.SimpleShell.Controls;
using SimpleToolkit.SimpleShell.Playground.Views.Pages;
using System.Windows.Input;

namespace SimpleToolkit.SimpleShell.Playground
{
    public partial class SimpleAppShell : SimpleShell
    {
        public ICommand BackCommand { get; set; }

        public SimpleAppShell()
        {
            BackCommand = new Command(async () => {
                await this.GoToAsync("..");
            });

            InitializeComponent();

            designLanguagesListPopover.Items = new List<DesignLanguageItem>
            {
                new DesignLanguageItem("Material3", () =>
                {
                    VisualStateManager.GoToState(this, "Material3");
                    designButton.HideAttachedPopover();
                }),
                new DesignLanguageItem("Cupertino", () =>
                {
                    VisualStateManager.GoToState(this, "Cupertino");
                    designButton.HideAttachedPopover();
                }),
                new DesignLanguageItem("Fluent", () =>
                {
                    VisualStateManager.GoToState(this, "Fluent");
                    designButton.HideAttachedPopover();
                }),
            };

            VisualStateManager.GoToState(this, "Material3");

            Routing.RegisterRoute(nameof(FirstYellowDetailPage), typeof(FirstYellowDetailPage));
            Routing.RegisterRoute(nameof(SecondYellowDetailPage), typeof(SecondYellowDetailPage));
            Routing.RegisterRoute(nameof(ThirdYellowDetailPage), typeof(ThirdYellowDetailPage));
            Routing.RegisterRoute(nameof(FirstGreenDetailPage), typeof(FirstGreenDetailPage));
        }

        private async void ShellItemButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var shellItem = button.BindingContext as BaseShellItem;

            if (!CurrentState.Location.OriginalString.Contains(shellItem.Route))
                await this.GoToAsync($"///{shellItem.Route}");
        }

        private async void TabBarItemSelected(object sender, TabItemSelectedEventArgs e)
        {
            if (!CurrentState.Location.OriginalString.Contains(e.ShellItem.Route))
                await this.GoToAsync($"///{e.ShellItem.Route}");
        }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            await this.GoToAsync("..");
        }

        private bool orangeAdded = false;

        private void AddButtonClicked(object sender, EventArgs e)
        {
            if (!orangeAdded)
            {
                this.Items.Add(new ShellContent()
                {
                    Title = "Orange",
                    Route = nameof(OrangePage),
                    ContentTemplate = new DataTemplate(typeof(OrangePage))
                });

                addButton.IsVisible = false;
            }

            orangeAdded = true;
        }

        private void ShowPopoverButtonClicked(object sender, EventArgs e)
        {
            var button = sender as View;
            button.ShowAttachedPopover();
        }

        private void DesignLanguagesListPopoverItemSelected(object sender, ListPopoverItemSelectedEventArgs e)
        {
            if (e.Item is DesignLanguageItem designLanguageItem)
            {
                designLanguageItem.Action?.Invoke();
            }
        }

        private record DesignLanguageItem(string Title, Action Action)
        {
            public override string ToString() => Title;
        }
    }
}
