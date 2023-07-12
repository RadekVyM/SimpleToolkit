using SimpleToolkit.Template.Core.Interfaces.ViewModels;

namespace SimpleToolkit.Template.Core.Interfaces.Services;

public interface INavigationService
{
    void GoTo(PageType pageType, IParameters? parameters = null);
    void GoBack();
}