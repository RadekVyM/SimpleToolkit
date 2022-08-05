namespace Radek.SimpleShell.Controls
{
    public partial class TabView
    {
        private void UpdateValuesToCupertino()
        {
            iconSize = new Size(15, 15);
            iconMargin = new Thickness(0, 0, 0, 15);
            buttonPadding = new Thickness(0, 20, 0, 0);
            tabViewHeight = 60;
            buttonTextTransform = TextTransform.None;
        }

        private void UpdateDrawableToCupertino()
        {

        }
    }
}
