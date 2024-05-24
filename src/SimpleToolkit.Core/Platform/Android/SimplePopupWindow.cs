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
    private IElement anchor;

    public IPopover VirtualView { get; private set; }


    public SimplePopupWindow(Context context, IMauiContext mauiContext) : base(context)
    {
        this.mauiContext = mauiContext ?? throw new ArgumentNullException(nameof(mauiContext));

        OutsideTouchable = true;
        Focusable = true;

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
            return;

        this.anchor = anchor;

        var (width, height, xOffset, platformAnchor) = GetWindowSpecs(anchor);

        // Height and Width have to be set to not show the PopupWindow outside of screen bounds
        if (width != 0 || height != 0)
        {
            Width = width;
            Height = height;
        }

        ShowAsDropDown(platformAnchor, xOffset, 0, GravityFlags.Center);
    }

    public void Hide()
    {
        ClearLayoutChangeListener(VirtualView);
        anchor = null;
        Dismiss();
    }

    public void CleanUp()
    {
        ClearLayoutChangeListener(VirtualView);
        anchor = null;
        VirtualView = null;
    }

    public void SetContent(IPopover popover)
    {
        if (popover.Content is null)
            return;

        var content = popover.Content.ToPlatform(mauiContext);

        ClearLayoutChangeListener(popover);
        content.LayoutChange += OnLayoutChange;

        ContentView = content;
    }

    private void ClearLayoutChangeListener(IPopover popover)
    {
        if (popover is null)
            return;

        var content = popover.Content.ToPlatform(mauiContext);
        content.LayoutChange -= OnLayoutChange;
    }

    private void OnLayoutChange(object sender, AView.LayoutChangeEventArgs e)
    {
        if (anchor is null)
            return;

        var (width, height, xOffset, platformAnchor) = GetWindowSpecs(anchor);

        // If the popover size changes while open, dimensions have to be updated manually
        if (width != Width || height != Height)
            Update(platformAnchor, xOffset, 0, width, height);
    }

    private (int width, int height, int xOffset, AView platformAnchor) GetWindowSpecs(IElement anchor)
    {
        var platformAnchor = anchor?.ToPlatform(mauiContext) ?? GetDefaultAnchor();
        var measure = (VirtualView.Content as IView).Measure(double.PositiveInfinity, double.PositiveInfinity);
        var density = DeviceDisplay.Current.MainDisplayInfo.Density;
        var width = (int)Math.Round(measure.Width * density);
        var height = (int)Math.Round(measure.Height * density);
        var xOffset = GetXOffset(platformAnchor, width);

        return (width, height, xOffset, platformAnchor);
    }

    private int GetXOffset(AView platformAnchor, int windowWidth)
    {
        if (VirtualView.HorizontalAlignment is HorizontalAlignment.Left)
            return 0;
        
        var offset = platformAnchor.Width - windowWidth;

        if (VirtualView.HorizontalAlignment is HorizontalAlignment.Right)
            return offset;

        return offset / 2;
    }

    private AView GetDefaultAnchor()
    {
        ArgumentNullException.ThrowIfNull(VirtualView.Parent);
        return VirtualView.Parent.ToPlatform(mauiContext);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            CleanUp();

        base.Dispose(disposing);
    }
}

#endif