using SimpleToolkit.SimpleShell.Transitions;

namespace SimpleToolkit.SimpleShell
{
	public partial class SimpleShell
	{
        public static readonly BindableProperty TransitionProperty =
            BindableProperty.CreateAttached("Transition", typeof(SimpleShellTransition), typeof(Page), null);

        public static readonly BindableProperty ShellSectionContainerTemplateProperty =
            BindableProperty.CreateAttached("ShellSectionContainerTemplate", typeof(DataTemplate), typeof(ShellSection), null, propertyChanged: OnShellSectionContainerTemplateChanged);

        public static readonly BindableProperty ShellSectionContainerProperty =
            BindableProperty.CreateAttached("ShellSectionContainer", typeof(IView), typeof(ShellSection), null, propertyChanged: OnShellSectionContainerChanged);

        public static SimpleShellTransition GetTransition(BindableObject item)
        {
            _ = item ?? throw new ArgumentNullException(nameof(item));
            return (SimpleShellTransition)item.GetValue(TransitionProperty);
        }

        public static void SetTransition(BindableObject item, SimpleShellTransition value)
        {
            _ = item ?? throw new ArgumentNullException(nameof(item));
            item.SetValue(TransitionProperty, value);
        }

        public static DataTemplate GetShellSectionContainerTemplate(BindableObject item)
        {
            _ = item ?? throw new ArgumentNullException(nameof(item));
            return (DataTemplate)item.GetValue(ShellSectionContainerTemplateProperty);
        }

        public static void SetShellSectionContainerTemplate(BindableObject item, DataTemplate value)
        {
            _ = item ?? throw new ArgumentNullException(nameof(item));
            item.SetValue(ShellSectionContainerTemplateProperty, value);
        }

        public static IView GetShellSectionContainer(BindableObject item)
        {
            _ = item ?? throw new ArgumentNullException(nameof(item));
            return (IView)item.GetValue(ShellSectionContainerProperty);
        }

        public static void SetShellSectionContainer(BindableObject item, IView value)
        {
            _ = item ?? throw new ArgumentNullException(nameof(item));
            item.SetValue(ShellSectionContainerProperty, value);
        }


        private static void OnShellSectionContainerTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var section = bindable as ShellSection;

            if (section.IsSet(SimpleShell.ShellSectionContainerProperty))
            {
                SimpleShell.SetShellSectionContainer(section, null);
            }
        }

        private static void OnShellSectionContainerChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }
    }
}

