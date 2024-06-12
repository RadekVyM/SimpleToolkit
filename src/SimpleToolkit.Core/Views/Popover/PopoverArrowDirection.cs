namespace SimpleToolkit.Core;

/// <summary>
/// Arrow directions of a popover.
/// </summary>
[Flags]
public enum PopoverArrowDirection
{
    Unknown = 0,
    Up = 1,
    Down = 2,
    Left = 4,
    Right = 8,
    Any = 15
}