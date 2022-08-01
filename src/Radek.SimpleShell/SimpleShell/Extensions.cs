using Radek.SimpleShell.Handlers;

#if ANDROID || WINDOWS
#endif

using Microsoft.Maui.Platform;

#if ANDROID
using Android.Views;
using AndroidX.Fragment.App;
#endif

namespace Radek.SimpleShell
{
    public static class Extensions
    {
        public static MauiAppBuilder AddSimpleShellHandlers(this MauiAppBuilder builder)
        {
            builder.ConfigureMauiHandlers(handlers =>
            {
//#if ANDROID
//                handlers.AddHandler(typeof(SimpleNavigationHost), typeof(SimpleNavigationHostHandler));
//                handlers.AddHandler(typeof(SimpleShell), typeof(SimpleShellHandler));
//                handlers.AddHandler(typeof(ShellItem), typeof(SimpleShellItemHandler));
//                handlers.AddHandler(typeof(ShellSection), typeof(SimpleShellSectionHandler));
//#elif IOS || MACCATALYST
//#elif WINDOWS
                handlers.AddHandler(typeof(SimpleNavigationHost), typeof(SimpleNavigationHostHandler));
                handlers.AddHandler(typeof(SimpleShell), typeof(SimpleShellHandler));
                handlers.AddHandler(typeof(ShellItem), typeof(SimpleShellItemHandler));
                handlers.AddHandler(typeof(ShellSection), typeof(SimpleShellSectionHandler));
//#endif
            });

            return builder;
        }

#if ANDROID
        internal static LayoutInflater GetLayoutInflater(this IMauiContext mauiContext)
        {
            var layoutInflater = mauiContext.Services.GetService<LayoutInflater>();

            if (layoutInflater == null && mauiContext.Context != null)
            {
                var activity = mauiContext.Context.GetActivity();

                if (activity != null)
                    layoutInflater = LayoutInflater.From(activity);
            }

            return layoutInflater ?? throw new InvalidOperationException("LayoutInflater Not Found");
        }

        internal static FragmentManager GetFragmentManager(this IMauiContext mauiContext)
        {
            var fragmentManager = mauiContext.Services.GetService<FragmentManager>();

            return fragmentManager
                ?? mauiContext.Context?.GetFragmentManager()
                ?? throw new InvalidOperationException("FragmentManager Not Found");
        }
#endif

        internal static SimpleNavigationHost FindSimpleNavigationHost(this IView view)
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

        internal static T FindParentOfType<T>(this Element element, bool includeThis = false)
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

        internal static IEnumerable<Element> GetParentsPath(this Element self)
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
