using SimpleToolkit.Core;
using SimpleToolkit.SimpleShell.Controls;
using Playground.Original.Views.Pages;
using SimpleToolkit.SimpleShell.Transitions;
using SimpleToolkit.SimpleShell.Extensions;
using System.Windows.Input;
using SimpleToolkit.SimpleShell;

namespace Playground.Original
{
    public partial class PlaygroundAppShell : SimpleShell
    {
        public ICommand BackCommand { get; set; }

        public PlaygroundAppShell()
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
            designLanguagesListPopover.SelectedItem = designLanguagesListPopover.Items.FirstOrDefault();

            Routing.RegisterRoute(nameof(ImagePage), typeof(ImagePage));
            Routing.RegisterRoute(nameof(FirstYellowDetailPage), typeof(FirstYellowDetailPage));
            Routing.RegisterRoute(nameof(SecondYellowDetailPage), typeof(SecondYellowDetailPage));
            Routing.RegisterRoute(nameof(ThirdYellowDetailPage), typeof(ThirdYellowDetailPage));
            Routing.RegisterRoute(nameof(FourthYellowDetailPage), typeof(FourthYellowDetailPage));
            Routing.RegisterRoute(nameof(FirstGreenDetailPage), typeof(FirstGreenDetailPage));
            Routing.RegisterRoute(nameof(ImageDetailPage), typeof(ImageDetailPage));

            Loaded += PlaygroundAppShellLoaded;

            this.SetTransition(
                callback: static args =>
                {
                    switch (args.TransitionType)
                    {
                        case SimpleShellTransitionType.Switching:
                            if (args.OriginShellSectionContainer == args.DestinationShellSectionContainer)
                            {
                                args.OriginPage.Opacity = 1 - args.Progress;
                                args.DestinationPage.Opacity = args.Progress;
                            }
                            else
                            {
                                (args.OriginShellSectionContainer ?? args.OriginPage).Opacity = 1 - args.Progress;
                                (args.DestinationShellSectionContainer ?? args.DestinationPage).Opacity = args.Progress;
                            }
                            break;
                        case SimpleShellTransitionType.Pushing:
                            args.DestinationPage.Opacity = args.DestinationPage.Width < 0 ? 0 : 1;
                            args.DestinationPage.TranslationX = (1 - args.Progress) * args.DestinationPage.Width;
                            break;
                        case SimpleShellTransitionType.Popping:
                            args.OriginPage.TranslationX = args.Progress * args.OriginPage.Width;
                            break;
                    }
                },
                duration: static args => 250u,
                finished: static args =>
                {
                    args.DestinationPage.TranslationX = 0;
                    args.OriginPage.TranslationX = 0;
                    args.OriginPage.Opacity = 1;
                    args.DestinationPage.Opacity = 1;
                    if (args.OriginShellSectionContainer is not null)
                        args.OriginShellSectionContainer.Opacity = 1;
                    if (args.DestinationShellSectionContainer is not null)
                        args.DestinationShellSectionContainer.Opacity = 1;
                },
                destinationPageInFront: static args => args.TransitionType switch
                {
                    SimpleShellTransitionType.Pushing => true,
                    _ => false
                },
                easing: static args => args.TransitionType switch
                {
                    SimpleShellTransitionType.Pushing => Easing.SinIn,
                    SimpleShellTransitionType.Popping => Easing.SinOut,
                    _ => Easing.Linear
                });
        }

        protected override bool OnBackButtonPressed()
        {
            System.Diagnostics.Debug.WriteLine($"{nameof(PlaygroundAppShell)}: Back button pressed");
            return base.OnBackButtonPressed();
        }

        private void PlaygroundAppShellLoaded(object sender, EventArgs e)
        {
            this.Window.SubscribeToSafeAreaChanges(OnSafeAreaChanged);
        }

        private void OnSafeAreaChanged(Thickness safeAreaPadding)
        {
            rootContainer.Padding = new Thickness(0, safeAreaPadding.Top, 0, safeAreaPadding.Bottom);
            floatingButtons.Padding = new Thickness(safeAreaPadding.Left, 0, safeAreaPadding.Right, 0);
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
                var tab = new Tab
                {
                    Title = "Orange",
                    Route = nameof(OrangePage)
                };

                tab.Items.Add(
                    new ShellContent()
                    {
                        Title = "Orange",
                        Route = nameof(OrangePage),
                        ContentTemplate = new DataTemplate(typeof(OrangePage))
                    });
                this.Items.Add(tab);

                addButton.IsVisible = false;
            }

            orangeAdded = true;
        }

        private void ShowPopoverButtonClicked(object sender, EventArgs e)
        {
            testBehavior.Test();
            var button = sender as View;
            button.ShowAttachedPopover();
        }

        private void DesignLanguagesListPopoverItemSelected(object sender, ListPopoverItemSelectedEventArgs e)
        {
            if (e.Item is DesignLanguageItem designLanguageItem)
            {
                designLanguagesListPopover.SelectedItem = designLanguageItem;
                designLanguageItem.Action?.Invoke();
            }
        }

        private void SwapButtonClicked(object sender, EventArgs e)
        {
            Content = new SimpleNavigationHost();
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            this.RootPageContainer = null;
        }

        private void SwapSectionButtonClicked(object sender, EventArgs e)
        {
            Resources.TryGetValue("AnotherSimpleShellSectionContainer", out object template);

            SimpleShell.SetShellGroupContainerTemplate(iconsTab, template as DataTemplate);
        }

        private record DesignLanguageItem(string Title, Action Action)
        {
            public override string ToString() => Title;
        }
    }
}
