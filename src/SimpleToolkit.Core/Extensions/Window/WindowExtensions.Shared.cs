namespace SimpleToolkit.Core
{
    /// <summary>
    /// Helpers for managing the safe area of an application window and helpers for changing the system bars appearance on Android.
    /// </summary>
    public static partial class WindowExtensions
    {
        private static readonly Dictionary<Guid, List<Action<Thickness>>> windowSafeAreaListeners = new Dictionary<Guid, List<Action<Thickness>>>();
        private static readonly Dictionary<Guid, Thickness> safeAreas = new Dictionary<Guid, Thickness>();

        /// <summary>
        /// Subscribes to safe area changes of the window.
        /// </summary>
        /// <param name="window">The window whose safe area changes you want to subscribe.</param>
        /// <param name="listener">The listener that obtains safe area insets of type <see cref="Thickness"/>.</param>
        public static void SubscribeToSafeAreaChanges(this IWindow window, Action<Thickness> listener)
        {
            if (window is not Element elementWindow)
                return;
            
            AddListener(elementWindow.Id, listener);

            listener?.Invoke(GetSafeArea(elementWindow.Id));
        }

        /// <summary>
        /// Unsubscribes from safe area changes of the window.
        /// </summary>
        /// <param name="window">The window whose safe area changes you want to unsubscribe.</param>
        /// <param name="listener">The listener that you want to remove from subscription.</param>
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
