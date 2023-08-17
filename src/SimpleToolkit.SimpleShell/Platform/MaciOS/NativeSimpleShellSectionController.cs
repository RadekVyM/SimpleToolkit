#if IOS || MACCATALYST

using Foundation;
using ObjCRuntime;
using UIKit;

namespace SimpleToolkit.SimpleShell.Platform;

// Based on https://github.com/dotnet/maui/blob/main/src/Controls/src/Core/Compatibility/Handlers/Shell/iOS/ShellSectionRenderer.cs

// TODO: Finish the back gesture support

public class NativeSimpleShellSectionController : UINavigationController
{
    TaskCompletionSource<bool> popCompletionTask;


    public NativeSimpleShellSectionController(UIViewController rootViewController) : base(rootViewController)
    {
        Delegate = new NavDelegate(this);

        this.NavigationBarHidden = true;
        this.ToolbarHidden = true;
    }


    public void ReplaceViewControllers(int from, UIViewController[] viewControllers)
    {
        var newViewControllers = ViewControllers;
        newViewControllers = newViewControllers
            .Take(from)
            .Concat(viewControllers)
            .ToArray();
        ViewControllers = newViewControllers;
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

    async void SendPoppedOnCompletion(Task popTask)
    {
        if (popTask == null)
        {
            throw new ArgumentNullException(nameof(popTask));
        }

        //var poppedPage = _shellSection.Stack[_shellSection.Stack.Count - 1];

        // this is used to setup appearance changes based on the incoming page
        //((IShellSectionController)_shellSection).SendPopping(popTask);

        await popTask;

        //DisposePage(poppedPage);
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

            if (popTask != null)
            {
                popTask.TrySetResult(true);
            }
        }

        public override void WillShowViewController(UINavigationController navigationController, [Transient] UIViewController viewController, bool animated)
        {
            var coordinator = viewController.GetTransitionCoordinator();
            if (coordinator != null && coordinator.IsInteractive)
            {
                // handle swipe to dismiss gesture 
                coordinator.NotifyWhenInteractionChanges(OnInteractionChanged);
            }
        }

        void OnInteractionChanged(IUIViewControllerTransitionCoordinatorContext context)
        {
            if (!context.IsCancelled)
            {
                self.popCompletionTask = new TaskCompletionSource<bool>();
                self.SendPoppedOnCompletion(self.popCompletionTask.Task);
            }
        }
    }
}

#endif