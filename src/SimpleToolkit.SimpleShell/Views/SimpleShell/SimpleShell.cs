﻿using SimpleToolkit.SimpleShell.Extensions;

namespace SimpleToolkit.SimpleShell;

// TODO: It looks like the toolbar is part of ShellSection on iOS. So it is missing in my implementation. How to solve it?
// 1) Lets hope that the toolbar will be handled as on Android and Windows when Shell totally transitions to handler architecture
// 2) Implement it myself in my SimpleShellSectionHandler - This can be a waste of time if 1) comes true
// I am waiting for transition to handler architecture and then I will see
// I have set default visibility of the toolbar to hidden for now

/// <summary>
/// Implementation of <see cref="Shell"/> that lets you define your custom navigation experience. 
/// </summary>
public partial class SimpleShell : Shell, ISimpleShell
{
    const string PageStateNamePrefix = "SimplePageState";
    const string ShellContentStateNamePrefix = "SimpleShellContentState";
    const string ShellSectionStateNamePrefix = "SimpleShellSectionState";
    const string PageTypeStateNamePrefix = "SimplePageTypeState";
    const string PageStatesGroupName = "SimplePageStates";
    const string ShellContentStatesGroupName = "SimpleShellContentStates";
    const string ShellSectionStatesGroupName = "SimpleShellSectionStates";
    const string PageTypeStatesGroupName = "SimplePageTypeStates";

    private bool defaultShellPropertyValuesSet;

    public static readonly BindableProperty ContentProperty =
        BindableProperty.Create(nameof(Content), typeof(IView), typeof(SimpleShell), defaultValueCreator: static (bindable) => new SimpleNavigationHost(), propertyChanged: OnContentChanged);
    public static readonly BindableProperty RootPageContainerProperty =
        BindableProperty.Create(nameof(RootPageContainer), typeof(IView), typeof(SimpleShell), propertyChanged: OnRootPageContainerChanged);
    public static readonly BindableProperty CurrentPageProperty =
        BindableProperty.Create(nameof(CurrentPage), typeof(Page), typeof(SimpleShell), defaultBindingMode: BindingMode.OneWay);
    public static readonly BindableProperty CurrentShellContentProperty =
        BindableProperty.Create(nameof(CurrentShellContent), typeof(ShellContent), typeof(SimpleShell), defaultBindingMode: BindingMode.OneWay);
    public static readonly BindableProperty CurrentShellSectionProperty =
        BindableProperty.Create(nameof(CurrentShellSection), typeof(ShellSection), typeof(SimpleShell), defaultBindingMode: BindingMode.OneWay);
    public static readonly BindableProperty ShellSectionsProperty =
        BindableProperty.Create(nameof(ShellSections), typeof(IReadOnlyList<ShellSection>), typeof(SimpleShell), defaultBindingMode: BindingMode.OneWay);
    public static readonly BindableProperty ShellContentsProperty =
        BindableProperty.Create(nameof(ShellContents), typeof(IReadOnlyList<ShellContent>), typeof(SimpleShell), defaultBindingMode: BindingMode.OneWay);
    public static readonly BindableProperty FlyoutItemsProperty =
        BindableProperty.Create(nameof(FlyoutItems), typeof(IReadOnlyList<FlyoutItem>), typeof(SimpleShell), defaultBindingMode: BindingMode.OneWay);
    public static readonly BindableProperty TabBarsProperty =
        BindableProperty.Create(nameof(TabBars), typeof(IReadOnlyList<TabBar>), typeof(SimpleShell), defaultBindingMode: BindingMode.OneWay);

    public static new SimpleShell Current => (SimpleShell)Shell.Current;

    public static bool UsesPlatformTransitions { get; internal set; }

