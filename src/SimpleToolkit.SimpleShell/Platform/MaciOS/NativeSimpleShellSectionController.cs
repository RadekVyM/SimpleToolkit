#if IOS || MACCATALYST

using Foundation;
using ObjCRuntime;
using UIKit;

namespace SimpleToolkit.SimpleShell.Platform;

// Based on https://github.com/dotnet/maui/blob/main/src/Controls/src/Core/Compatibility/Handlers/Shell/iOS/ShellSectionRenderer.cs

// TODO: I should maybe call DisconnectHandler() on page.Handler of pages that are not being reused

public class NativeSimpleShellSectionController : UINavigationController
{
    private SimpleShell shell;
    private TaskCompletionSource<bool> popCompletionTask;
    private bool disposed;


    public NativeSimpleShellSectionController(UIViewController rootViewController, SimpleShell shell)
        : base(rootViewController)
    {
        Delegate = new NavDelegate(this);

        this.NavigationBarHidden = true;
        this.ToolbarHidden = true;
        this.shell = shell;
    }


    public void HandleNewStack(UIViewController[] stack, bool animated)
    {
        var lastInStack = stack[stack.Length - 1];

        if (ViewControllers[ViewControllers.Length - 1] == lastInStack)
        {
            ReplaceViewControllers(1, stack.Skip(1).ToArray());
            return;
        }

        UIViewController lastSame = null;

        for (int i = 0; i < stack.Length; i++)
        {
            if (i < ViewControllers.Length && stack[i] == ViewControllers[i])
                lastSame = stack[i];
            else
                break;
        }

        if (lastSame == lastInStack)
        {
            PopToViewController(lastSame, animated);
        }
        else
        {
            PushViewController(lastInStack, animated);
            ReplaceViewControllers(1, stack.Skip(1).ToArray());
        }
    }

    public override void ViewDidLoad()
    {
        if (disposed)
            return;

        base.ViewDidLoad();

        InteractivePopGestureRecognizer.Delegate = new GestureDelegate(this, ShouldPop);
    }

    public override void ViewDidDisappear(bool animated)
    {
        popCompletionTask?.TrySetResult(false);
        popCompletionTask = null;

        base.ViewDidDisappear(animated);
    }

    protected override void Dispose(bool disposing)
    {
        disposed = true;
        shell = null;
        base.Dispose(disposing);
    }

    private void ReplaceViewControllers(int from, UIViewController[] viewControllers)
    {
        var newViewControllers = ViewControllers;
        newViewControllers = newViewControllers
            .Take(from)
            .Concat(viewControllers)
            .ToArray();
        ViewControllers = newViewControllers;
    }

    private async void SendPoppedOnCompletion(Task<bool> popTask)
    {
        if (popTask is null)
            throw new ArgumentNullException(nameof(popTask));

        // TODO: Is this the right solution?
        if (await popTask)
            await shell?.GoToAsync("..");

        //DisposePage(poppedPage);
    }

    private bool ShouldPop()
    {
        var shellItem = shell.CurrentItem;
        var shellSection = shellItem?.CurrentItem;
        var shellContent = shellSection?.CurrentItem;
        var stack = shellSection?.Stack.ToList();

        stack?.RemoveAt(stack.Count - 1);

        return ((IShellController)shell).ProposeNavigation(ShellNavigationSource.Pop, shellItem, shellSection, shellContent, stack, true);
    }


    class GestureDelegate : UIGestureRecognizerDelegate
    {
        readonly UINavigationController navigationController;
        readonly Func<bool> shouldPop;

        public GestureDelegate(UINavigationController navigationController, Func<bool> shouldPop)
        {
            this.navigationController = navigationController;
            this.shouldPop = shouldPop;
        }

        public override bool ShouldBegin(UIGestureRecognizer recognizer)
        {
            if (navigationController.ViewControllers.Length == 1)
                return false;

            var should = shouldPop();

            System.Diagnostics.Debug.WriteLine($"Should: {should}");

            return should;
        }
    }

    class NavDelegate : UINavigationControllerDelegate
    {
        readonly NativeSimpleShellSectionController self;

        public NavDelegate(NativeSimpleShellSectionController renderer)
        {
            self = renderer;
        }

        // This is currently working around a Mono Interpreter bug
        // if you remove this code please verify that hot restart still works
        // https://github.com/xamarin/Xamarin.Forms/issues/10519
        [Export("navigationController:animationControllerForOperation:fromViewController:toViewController:")]
        [Foundation.Preserve(Conditional = true)]
        public new IUIViewControllerAnimatedTransitioning GetAnimationControllerForOperation(UINavigationController navigationController, UINavigationControllerOperation operation, UIViewController fromViewController, UIViewController toViewController)
        {
            return null;
        }

        public override void DidShowViewController(UINavigationController navigationController, [Transient] UIViewController viewController, bool animated)
        {
            var popTask = self.popCompletionTask;

            popTask?.TrySetResult(true);
        }

        public override void WillShowViewController(UINavigationController navigationController, [Transient] UIViewController viewController, bool animated)
        {
            var coordinator = viewController.GetTransitionCoordinator();
            if (coordinator is not null && coordinator.IsInteractive)
            {
                // handle swipe to dismiss gesture 
                coordinator.NotifyWhenInteractionChanges(OnInteractionChanged);
            }
        }

        void OnInteractionChanged(IUIViewControllerTransitionCoordinatorContext context)
        {
            //System.Diagnostics.Debug.WriteLine($"{context.CompletionVelocity}");

            if (!context.IsCancelled)
            {
                self.popCompletionTask = new TaskCompletionSource<bool>();
                self.SendPoppedOnCompletion(self.popCompletionTask.Task);
            }
        }
    }
}

#endif