using SimpleToolkit.SimpleShell.Playground.Views.Pages;

namespace SimpleToolkit.SimpleShell.Playground
{
    public partial class NormalAppShell : Shell
    {
        public NormalAppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(FirstYellowDetailPage), typeof(FirstYellowDetailPage));
            Routing.RegisterRoute(nameof(SecondYellowDetailPage), typeof(SecondYellowDetailPage));
            Routing.RegisterRoute(nameof(ThirdYellowDetailPage), typeof(ThirdYellowDetailPage));
            Routing.RegisterRoute(nameof(FirstGreenDetailPage), typeof(FirstGreenDetailPage));
        }
    }
}
