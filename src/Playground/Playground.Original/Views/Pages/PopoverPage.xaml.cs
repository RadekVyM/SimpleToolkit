using SimpleToolkit.Core;

namespace Playground.Original.Views.Pages
{
    public partial class PopoverPage : ContentPage
    {
        public PopoverPage()
        {
            InitializeComponent();
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            var button = sender as View;
            
            button.ShowAttachedPopover();
        }
    }
}
