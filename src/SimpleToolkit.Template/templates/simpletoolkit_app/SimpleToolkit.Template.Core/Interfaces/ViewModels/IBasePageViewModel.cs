namespace SimpleToolkit.Template.Core.Interfaces.ViewModels;

public interface IBasePageViewModel : IBaseViewModel
{
    void OnNavigatedTo();
    void OnNavigatedFrom();
    void OnApplyParameters(IParameters parameters);
    void OnApplyFirstParameters(IParameters parameters);
    void OnApplyOtherParameters(IParameters parameters);
}