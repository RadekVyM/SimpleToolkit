using SimpleToolkit.Template.Core.Interfaces.Services;
using SimpleToolkit.Template.Core;
using SimpleToolkit.Template.Core.Interfaces.ViewModels;

namespace SimpleToolkit.Template.Maui.Services;

public class NavigationService : INavigationService
{
    internal const string ParametersKey = "Parameters";

    private readonly IList<PageType> rootPages = new List<PageType> {
        PageType.MainPage
    };


    public NavigationService()
    {
    }


    public void GoBack()
    {
        Shell.Current.GoToAsync("..");
    }

    public async void GoTo(PageType pageType, IParameters parameters = null)
    {
        await Shell.Current.GoToAsync(GetPageRoute(pageType), true, new Dictionary<string, object>
        {
            [ParametersKey] = parameters
        });
    }

    private string GetPageRoute(PageType pageType)
    {
        if (rootPages.Contains(pageType))
            return $"///{pageType.ToString()}";
        return pageType.ToString();
    }
}