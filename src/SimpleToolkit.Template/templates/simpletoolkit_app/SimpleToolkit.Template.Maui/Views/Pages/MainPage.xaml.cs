using SimpleToolkit.Template.Core.Interfaces.Services;
using SimpleToolkit.Template.Core.Interfaces.ViewModels;

namespace SimpleToolkit.Template.Maui.Views.Pages;

public partial class MainPage : BaseRootContentPage
{
	public MainPage(INavigationService navigationService, IMainPageViewModel viewModel) : base(navigationService)
	{
		BindingContext = viewModel;

		InitializeComponent();
	}
}