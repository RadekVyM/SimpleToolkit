namespace SimpleToolkit.Core
{
    /// <summary>
    /// Arguments of <see cref="ContentButton"/> events.
    /// </summary>
    public class ContentButtonEventArgs : EventArgs
    {
        /// <summary>
        /// Position of an interaction relative to the <see cref="ContentButton"/>
        /// </summary>
        public Point InteractionPosition { get; init; }
    }
}
