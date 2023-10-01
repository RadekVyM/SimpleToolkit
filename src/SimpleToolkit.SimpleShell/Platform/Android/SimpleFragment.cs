#if ANDROID

using Android.OS;
using Android.Views;
using Android.Views.Animations;
using AndroidX.Core.View;
using AndroidX.Fragment.App;
using Microsoft.Maui.Controls.Platform.Compatibility;
using AndroidAnimation = Android.Views.Animations.Animation;
using AnimationSet = Android.Views.Animations.AnimationSet;
using AView = Android.Views.View;

namespace SimpleToolkit.SimpleShell.Platform;

// Based on: https://github.com/dotnet/maui/blob/main/src/Controls/src/Core/Compatibility/Handlers/Shell/Android/ShellContentFragment.cs

public class SimpleFragment : Fragment, AndroidAnimation.IAnimationListener
{
    private CustomFrameLayout root;
    private AView view;
    private bool disposed;
    private bool isAnimating = false;
    private float previousZIndex = 0f;

    public bool OnTop { get; set; }
    public event EventHandler AnimationFinished;


    public SimpleFragment(AView view)
    {
        this.view = view;
    }


    async void AndroidAnimation.IAnimationListener.OnAnimationEnd(AndroidAnimation animation)
    {
        View?.SetLayerType(LayerType.None, null);
        AnimationFinished?.Invoke(this, EventArgs.Empty);
        isAnimating = false;

        await Task.Delay(50);

        if (View is not null)
            ViewCompat.SetTranslationZ(View, previousZIndex);
    }

    public override void OnResume()
    {
        base.OnResume();
        if (!isAnimating)
            AnimationFinished?.Invoke(this, EventArgs.Empty);
    }

    public override AndroidAnimation OnCreateAnimation(int transit, bool enter, int nextAnim)
    {
        var result = base.OnCreateAnimation(transit, enter, nextAnim);
        isAnimating = true;

        if (result is null && nextAnim != 0)
            result = AnimationUtils.LoadAnimation(Context, nextAnim);

        if (result is null)
        {
            AnimationFinished?.Invoke(this, EventArgs.Empty);
            return result;
        }

        // we only want to use a hardware layer for the entering view because its quite likely
        // the view exiting is animating a button press of some sort. This means lots of GPU
        // transactions to update the texture.
        if (enter)
            View.SetLayerType(LayerType.Hardware, null);

        // This is very strange what we are about to do. For whatever reason if you take this animation
        // and wrap it into an animation set it will have a 1 frame glitch at the start where the
        // fragment shows at the final position. That sucks. So instead we reach into the returned
        // set and hook up to the first item. This means any animation we use depends on the first item
        // finishing at the end of the animation.

        if (result is AnimationSet set)
            set.Animations[0].SetAnimationListener(this);

        if (OnTop)
        {
            previousZIndex = ViewCompat.GetTranslationZ(View);
            ViewCompat.SetTranslationZ(View, 1000f);
        }

        return result;
    }

    public void OnAnimationRepeat(AndroidAnimation animation)
    {
    }

    public void OnAnimationStart(AndroidAnimation animation)
    {
    }

    public override AView OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        return view;
    }

    private void Destroy()
    {
        root?.RemoveAllViews();
        root?.Dispose();
        root = null;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposed)
            return;

        disposed = true;
        if (disposing)
        {
            Destroy();
            view = null;
        }

        base.Dispose(disposing);
    }

    // Use OnDestroy instead of OnDestroyView because OnDestroyView will be
    // called before the animation completes. This causes tons of tiny issues.
    public override void OnDestroy()
    {
        base.OnDestroy();
        Destroy();
    }
}

#endif