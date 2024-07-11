using Android.Animation;
using Android.OS;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using AndroidX.Core.View;
using AndroidX.Fragment.App;
using AndroidAnimation = Android.Views.Animations.Animation;
using AnimationSet = Android.Views.Animations.AnimationSet;
using AView = Android.Views.View;

namespace SimpleToolkit.SimpleShell.Platform;

// Based on: https://github.com/dotnet/maui/blob/main/src/Controls/src/Core/Compatibility/Handlers/Shell/Android/ShellContentFragment.cs

public class SimpleFragment : Fragment, AndroidAnimation.IAnimationListener, Animator.IAnimatorListener
{
    private FrameLayout root;
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


    void AndroidAnimation.IAnimationListener.OnAnimationStart(AndroidAnimation animation)
    {
    }

    void AndroidAnimation.IAnimationListener.OnAnimationRepeat(AndroidAnimation animation)
    {
    }

    async void AndroidAnimation.IAnimationListener.OnAnimationEnd(AndroidAnimation animation)
    {
        await OnAnimationEnd();
    }

    void Animator.IAnimatorListener.OnAnimationStart(Animator animation)
    {
    }

    void Animator.IAnimatorListener.OnAnimationRepeat(Animator animation)
    {
    }

    async void Animator.IAnimatorListener.OnAnimationEnd(Animator animation)
    {
        await OnAnimationEnd();
    }

    void Animator.IAnimatorListener.OnAnimationCancel(Animator animation)
    {
    }

    public void ClearAnimationFinished()
    {
        AnimationFinished = null;
    }

    public override void OnResume()
    {
        base.OnResume();
        if (!isAnimating)
            AnimationFinished?.Invoke(this, EventArgs.Empty);
    }

    public override Animator OnCreateAnimator(int transit, bool enter, int nextAnim)
    {
        var result = base.OnCreateAnimator(transit, enter, nextAnim);

        if (result is null && nextAnim != 0)
            result = AnimatorInflater.LoadAnimator(Context, nextAnim);

        if (result is null)
        {
            AnimationFinished?.Invoke(this, EventArgs.Empty);
            return result;
        }

        if (enter)
            View.SetLayerType(LayerType.Hardware, null);

        result.AddListener(this);

        if (OnTop)
        {
            previousZIndex = ViewCompat.GetTranslationZ(View);
            ViewCompat.SetTranslationZ(View, 1000f);
        }

        return result;
    }

    public override AndroidAnimation OnCreateAnimation(int transit, bool enter, int nextAnim)
    {
        var result = base.OnCreateAnimation(transit, enter, nextAnim);
        isAnimating = true;

        if (result is null && nextAnim != 0)
        {
            var name = Resources.GetResourceTypeName(nextAnim);

            if (name == "animator")
                return null;
            else
                result = AnimationUtils.LoadAnimation(Context, nextAnim);
        }

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

    public override AView OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        // The view needs to be placed in a container for translations pivots to work
        var frame = new FrameLayout(Context);
        frame.AddView(view);

        return frame;
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

    private void Destroy()
    {
        root?.RemoveAllViews();
        root?.Dispose();
        root = null;
    }

    private async Task OnAnimationEnd()
    {
        View?.SetLayerType(LayerType.None, null);
        AnimationFinished?.Invoke(this, EventArgs.Empty);
        isAnimating = false;

        await Task.Delay(50);

        if (View is not null)
            ViewCompat.SetTranslationZ(View, previousZIndex);
    }
}