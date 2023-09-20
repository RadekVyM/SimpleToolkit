using SimpleToolkit.SimpleShell.Transitions;

namespace SimpleToolkit.SimpleShell.Extensions
{
    public static class SimpleShellTransitionExtensions
	{
        /// <summary>
        /// Combines two page transitions.
        /// </summary>
        /// <param name="originalTransition">Default page transition</param>
        /// <param name="transition">Page transition which should be combined with the default one</param>
        /// <param name="when">When the second page transition should be used</param>
        /// <returns>New page transition</returns>
		public static SimpleShellTransition CombinedWith(
            this SimpleShellTransition originalTransition,
            SimpleShellTransition transition,
            Func<SimpleShellTransitionArgs, bool> when)
		{
            return new SimpleShellTransition(
				callback: args =>
				{
					if (when?.Invoke(args) ?? false)
						transition?.Callback?.Invoke(args);
					else
						originalTransition?.Callback?.Invoke(args);
				},
				duration: args =>
                {
                    if (when?.Invoke(args) ?? false)
                        return transition?.Duration?.Invoke(args) ?? SimpleShellTransition.DefaultDuration;
                    else
                        return originalTransition?.Duration?.Invoke(args) ?? SimpleShellTransition.DefaultDuration;
                },
				starting: args =>
                {
                    if (when?.Invoke(args) ?? false)
                        transition?.Starting?.Invoke(args);
                    else
                        originalTransition?.Starting?.Invoke(args);
                },
				finished: args =>
                {
                    if (when?.Invoke(args) ?? false)
                        transition?.Finished?.Invoke(args);
                    else
                        originalTransition?.Finished?.Invoke(args);
                },
				destinationPageInFront: args =>
                {
                    if (when?.Invoke(args) ?? false)
                        return transition?.DestinationPageInFront?.Invoke(args) ?? false;
                    else
                        return originalTransition?.DestinationPageInFront?.Invoke(args) ?? false;
                },
                easing: args =>
                {
                    if (when?.Invoke(args) ?? false)
                        return transition?.Easing?.Invoke(args) ?? Easing.Linear;
                    else
                        return originalTransition?.Easing?.Invoke(args) ?? Easing.Linear;
                });
		}
	}
}

