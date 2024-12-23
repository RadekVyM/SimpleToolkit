using SimpleToolkit.SimpleShell;

namespace Playground.Original.Extensions;

public static class ViewExtensions
{
    public static SimpleNavigationHost? FindSimpleNavigationHost(this IView view)
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
                    if (child is not IView childView)
                        continue;

                    var found = FindSimpleNavigationHost(childView);
                    if (found is not null)
                        return found;
                }
                break;
            case IContentView contenView:
                if (contenView.Content is IView content)
                    return FindSimpleNavigationHost(content);
                break;
        }

        return null;
    }
}