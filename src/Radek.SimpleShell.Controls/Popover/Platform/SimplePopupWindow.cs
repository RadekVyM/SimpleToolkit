#if ANDROID

using Android.Content;
using Android.Graphics.Drawables;
using Android.Widget;
using Microsoft.Maui.Platform;
using AView = Android.Views.View;

namespace Radek.SimpleShell.Controls.Platform
{
    public class SimplePopupWindow : PopupWindow
    {
        readonly IMauiContext mauiContext;

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

            var container = popup.Content.ToPlatform(mauiContext);
            ContentView = container;
        }
    }
}

#endif