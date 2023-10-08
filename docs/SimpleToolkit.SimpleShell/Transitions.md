# `SimpleShell` page transitions

`SimpleShell` allows you to define custom transition animations between pages during navigation.

There are two types of transitions that can be used:

- Platform-specific transitions - transitions provided by the platform-specific APIs and controls. These transitions are used by default.
- Universal transitions - fully cross-platform transitions that are defined using only .NET MAUI APIs.

These two types **cannot** be combined in one application. You can only use one or the other.

## Platform-specific transitions

Platform-specific transitions are transitions provided by the platform-specific APIs and controls:

- [View animations](https://developer.android.com/develop/ui/views/animations/view-animation) and [fragment transactions](https://developer.android.com/guide/fragments/transactions) on Android
- [`UIView` animations](https://developer.apple.com/documentation/uikit/uiview/1622451-animate) and [`UINavigationController` transitions](https://developer.apple.com/documentation/uikit/uiviewcontrolleranimatedtransitioning) on iOS/Mac Catalyst
- [`EntranceThemeTransition`](https://learn.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.animation.entrancethemetransition) and [`NavigationTransitionInfo`](https://learn.microsoft.com/en-us/uwp/api/windows.ui.xaml.media.animation.navigationtransitioninfo) objects and the [`Frame`](https://learn.microsoft.com/en-us/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.controls.frame) control on Windows (WinUI)

There are predefined platform-specific transitions in `SimpleShell`, which are used by default. Defining your own custom platform-specific transitions requires knowledge of the underlying technologies.

By using platform-specific APIs more directly, transition animations of this type are quite a bit smoother than the universal ones. On the other hand, these animations will probably never look the same on all supported platforms.

I recommend choosing this type of transitions when platform-specific look and feel of your transitions is required and when you do not need to take a full control over your transitions appearance.

### Custom platform-specific transitions

Custom platform-specific transition configuration is represented by a `PlatformSimpleShellTransition` object, which contains different properties by platform. The `PlatformSimpleShellTransition` class implements the `ISimpleShellTransition` interface.

Transitions can be defined separately for each state of the navigation. There are three states:

- Switching - new root page (`ShellContent`) is being set
- Pushing - new page is being pushed to the navigation stack
- Popping - existing page is being popped from the navigation stack

These are all the `PlatformSimpleShellTransition` properties by platform:

- **Android**:

  - `SwitchingEnterAnimation` - a method returning the ID of an animation that is applied to the entering page on page switching
  - `SwitchingLeaveAnimation` - a method returning the ID of an animation that is applied to the leaving page on page switching
  - `PushingEnterAnimation` - a method returning the ID of an animation that is applied to the entering page on page pushing
  - `PushingLeaveAnimation` - a method returning the ID of an animation that is applied to the leaving page on page pushing
  - `PoppingEnterAnimation` - a method returning the ID of an animation that is applied to the entering page on page popping
  - `PoppingLeaveAnimation` - a method returning the ID of an animation that is applied to the leaving page on page popping
  - `DestinationPageInFrontOnSwitching` - a method returning whether the destination page should be displayed in front of the origin page on page switching
  - `DestinationPageInFrontOnPushing` - a method returning whether the destination page should be displayed in front of the origin page on page pushing
  - `DestinationPageInFrontOnPopping` - a method returning whether the destination page should be displayed in front of the origin page on page popping

> Visit the official documentation for more information about the [view animations](https://developer.android.com/develop/ui/views/animations/view-animation).

- **iOS/Mac Catalyst**:

  - `DestinationPageInFrontOnSwitching` - a method returning whether the destination page should be displayed in front of the origin page on page switching
  - `SwitchingAnimation` - a method returning a switching animation represented by an action with two parameters for platform views (of type `UIView`) of the origin and destination pages. This is where you change any animatable properties of the platform views. The change will be automatically animated.
  - `SwitchingAnimationDuration` - a method returning a duration of the animation
  - `SwitchingAnimationStarting` - a method returning an action which is called before the animation starts. All preparatory work (such as setting initial values of the animated properties) should be done in this action. This action has two parameters for platform views (of type `UIView`) of the origin and destination pages
  - `SwitchingAnimationFinished` - a method returning an action which is called right after the animation plays. All cleaning work (such as setting the values of the animated properties back to initial values) should be done in this action. This action has two parameters for platform views (of type `UIView`) of the origin and destination pages
  - `PushingAnimation` - a method returning an object of type `IUIViewControllerAnimatedTransitioning`, which represents the animation that is played on page pushing
  - `PoppingAnimation` - a method returning an object of type `IUIViewControllerAnimatedTransitioning`, which represents the animation that is played on page popping

> Visit the official documentation for more information about the [`UIView` animations](https://developer.apple.com/documentation/uikit/uiview/1622451-animate), [`IUIViewControllerAnimatedTransitioning` interface](https://developer.apple.com/documentation/uikit/uiviewcontrolleranimatedtransitioning) and [`UINavigationController` transitions](https://developer.apple.com/documentation/uikit/uinavigationcontroller).

- **Windows (WinUI)**:

  - `SwitchingAnimation` - a method returning an `EntranceThemeTransition` object that is applied on page switching
  - `PushingAnimation` - a method returning an `NavigationTransitionInfo` object that is applied on page pushing
  - `PoppingAnimation` - a method returning an `NavigationTransitionInfo` object that is applied on page popping

> Visit the official WinUI documentation for more information about the [`EntranceThemeTransition`](https://learn.microsoft.com/en-us/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.media.animation.entrancethemetransition) and [`NavigationTransitionInfo`](https://learn.microsoft.com/en-us/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.media.animation.navigationtransitioninfo) classes.

If a property is set to `null`, the default option is used.

#### `SimpleShellTransitionArgs`

Each of these methods takes a `SimpleShellTransitionArgs` object as a parameter. This object provides useful information which can help deciding which animation should be used:

- `OriginPage` of type `VisualElement` - page from which the navigation is initiated
- `DestinationPage` of type `VisualElement` - destination page of the navigation
- `OriginShellSectionContainer` of type `VisualElement` - `ShellSection` container from which the navigation is initiated. Can be `null` if no container is defined
- `DestinationShellSectionContainer` of type `VisualElement` - destination `ShellSection` container of the navigation. Can be `null` if no container is defined
- `OriginShellItemContainer` of type `VisualElement` - `ShellItem` container from which the navigation is initiated. Can be `null` if no container is defined
- `DestinationShellItemContainer` of type `VisualElement` - destination `ShellItem` container of the navigation. Can be `null` if no container is defined
- `Shell` - current instance of `SimpleShell`
- `IsOriginPageRoot` - whether the origin page is a root page
- `IsDestinationPageRoot` - whether the destination page is a root page
- `Progress` - progress of the transition. Number from 0 to 1. For platform-specific transitions, this property is always set to 0
- `TransitionType` - type of the transition that is represented by `SimpleShellTransitionType` enumeration:
  - `Switching` - new root page (`ShellContent`) is being set
  - `Pushing` - new page is being pushed to the navigation stack
  - `Popping` - existing page is being popped from the navigation stack

#### Setting a transition

`PlatformSimpleShellTransition` can be set to any page via the `SimpleShell.Transition` attached property. If you set a transition on your `SimpleShell` object, that transition will be used as the default transition for all pages. Transitions can be set on each page individually.

When navigating from one page to another, **transition of the destination page is played**.

Setting a transition can be simplified using the `SetTransition()` extension method:

```csharp
public static void SetTransition(
    this Page page,
    ISimpleShellTransition transition)
```

#### Example

## Universal transitions

Although, these platform-specific transition animations can be modified, it is quite limited. If you want to take full control over the transitions, you need to disable the platform-specific ones by setting the `usePlatformTransitions` parameter of the `UseSimpleShell()` extension method to `false` and define your own platform-independent animations:

```csharp
builder.UseSimpleShell(usePlatformTransitions: false);
```

Universal transitions are fully cross-platform transitions that are defined using only .NET MAUI APIs. `SimpleShell` comes with no predefined universal transitions.

### `SimpleShellTransition`

Each universal transition is represented by a `SimpleShellTransition` object which contains these read-only properties settable via its constructors:

- `Callback` - a method that is called when progress of the transition changes. Progress of the transition is passed to the method through a parameter of type `SimpleShellTransitionArgs`
- `Starting` - a method that is called when the transition starts
- `Finished` - a method that is called when the transition finishes
- `Duration` - a method returning duration of the transition
- `DestinationPageInFront` - a method returning whether the destination page should be displayed in front of the origin page when navigating from one page to another
- `Easing` - a method returning an easing of the transition animation

Each of these methods takes a `SimpleShellTransitionArgs` object as a parameter. Useful information about currently running transition can be obtained from this object.

The `SimpleShellTransition` class implements the `ISimpleShellTransition` interface.

### Setting a transition

`SimpleShellTransition` can be set to any page via `SimpleShell.Transition` attached property. If you set a transition on your `SimpleShell` object, that transition will be used as the default transition for all pages.

When navigating from one page to another, **transition of the destination page is played**.

#### Extension methods

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

### Example

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

The universal transition looks the same on all platforms:

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

#### Combining transitions

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
