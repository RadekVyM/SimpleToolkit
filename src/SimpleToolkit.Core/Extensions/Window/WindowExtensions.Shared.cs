namespace SimpleToolkit.Core
{
    public static partial class WindowExtensions
    {
        private static readonly Dictionary<Guid, List<Action<Thickness>>> windowSafeAreaListeners = new Dictionary<Guid, List<Action<Thickness>>>();
        private static readonly Dictionary<Guid, Thickness> safeAreas = new Dictionary<Guid, Thickness>();

        public static void SubscribeToSafeAreaChanges(this IWindow window, Action<Thickness> listener)
        {
            if (window is not Element elementWindow)
                return;
            
            AddListener(elementWindow.Id, listener);

            listener?.Invoke(GetSafeArea(elementWindow.Id));
        }

        public static void UnsubscribeFromSafeAreaChanges(this IWindow window, Action<Thickness> listener)
        {
            if (window is Element elementWindow)
                RemoveListener(elementWindow.Id, listener);
        }

        private static void AddListener(Guid windowId, Action<Thickness> listener)
        {
            if (!windowSafeAreaListeners.ContainsKey(windowId))
                windowSafeAreaListeners[windowId] = new List<Action<Thickness>>();

            if (!windowSafeAreaListeners[windowId].Contains(listener))
                windowSafeAreaListeners[windowId].Add(listener);
        }

        private static void RemoveListener(Guid windowId, Action<Thickness> listener)
        {
            if (windowSafeAreaListeners.ContainsKey(windowId))
                windowSafeAreaListeners[windowId].Remove(listener);
        }

        private static Thickness GetSafeArea(Guid windowId)
        {
            if (!safeAreas.ContainsKey(windowId))
                safeAreas[windowId] = new Thickness(0);

            return safeAreas[windowId];
        }

        private static void InvokeListenersIfChanged(Guid windowId, Thickness safeArea)
        {
            if (!safeAreas.ContainsKey(windowId))
                safeAreas[windowId] = new Thickness(0);

            if (safeAreas[windowId] == safeArea)
                return;

            safeAreas[windowId] = safeArea;

            if (!windowSafeAreaListeners.ContainsKey(windowId))
                windowSafeAreaListeners[windowId] = new List<Action<Thickness>>();

            foreach (var listener in windowSafeAreaListeners[windowId])
            {
                listener?.Invoke(safeAreas[windowId]);
            }
        }
    }
}
