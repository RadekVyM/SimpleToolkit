#if ANDROID

using Android.Content;
using Android.Graphics.Drawables;
using Android.Widget;
using Microsoft.Maui;
using Microsoft.Maui.Platform;
using AView = Android.Views.View;
using Math = System.Math;

namespace SimpleToolkit.Core.Handlers
{
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

            var measure = (VirtualView.Content as IView).Measure(double.PositiveInfinity, double.PositiveInfinity);
            var width = (int)Math.Round(measure.Width * DeviceDisplay.Current.MainDisplayInfo.Density);
            var height = (int)Math.Round(measure.Height * DeviceDisplay.Current.MainDisplayInfo.Density);

            //var content = VirtualView.Content.ToPlatform(mauiContext);
            //content.Measure(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

            //content.Post(() =>
            //{
            //    VirtualView.Content.Layout(new Rect(0, 0, width, height));
            //});

            // Height and Width have to be set to not show the PopupWindow outside of screen bounds
            if (width != 0 || height != 0)
            {
                Width = width;
                Height = height;
                VirtualView.Content.Layout(new Rect(0, 0, width, height));
            }

            if (anchor is not null)
            {
                var platformAnchor = anchor.ToPlatform(mauiContext);
                ShowAsDropDown(platformAnchor);
            }
            else
            {
                ArgumentNullException.ThrowIfNull(VirtualView.Parent);
                var platformAnchor = VirtualView.Parent.ToPlatform(mauiContext);
                ShowAsDropDown(platformAnchor);
            }
        }

        public void Hide()
        {
            Dismiss();
        }

        public void CleanUp()
        {
            VirtualView = null;
        }

        public void SetContent(IPopover popup)
        {
            if (popup.Content is null)
                return;

            var content = popup.Content.ToPlatform(mauiContext);
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
}

#endif