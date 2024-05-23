#if ANDROID

using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Platform;
using AView = Android.Views.View;

namespace SimpleToolkit.Core.Platform;

public class SimplePopupWindow : PopupWindow
{
    private readonly IMauiContext mauiContext;

    public IPopover VirtualView { get; private set; }


    public SimplePopupWindow(Context context, IMauiContext mauiContext) : base(context)
    {
        this.mauiContext = mauiContext ?? throw new ArgumentNullException(nameof(mauiContext));

        OutsideTouchable = true;
        Focusable = true;
        Elevation = 0;
        Width = WindowManagerLayoutParams.WrapContent;
        Height = WindowManagerLayoutParams.WrapContent;

        SetBackgroundDrawable(new ColorDrawable(Colors.Transparent.ToPlatform()));
    }


    public AView SetElement(IPopover element)
    {
        ArgumentNullException.ThrowIfNull(element);

        VirtualView = element;

        SetContent(VirtualView);

        return ContentView;
    }

    public void Show(IElement anchor)
    {
        if (VirtualView is null)
        {
            return;
        }

        var platformAnchor = anchor?.ToPlatform(mauiContext) ?? GetDefaultAnchor();
        var xOffset = GetXOffset(platformAnchor);

        ShowAsDropDown(platformAnchor, xOffset, 0, GravityFlags.Center);
    }

    private int GetXOffset(AView platformAnchor)
    {
        if (VirtualView.HorizontalAlignment is HorizontalAlignment.Left)
            return 0;
        
        ContentView?.Measure((int)MeasureSpecMode.Unspecified, (int)MeasureSpecMode.Unspecified);
        var offset = -((ContentView?.MeasuredWidth ?? 0) - platformAnchor.Width);

        if (VirtualView.HorizontalAlignment is HorizontalAlignment.Right)
            return offset;

        return offset / 2;
    }

    private AView GetDefaultAnchor()
    {
        ArgumentNullException.ThrowIfNull(VirtualView.Parent);
        return VirtualView.Parent.ToPlatform(mauiContext);
    }

    public void Hide()
    {
        Dismiss();
    }

    public void CleanUp()
    {
        VirtualView = null;
    }

    public void SetContent(IPopover popover)
    {
        if (popover.Content is null)
            return;

        var content = popover.Content.ToPlatform(mauiContext);
        ContentView = content;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            VirtualView = null;
        }

        base.Dispose(disposing);
    }
}

#endif