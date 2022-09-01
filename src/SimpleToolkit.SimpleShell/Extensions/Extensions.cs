namespace SimpleToolkit.SimpleShell
{
    internal static class Extensions
    {
        public static SimpleNavigationHost FindSimpleNavigationHost(this IView view)
        {
            if (view is null)
                return null;

            switch (view)
            {
                case SimpleNavigationHost navigationHost:
                    return navigationHost;
                case IBindableLayout layout:
                    foreach (var child in layout.Children)
                    {
                        var found = FindSimpleNavigationHost(child as IView);
                        if (found is not null)
                            return found;
                    }
                    break;
                case IContentView contenView:
                    return FindSimpleNavigationHost(contenView.Content as IView);
            }

            return null;
        }

        public static T FindParentOfType<T>(this Element element, bool includeThis = false)
            where T : IElement
        {
            if (includeThis && element is T view)
                return view;

            foreach (var parent in element.GetParentsPath())
            {
                if (parent is T parentView)
                    return parentView;
            }

            return default;
        }

        public static IEnumerable<Element> GetParentsPath(this Element self)
        {
            Element current = self;

            while (!(current.RealParent == null || current.RealParent is IApplication))
            {
                current = current.RealParent;
                yield return current;
            }
        }
    }
}
