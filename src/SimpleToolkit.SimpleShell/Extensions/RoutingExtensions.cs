namespace SimpleToolkit.SimpleShell;

internal static class RoutingExtensions
{
    private const string ImplicitPrefix = "IMPL_";
    private const string DefaultPrefix = "D_FAULT_";

    public static bool IsImplicit(BindableObject source)
    {
        return IsImplicit(Routing.GetRoute(source));
    }

    public static bool IsImplicit(string source)
    {
        return source.StartsWith(ImplicitPrefix, StringComparison.Ordinal);
    }
}
