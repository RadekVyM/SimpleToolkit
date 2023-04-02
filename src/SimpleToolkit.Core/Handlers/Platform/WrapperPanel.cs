#if WINDOWS

using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WRect = Windows.Foundation.Rect;

// Based on https://github.com/CommunityToolkit/Maui/blob/main/src/CommunityToolkit.Maui.Core/Views/Popup/MauiPopup.android.cs

namespace SimpleToolkit.Core.Handlers
{
    internal class WrapperPanel : Panel
    {
        private View view;
        private FrameworkElement frameworkElement;
        private IPlatformViewHandler handler => view.Handler as IPlatformViewHandler;
        
        
        public WrapperPanel(View view, IMauiContext mauiContext)
        {
            ArgumentNullException.ThrowIfNull(view);
            ArgumentNullException.ThrowIfNull(mauiContext);

            this.view = view;
            this.view.MeasureInvalidated += OnMeasureInvalidated;

            frameworkElement = view.ToPlatform(mauiContext);
            Children.Add(frameworkElement);

            // make sure we re-measure once the template is applied

            frameworkElement.Loaded += (sender, args) =>
            {
                // If the view is a layout (stacklayout, grid, etc) we need to trigger a layout pass
                // with all the controls in a consistent native state (i.e., loaded) so they'll actually
                // have Bounds set
                handler?.PlatformView?.InvalidateMeasure(view);
                InvalidateMeasure();
            };
        }


        public void CleanUp()
        {
            view.MeasureInvalidated -= OnMeasureInvalidated;
            view = null;
            frameworkElement = null;
        }

        protected override global::Windows.Foundation.Size ArrangeOverride(global::Windows.Foundation.Size finalSize)
        {
            view.Frame = new Rect(0, 0, finalSize.Width, finalSize.Height);
            frameworkElement?.Arrange(new WRect(0, 0, finalSize.Width, finalSize.Height));

            if (view.Width <= 0 || view.Height <= 0)
                // Hide Panel when size _view is empty.
                // It is necessary that this element does not overlap other elements when it should be hidden.
                Opacity = 0;
            else
                Opacity = 1;

            return finalSize;
        }

        protected override global::Windows.Foundation.Size MeasureOverride(global::Windows.Foundation.Size availableSize)
        {
            frameworkElement.Measure(availableSize);

            var request = frameworkElement.DesiredSize;

            if (request.Height < 0)
                request.Height = availableSize.Height;

            global::Windows.Foundation.Size result;
            if (view.HorizontalOptions.Alignment == Microsoft.Maui.Controls.LayoutAlignment.Fill && !double.IsInfinity(availableSize.Width) && availableSize.Width != 0)
                result = new global::Windows.Foundation.Size(availableSize.Width, request.Height);
            else
                result = request;

            return result;
        }

        private void OnMeasureInvalidated(object sender, EventArgs e)
        {
            InvalidateMeasure();
        }
    }
}

#endif