# `SimpleShell` page transitions

`SimpleShell` allows you to define custom transitions between pages during navigation.

## `SimpleShellTransition`

Each transition is represented by a `SimpleShellTransition` object which contains these read-only properties settable via its constructors:

- `Callback` - method that is called when progress of the transition changes. Progress of the transition is passed to the method through a parameter of type `SimpleShellTransitionArgs`
- `Starting` - method that is called when the transition starts
- `Finished` - method that is called when the transition finishes
- `Duration` - method returning duration of the transition
- `DestinationPageInFront` - method returning whether the destination page should be displayed in front of the origin page when navigating from one page to another
- `Easing` - method returning an easing of the transition animation

Each of these methods takes a `SimpleShellTransitionArgs` object as a parameter. Usefull information about currently running transition can be obtained from this object:

- `OriginPage` of type `VisualElement` - page from which the navigation is initiated
- `DestinationPage` of type `VisualElement` - destination page of the navigation
- `OriginShellSectionContainer` of type `VisualElement` - `ShellSectionContainer` from which the navigation is initiated. Can be `null` if no container is defined
- `DestinationShellSectionContainer` of type `VisualElement` - destination `ShellSectionContainer` of the navigation. Can be `null` if no container is defined
- `Shell` - current instance of `SimpleShell`
- `IsOriginPageRoot` - whether the origin page is a root page
- `IsDestinationPageRoot` - whether the destination page is a root page
- `Progress` - progress of the transition. Number from 0 to 1
- `TransitionType` - type of the transition that is represented by `SimpleShellTransitionType` enumeration:
    - `Switching` - new root page (`ShellContent`) is being set
    - `Pushing` - new page is being pushed to the navigation stack
    - `Popping` - existing page is being popped from the navigation stack

## Setting a transition

`SimpleShellTransition` can be set to any page via `SimpleShell.Transition` attached property. If you set a transition on your `SimpleShell` object, that transition will be used as the default transition for all pages.

When navigating from one page to another, **transition of the destination page is played**.

> Every `ShellContent` needs to be placed inside a `Tab` element to play the transition while navigating between two root pages.

### Extension methods

Setting transition can be simplified using several extension methods. These are headers of the methods:

```csharp
public static void SetTransition(
    this Page page,
    SimpleShellTransition transition)

public static void SetTransition(
    this Page page,
    Action<SimpleShellTransitionArgs> callback,
    Func<SimpleShellTransitionArgs, uint> duration = null,
    Action<SimpleShellTransitionArgs> starting = null,
    Action<SimpleShellTransitionArgs> finished = null,
    Func<SimpleShellTransitionArgs, bool> destinationPageInFront = null,
    Func<SimpleShellTransitionArgs, Easing> easing = null)

public static void SetTransition(
    this Page page,
    Action<SimpleShellTransitionArgs> switchingCallback = null,
    Action<SimpleShellTransitionArgs> pushingCallback = null,
    Action<SimpleShellTransitionArgs> poppingCallback = null,
    Func<SimpleShellTransitionArgs, uint> duration = null,
    Action<SimpleShellTransitionArgs> starting = null,
    Action<SimpleShellTransitionArgs> finished = null,
    Func<SimpleShellTransitionArgs, bool> destinationPageInFront = null,
    Func<SimpleShellTransitionArgs, Easing> easing = null)
```

## Example

The default transition for all pages can be set, for example, in the constructor of your `AppShell`:

```csharp
public AppShell()
{
    InitializeComponent();

    Routing.RegisterRoute(nameof(YellowDetailPage), typeof(YellowDetailPage));

    this.SetTransition(
        callback: static args =>
        {
            switch (args.TransitionType)
            {
                case SimpleShellTransitionType.Switching:
                    if (args.OriginShellSectionContainer == args.DestinationShellSectionContainer)
                    {
                        // Navigatating within the same ShellSection
                        args.OriginPage.Opacity = 1 - args.Progress;
                        args.DestinationPage.Opacity = args.Progress;
                    }
                    else
                    {
                        // Navigatating between different ShellSections
                        (args.OriginShellSectionContainer ?? args.OriginPage).Opacity = 1 - args.Progress;
                        (args.DestinationShellSectionContainer ?? args.DestinationPage).Opacity = args.Progress;
                    }
                    break;
                case SimpleShellTransitionType.Pushing:
                    // Hide the page until it is fully measured
                    args.DestinationPage.Opacity = args.DestinationPage.Width < 0 ? 0.01 : 1;
                    // Slide the page in from right
                    args.DestinationPage.TranslationX = (1 - args.Progress) * args.DestinationPage.Width;
                    break;
                case SimpleShellTransitionType.Popping:
                    // Slide the page out to right
                    args.OriginPage.TranslationX = args.Progress * args.OriginPage.Width;
                    break;
            }
        },
        finished: static args =>
        {
            args.OriginPage.Opacity = 1;
            args.OriginPage.TranslationX = 0;
            args.DestinationPage.Opacity = 1;
            args.DestinationPage.TranslationX = 0;
            if (args.OriginShellSectionContainer is not null)
                args.OriginShellSectionContainer.Opacity = 1;
            if (args.DestinationShellSectionContainer is not null)
                args.DestinationShellSectionContainer.Opacity = 1;
        },
        destinationPageInFront: static args => args.TransitionType switch
        {
            SimpleShellTransitionType.Popping => false,
            _ => true
        },
        duration: static args => args.TransitionType switch
        {
            SimpleShellTransitionType.Switching => 300u,
            _ => 200u
        },
        easing: static args => args.TransitionType switch
        {
            SimpleShellTransitionType.Pushing => Easing.SinIn,
            SimpleShellTransitionType.Popping => Easing.SinOut,
            _ => Easing.Linear
        });
}
```

Output:

https://github.com/RadekVyM/SimpleToolkit/assets/65116078/694efb22-2a1f-4ec2-b169-307499357ae4

Transitions can be set on each page individually. Default transition will be overridden if it is already set:

```csharp
public YellowDetailPage()
{
    InitializeComponent();

    this.SetTransition(
        callback: args => args.DestinationPage.Scale = args.Progress,
        starting: args => args.DestinationPage.Scale = 0,
        finished: args => args.DestinationPage.Scale = 1,
        duration: args => 500u);
}
```

### Combining transitions

Two transitions can be combined into one when you want to use different transitions under different conditions:

```csharp
public YellowDetailPage()
{
    InitializeComponent();

    this.SetTransition(new SimpleShellTransition(
        callback: args => args.DestinationPage.Scale = args.Progress,
        starting: args => args.DestinationPage.Scale = 0,
        finished: args => args.DestinationPage.Scale = 1,
        duration: args => 500u)
        .CombinedWith(
            transition: SimpleShell.Current.GetTransition(),
            when: args => args.TransitionType != SimpleShellTransitionType.Pushing));
}
```

`when` delegate determines when the second `transition` should be used.

> In this example, scale transition is used only when the page is being pushed to the navigation stack, otherwise the default transition defined in the shell is used.
