namespace SimpleToolkit.SimpleButton;

/// <summary>
/// Arguments of <see cref="SimpleButton"/> events.
/// </summary>
public class SimpleButtonEventArgs : EventArgs
{
    /// <summary>
    /// Position of an interaction relative to the <see cref="SimpleButton"/>
    /// </summary>
    public Point InteractionPosition { get; init; }
}