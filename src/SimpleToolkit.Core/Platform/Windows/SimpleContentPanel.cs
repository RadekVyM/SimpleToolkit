#if WINDOWS

using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation.Peers;
using Microsoft.UI.Xaml.Automation.Provider;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Path = Microsoft.UI.Xaml.Shapes.Path;

namespace SimpleToolkit.Core.Platform;

public class SimpleContentPanel : Panel, IDisposable
{
    private readonly Path borderPath;
    private IShape borderShape;

    internal Path BorderPath => borderPath;
    internal Func<double, double, Size> CrossPlatformMeasure { get; set; }
    internal Func<Microsoft.Maui.Graphics.Rect, Size> CrossPlatformArrange { get; set; }
    internal Action Clicked { get; set; }


    public SimpleContentPanel()
    {
        borderPath = new Path();
        EnsureBorderPath();

        SizeChanged += ContentPanelSizeChanged;
    }


    public void UpdateBackground(Paint background)
    {
        if (borderPath is null)
            return;

        borderPath.UpdateBackground(background);
    }

    public void UpdateBorderShape(IShape borderShape)
    {
        this.borderShape = borderShape;

        if (borderPath is null)
            return;

        borderPath.UpdateBorderShape(this.borderShape, ActualWidth, ActualHeight);
    }

    public void Dispose()
    {
        Clicked = null;
        CrossPlatformMeasure = null;
        CrossPlatformArrange = null;
        SizeChanged -= ContentPanelSizeChanged;
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new ContentButtonAutomationPeer(this);
    }

    protected override global::Windows.Foundation.Size MeasureOverride(global::Windows.Foundation.Size availableSize)
    {
        if (CrossPlatformMeasure is null)
        {
            return base.MeasureOverride(availableSize);
        }

        var measure = CrossPlatformMeasure(availableSize.Width, availableSize.Height);

        return measure.ToPlatform();
    }

    protected override global::Windows.Foundation.Size ArrangeOverride(global::Windows.Foundation.Size finalSize)
    {
        if (CrossPlatformArrange is null)
        {
            return base.ArrangeOverride(finalSize);
        }

        var width = finalSize.Width;
        var height = finalSize.Height;

        var actual = CrossPlatformArrange(new Microsoft.Maui.Graphics.Rect(0, 0, width, height));

        borderPath?.Arrange(new global::Windows.Foundation.Rect(0, 0, finalSize.Width, finalSize.Height));

        return new global::Windows.Foundation.Size(actual.Width, actual.Height);
    }

    private void ContentPanelSizeChanged(object sender, Microsoft.UI.Xaml.SizeChangedEventArgs e)
    {
        if (borderPath is null || borderShape is null)
            return;

        var strokeThickness = borderPath?.StrokeThickness ?? 0;

        if (ActualWidth <= 0 || ActualHeight <= 0)
            return;

        var pathSize = new Microsoft.Maui.Graphics.Rect(0, 0, ActualWidth + strokeThickness, ActualHeight + strokeThickness);
        var shapePath = borderShape.PathForBounds(pathSize);
        var geometry = shapePath.AsPathGeometry();

        if (borderPath is not null)
        {
            borderPath.Data = geometry;
            borderPath.RenderTransform = new TranslateTransform() { X = -(strokeThickness / 2), Y = -(strokeThickness / 2) };
        }
    }

    internal void EnsureBorderPath()
    {
        if (!Children.Contains(borderPath))
        {
            Children.Add(borderPath);
        }
    }

    private class ContentButtonAutomationPeer : FrameworkElementAutomationPeer, IInvokeProvider
    {
        public ContentButtonAutomationPeer(FrameworkElement owner) : base(owner)
        {
        }

        public void Invoke()
        {
            ((SimpleContentPanel)Owner).Clicked?.Invoke();
        }

        protected override IList<AutomationPeer> GetChildrenCore()
        {
            return null;
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Button;
        }

        protected override object GetPatternCore(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.Invoke)
            {
                return this;
            }

            return base.GetPatternCore(patternInterface);
        }
    }
}

#endif