    public virtual IView Content
    {
        get => (IView)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public virtual IView? RootPageContainer
    {
        get => GetValue(RootPageContainerProperty) as IView;
        set => SetValue(RootPageContainerProperty, value);
    }

    public new Page CurrentPage
    {
        get => (Page)GetValue(CurrentPageProperty);
        private protected set => SetValue(CurrentPageProperty, value);
    }

    public ShellContent CurrentShellContent
    {
        get => (ShellContent)GetValue(CurrentShellContentProperty);
        private protected set => SetValue(CurrentShellContentProperty, value);
    }

    public ShellSection CurrentShellSection
    {
        get => (ShellSection)GetValue(CurrentShellSectionProperty);
        private protected set => SetValue(CurrentShellSectionProperty, value);
    }

    public IReadOnlyList<ShellSection> ShellSections
    {
        get => (IReadOnlyList<ShellSection>)GetValue(ShellSectionsProperty);
        private protected set => SetValue(ShellSectionsProperty, value);
    }

    public IReadOnlyList<ShellContent> ShellContents
    {
        get => (IReadOnlyList<ShellContent>)GetValue(ShellContentsProperty);
        private protected set => SetValue(ShellContentsProperty, value);
    }

    public new IReadOnlyList<FlyoutItem> FlyoutItems
    {
        get => (IReadOnlyList<FlyoutItem>)GetValue(FlyoutItemsProperty);
        private protected set => SetValue(FlyoutItemsProperty, value);
    }

    public IReadOnlyList<TabBar> TabBars
    {
        get => (IReadOnlyList<TabBar>)GetValue(TabBarsProperty);
        private protected set => SetValue(TabBarsProperty, value);
    }


    public SimpleShell()
    {
        UpdateLogicalChildren(null, Content as Element);

        Navigated += SimpleShellNavigated;
        Loaded += SimpleShellLoaded;
    }


    protected override void OnHandlerChanging(HandlerChangingEventArgs args)
    {
        // TODO: If I do not comment this out, I get this exception on Android when I leave the app using the back button and then open the app again:
        // System.InvalidOperationException: 'Handler is already being set elsewhere'
        // But it is really weird because it looks like nothing is called in the OnHandlerChanging() method. Page do not even override it
        // There is also the HandlerChanging event for those who need to do something on handler changing
        // This situation overall causes problems - e.g. when I try to navigate deeper to the stack after coming back to the app, the app crashes with 'pending navigation' exception

        //base.OnHandlerChanging(args);

        SetDefaultShellPropertyValues();
        UpdateVisualStates();
        UpdateItemsCollections();
    }

    private void UpdateVisualStates()
    {
        if (CurrentItem?.CurrentItem is not null)
            UpdateVisualState(CurrentItem.CurrentItem.Route, ShellSectionStateNamePrefix, ShellSectionStatesGroupName);
        
        if (CurrentItem?.CurrentItem?.CurrentItem is not null)
            UpdateVisualState(CurrentItem.CurrentItem.CurrentItem.Route, ShellContentStateNamePrefix, ShellContentStatesGroupName);
        
        if (CurrentPage is not null)
            UpdateVisualState(CurrentPage.GetType().Name, PageStateNamePrefix, PageStatesGroupName);

        var lastPageRoute = CurrentState?.Location?.OriginalString?.Split("/", StringSplitOptions.RemoveEmptyEntries)?.LastOrDefault();
        var pageType = CurrentItem?.CurrentItem?.CurrentItem?.Route == lastPageRoute ? "RootPage" : "RegisteredPage";

        UpdateVisualState(pageType, PageTypeStateNamePrefix, PageTypeStatesGroupName);
    }

    private void UpdateVisualState(string state, string statePrefix, string groupName)
    {
        var group = VisualStateManager.GetVisualStateGroups(this).FirstOrDefault(g => g.Name == groupName);

        if (group is null)
            return;

        var stateName = $"{statePrefix}.{state}";
        if (group.States.Any(s => s.Name == stateName))
            VisualStateManager.GoToState(this, stateName);
        else
            VisualStateManager.GoToState(this, statePrefix);
    }

    private void SetDefaultShellPropertyValues()
    {
        if (defaultShellPropertyValuesSet)
            return;

        if (!IsSet(Shell.BackButtonBehaviorProperty))
        {
            Shell.SetBackButtonBehavior(this, new BackButtonBehavior { IsVisible = false });
        }
        if (!IsSet(Shell.NavBarIsVisibleProperty))
        {
            Shell.SetNavBarIsVisible(this, false);
        }

        defaultShellPropertyValuesSet = true;
    }

    private IEnumerable<ShellSection> GetShellSections()
    {
        var list = new HashSet<ShellSection>();

        foreach (var shellItem in Items)
        {
            var shellSections = shellItem.GetShellSections();
            foreach (var section in shellSections)
                list.Add(section);
        }

        return list;
    }

    private IEnumerable<ShellContent> GetShellContents()
    {
        var list = new HashSet<ShellContent>();

        foreach (var shellItem in Items)
        {
            var shellContents = shellItem.GetShellContents();
            foreach (var content in shellContents)
                list.Add(content);
        }

        return list;
    }

    private void SimpleShellNavigated(object? sender, ShellNavigatedEventArgs e)
    {
        SetDefaultShellPropertyValues();

        CurrentPage = base.CurrentPage;
        CurrentShellSection = CurrentItem.CurrentItem;
        CurrentShellContent = CurrentItem.CurrentItem.CurrentItem;
        // If BackButtonBehavior is not set on CurrentPage, set BackButtonBehavior of the Shell
        if (!CurrentPage.IsSet(Shell.BackButtonBehaviorProperty))
        {
            Shell.SetBackButtonBehavior(CurrentPage, Shell.GetBackButtonBehavior(this));
        }
        if (!CurrentPage.IsSet(Shell.NavBarIsVisibleProperty))
        {
            Shell.SetNavBarIsVisible(CurrentPage, Shell.GetNavBarIsVisible(this));
        }

        UpdateVisualStates();
    }

    private void SimpleShellDescendantChanged(object? sender, ElementEventArgs e)
    {
        // Update collections if the logical structure of the shell changes
        if (e.Element is BaseShellItem)
        {
            UpdateItemsCollections();
        }
    }

    private void SimpleShellLoaded(object? sender, EventArgs e)
    {
        SetDefaultShellPropertyValues();
        UpdateVisualStates();
        UpdateItemsCollections();

        DescendantAdded += SimpleShellDescendantChanged;
        DescendantRemoved += SimpleShellDescendantChanged;
    }

    private void UpdateItemsCollections()
    {
        var newShellSections = GetShellSections().ToList();
        var newShellContents = GetShellContents().ToList();
        var newFlyoutItems = Items.OfType<FlyoutItem>().ToList();
        var newTabBars = Items.OfType<TabBar>().ToList();

        if (ShouldUpdate(ShellSections, newShellSections))
            ShellSections = newShellSections;
        if (ShouldUpdate(ShellContents, newShellContents))
            ShellContents = newShellContents;
        if (ShouldUpdate(FlyoutItems, newFlyoutItems))
            FlyoutItems = newFlyoutItems;
        if (ShouldUpdate(TabBars, newTabBars))
            TabBars = newTabBars;

        static bool ShouldUpdate<T>(IEnumerable<T> items, IEnumerable<T> newItems)
            => items is null || !items.SequenceEqual(newItems);
    }

    private static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var oldView = oldValue as Element;
        var newView = newValue as Element;

        if (bindable is SimpleShell simpleShell)
        {
            simpleShell.UpdateVisualStates();

            simpleShell.UpdateLogicalChildren(oldView, newView);
        }
    }

    private static void OnRootPageContainerChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var oldView = oldValue as Element;
        var newView = newValue as Element;

        if (bindable is SimpleShell simpleShell)
        {
            simpleShell.UpdateLogicalChildren(oldView, newView);
        }
    }

    private void UpdateLogicalChildren(Element? oldView, Element? newView)
    {
        if (oldView is not null)
        {
            oldView.Parent = null;
            RemoveLogicalChild(oldView);
        }

        if (newView is not null)
        {
            newView.Parent = this;
            AddLogicalChild(newView);
        }
    }
}