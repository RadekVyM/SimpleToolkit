namespace SimpleToolkit.Core;

/// <summary>
/// Arrow directions of a popover.
/// </summary>
[Flags]
public enum PopoverArrowDirection : ulong
{
    Up = 1,
    Down = 2,
    Left = 4,
    Right = 8,
    Any = 15,
    Unknown = ulong.MaxValue
}