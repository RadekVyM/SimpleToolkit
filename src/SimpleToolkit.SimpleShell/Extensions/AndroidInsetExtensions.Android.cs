using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AView = Android.Views.View;

namespace SimpleToolkit.SimpleShell.Extensions;

public static class AndroidInsetExtensions
{
    [DynamicDependency("SetupViewWithLocalListener", "Microsoft.Maui.Platform.MauiWindowInsetListener", "Microsoft.Maui")]
    public static void SetupViewWithLocalListener(AView view)
    {
        var mauiAssembly = typeof(Microsoft.Maui.MauiAppCompatActivity).Assembly;
        var type = mauiAssembly.GetType("Microsoft.Maui.Platform.MauiWindowInsetListener");

        if (type is not null)
        {
            var method = type.GetMethod("SetupViewWithLocalListener", 
                BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

            method?.Invoke(null, [view, null]);
        }
    }

    [DynamicDependency("RemoveViewWithLocalListener", "Microsoft.Maui.Platform.MauiWindowInsetListener", "Microsoft.Maui")]
    public static void RemoveViewWithLocalListener(AView view)
    {
        var mauiAssembly = typeof(Microsoft.Maui.MauiAppCompatActivity).Assembly;
        var type = mauiAssembly.GetType("Microsoft.Maui.Platform.MauiWindowInsetListener");

        if (type is not null)
        {
            var method = type.GetMethod("RemoveViewWithLocalListener", 
                BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

            method?.Invoke(null, [view]);
        }
    }
}