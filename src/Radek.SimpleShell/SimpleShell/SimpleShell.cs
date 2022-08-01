using System.Runtime.CompilerServices;

namespace Radek.SimpleShell
{
    public interface ISimpleShell : IFlyoutView, IView, IElement, ITransform, IShellController, IPageController, IVisualElementController, IElementController, IPageContainer<Page>
    {
        View Content { get; set; }
        ShellItem CurrentItem { get; set; }
    }

    public class SimpleShell : Shell, ISimpleShell
    {
        public static readonly BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(View), typeof(SimpleShell), propertyChanged: OnContentChanged);

        public virtual View Content
        {
            get => (View)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        protected override void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanging(propertyName);
        }

        protected override void OnHandlerChanging(HandlerChangingEventArgs args)
        {
            base.OnHandlerChanging(args);
            //this.SetValue(NavBarIsVisibleProperty, false);
        }

        private static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }
    }
}
