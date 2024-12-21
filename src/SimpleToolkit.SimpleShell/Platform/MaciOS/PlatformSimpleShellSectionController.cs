using Foundation;
using ObjCRuntime;
using UIKit;

namespace SimpleToolkit.SimpleShell.Platform;

// Based on https://github.com/dotnet/maui/blob/main/src/Controls/src/Core/Compatibility/Handlers/Shell/iOS/ShellSectionRenderer.cs

public record PlatformSimpleShellControllerTransitionPair(IUIViewControllerAnimatedTransitioning? PushingTransition, IUIViewControllerAnimatedTransitioning? PoppingTransition);

public class PlatformSimpleShellSectionController : UINavigationController
{
    private SimpleShell? shell;
    private TaskCompletionSource<bool>? popCompletionTask;
    private readonly Dictionary<UIViewController, TaskCompletionSource<bool>> completionTasks = [];
    private Dictionary<UIViewController, PlatformSimpleShellControllerTransitionPair> transitions = [];
    private bool disposed;


    public PlatformSimpleShellSectionController(UIViewController rootViewController, SimpleShell shell)
        : base(rootViewController)
    {
        Delegate = new ControllerDelegate(this);

        this.NavigationBarHidden = true;
        this.ToolbarHidden = true;
        this.shell = shell;
    }


    public async Task HandleNewStack(UIViewController[] stack, IDictionary<UIViewController, PlatformSimpleShellControllerTransitionPair> newTransitions, bool animated)
    {
        _ = ViewControllers ?? throw new NullReferenceException("ViewControllers should not be null here.");

        var lastInStack = stack[stack.Length - 1];

        UpdateTransitions(newTransitions);

        if (ViewControllers[ViewControllers.Length - 1] == lastInStack)
        {
            ReplaceViewControllers(1, stack.Skip(1).ToArray());
            return;
        }

        UIViewController? lastSame = null;

        for (int i = 0; i < stack.Length; i++)
        {
            if (i < ViewControllers.Length && stack[i] == ViewControllers[i])
                lastSame = stack[i];
            else
                break;
        }

        UIViewController waitingFor;

        if (lastSame == lastInStack)
        {
            waitingFor = lastSame;
            PopToViewController(lastSame, animated);
        }
        else
        {
            waitingFor = lastInStack;
            PushViewController(lastInStack, animated);
            ReplaceViewControllers(1, stack.Skip(1).ToArray());
        }

        var taskSource = completionTasks[waitingFor] = new TaskCompletionSource<bool>();
        var task = taskSource.Task;

        await task;
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
        var sourcesToComplete = new List<TaskCompletionSource<bool>>();

        foreach (var item in completionTasks.Values)
            sourcesToComplete.Add(item);

        completionTasks.Clear();

        foreach (var source in sourcesToComplete)
            source.TrySetResult(false);

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
        var newViewControllers = ViewControllers ?? throw new NullReferenceException("ViewControllers should not be null here.");
        newViewControllers = newViewControllers
            .Take(from)
            .Concat(viewControllers)
            .ToArray();
        ViewControllers = newViewControllers;
    }

    private void UpdateTransitions(IDictionary<UIViewController, PlatformSimpleShellControllerTransitionPair> newTransitions)
    {
        transitions = new(newTransitions);
    }

    private async void SendPoppedOnCompletion(Task<bool> popTask)
    {
        if (popTask is null)
            throw new ArgumentNullException(nameof(popTask));

        // TODO: Is this the right solution?
        if (await popTask && shell is not null)
            await shell.GoToAsync("..");
    }

    private bool ShouldPop()
    {
        var shellItem = shell?.CurrentItem;
        var shellSection = shellItem?.CurrentItem;
        var shellContent = shellSection?.CurrentItem;
        var stack = shellSection?.Stack.ToList();

        stack?.RemoveAt(stack.Count - 1);

        return (shell as IShellController)?.ProposeNavigation(ShellNavigationSource.Pop, shellItem, shellSection, shellContent, stack, true) ?? false;
    }


    class GestureDelegate(UINavigationController navigationController, Func<bool> shouldPop) : UIGestureRecognizerDelegate
    {
        readonly UINavigationController navigationController = navigationController;
        readonly Func<bool> shouldPop = shouldPop;


        public override bool ShouldBegin(UIGestureRecognizer recognizer)
        {
            _ = navigationController.ViewControllers ?? throw new NullReferenceException("ViewControllers should not be null here.");

            if (navigationController.ViewControllers.Length == 1)
                return false;

            return shouldPop?.Invoke() ?? true;
        }
    }

    class ControllerDelegate(PlatformSimpleShellSectionController controller) : UINavigationControllerDelegate
    {
        readonly PlatformSimpleShellSectionController self = controller;


        // This is currently working around a Mono Interpreter bug
        // if you remove this code please verify that hot restart still works
        // https://github.com/xamarin/Xamarin.Forms/issues/10519
        [Export("navigationController:animationControllerForOperation:fromViewController:toViewController:")]
        [Foundation.Preserve(Conditional = true)]
        public new IUIViewControllerAnimatedTransitioning? GetAnimationControllerForOperation(UINavigationController navigationController, UINavigationControllerOperation operation, UIViewController fromViewController, UIViewController toViewController)
        {
            var transitions = self.transitions;

            if (transitions.TryGetValue(toViewController, out var transition))
            {
                return operation switch
                {
                    UINavigationControllerOperation.Pop => transition.PoppingTransition,
                    _ => transition.PushingTransition,
                };
            }
            return null;
        }

        public override void DidShowViewController(UINavigationController navigationController, [Transient] UIViewController viewController, bool animated)
        {
            var tasks = self.completionTasks;
            var popTask = self.popCompletionTask;

            if (tasks.TryGetValue(viewController, out var source))
            {
                source.TrySetResult(true);
                tasks.Remove(viewController);
            }
            else
            {
                popTask?.TrySetResult(true);
            }
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