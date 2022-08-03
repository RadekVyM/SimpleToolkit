using System.Runtime.CompilerServices;

namespace Radek.SimpleShell
{
    public interface ISimpleShell : IFlyoutView, IView, IElement, ITransform, IShellController, IPageController, IVisualElementController, IElementController, IPageContainer<Page>
    {
        IView Content { get; set; }
        ShellItem CurrentItem { get; set; }
    }

    public class SimpleShell : Shell, ISimpleShell
    {
        public static readonly BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(IView), typeof(SimpleShell), propertyChanged: OnContentChanged);

        public virtual IView Content
        {
            get => (IView)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        protected override void OnHandlerChanging(HandlerChangingEventArgs args)
        {
            base.OnHandlerChanging(args);
            //this.SetValue(NavBarIsVisibleProperty, false);
        }

        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {
            if (Shell.GetBackButtonBehavior(CurrentPage) == Shell.BackButtonBehaviorProperty.DefaultValue)
            {
                Shell.SetBackButtonBehavior(CurrentPage, Shell.GetBackButtonBehavior(this));
            }
            base.OnNavigated(args);
        }

        private static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }
    }
}
