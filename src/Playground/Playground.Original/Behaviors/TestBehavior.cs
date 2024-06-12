namespace Playground.Original.Behaviors;

public class TestBehavior : Behavior<View>
{
    View attachedBindable;

    public void Test()
    {
        System.Diagnostics.Debug.WriteLine($"----- MauiContext: {attachedBindable?.Handler?.MauiContext?.ToString()}");
    }

    protected override void OnAttachedTo(View bindable)
    {
        bindable.Loaded += static (sender, e) =>
        {
            if (sender is not View loadedBindable)
                return;

            // Do the work with loadedBindable...

            System.Diagnostics.Debug.WriteLine($"----- Loaded MauiContext: {loadedBindable.Handler?.MauiContext?.ToString()}");
        };
        attachedBindable = bindable;
        base.OnAttachedTo(bindable);
        System.Diagnostics.Debug.WriteLine($"----- Attached MauiContext: {bindable.Handler?.MauiContext?.ToString()}");
    }

    protected override void OnDetachingFrom(View bindable)
    {
        base.OnDetachingFrom(bindable);
        System.Diagnostics.Debug.WriteLine($"----- Detaching MauiContext: {bindable.Handler?.MauiContext?.ToString()}");
        attachedBindable = null;
    }   
}