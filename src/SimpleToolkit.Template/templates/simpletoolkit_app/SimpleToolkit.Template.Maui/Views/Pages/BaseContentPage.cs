using SimpleToolkit.Core;
using SimpleToolkit.Template.Core.Interfaces.Services;
using SimpleToolkit.Template.Core.Interfaces.ViewModels;
using SimpleToolkit.Template.Core.ViewModels.Parameters;
using SimpleToolkit.Template.Maui.Services;

namespace SimpleToolkit.Template.Maui.Views.Pages;

public class BaseContentPage : ContentPage, IQueryAttributable
{
    protected readonly INavigationService navigationService;
    private IParameters parameters;


    public BaseContentPage(INavigationService navigationService)
    {
        this.navigationService = navigationService;

        Loaded += BaseContentPageLoaded;
        Unloaded += BaseContentPageUnloaded;

        SimpleToolkit.SimpleShell.SimpleShell.Current.Navigating += CurrentNavigating;
    }


    private void CurrentNavigating(object sender, ShellNavigatingEventArgs e)
    {
        if (SimpleToolkit.SimpleShell.SimpleShell.Current.CurrentPage == this)
        {
            // Null parameters when navigating to a different ShellContent
            var targetString = e.Target.Location.ToString();

            if (targetString.StartsWith("//") && !targetString.Contains(e.Current.Location.ToString()))
                parameters = null;
        }
    }

    private void BaseContentPageLoaded(object sender, EventArgs e)
    {
        this.Window.SubscribeToSafeAreaChanges(OnSafeAreaChanged);
    }

    private void BaseContentPageUnloaded(object sender, EventArgs e)
    {
        this.Window.UnsubscribeFromSafeAreaChanges(OnSafeAreaChanged);
    }

    protected virtual void OnSafeAreaChanged(Thickness safeArea)
    {
        this.Padding = safeArea;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (BindingContext is IBasePageViewModel viewModel)
        {
            viewModel.OnNavigatedTo();
        }
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);

        if (BindingContext is IBasePageViewModel viewModel)
        {
            viewModel.OnNavigatedFrom();
        }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var isFirst = parameters is null;
        _ = query.TryGetValue(NavigationService.ParametersKey, out object param);

        parameters = param as IParameters ?? new EmptyParameters();

        if (BindingContext is IBasePageViewModel viewModel)
        {
            viewModel.OnApplyParameters(parameters);

            if (isFirst)
                viewModel.OnApplyFirstParameters(parameters);
            else
                viewModel.OnApplyOtherParameters(parameters);
        }
    }
}