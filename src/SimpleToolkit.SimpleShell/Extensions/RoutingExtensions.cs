namespace SimpleToolkit.SimpleShell
{
    internal static class RoutingExtensions
    {
        const string ImplicitPrefix = "IMPL_";
        const string DefaultPrefix = "D_FAULT_";
        internal const string PathSeparator = "/";

        public static bool IsImplicit(BindableObject source)
        {
            return IsImplicit(Routing.GetRoute(source));
        }

        public static bool IsImplicit(string source)
        {
            return source.StartsWith(ImplicitPrefix, StringComparison.Ordinal);
        }
    }
}
