# Visual states

`SimpleShell` provides multiple groups of visual states.

## `ShellSection` states

States in the `SimpleShellSectionState.[ShellSection route]` format:

```xml
<VisualStateManager.VisualStateGroups>
    <VisualStateGroup x:Name="SimpleShellSectionStates">
        <VisualState x:Name="SimpleShellSectionState.HomeTab">
            <VisualState.Setters>
                <Setter TargetName="tabBar" Property="View.Background" Value="Red"/>
            </VisualState.Setters>
        </VisualState>
        <VisualState x:Name="SimpleShellSectionState.SettingsTab">
            <VisualState.Setters>
                <Setter TargetName="tabBar" Property="View.Background" Value="Green"/>
            </VisualState.Setters>
        </VisualState>
    </VisualStateGroup>
</VisualStateManager.VisualStateGroups>
```

When a user navigates to a tab with a `HomeTab` route, the view named `tabBar` will have a red background, and when to a tab with a `SettingsTab` route, the view named `tabBar` will have a green background. 

## `ShellContent` states

States in the `SimpleShellContentState.[ShellContent route]` format:

```xml
<VisualStateManager.VisualStateGroups>
    <VisualStateGroup x:Name="SimpleShellContentStates">
        <VisualState x:Name="SimpleShellContentState.HomePage">
            <VisualState.Setters>
                <Setter TargetName="tabBar" Property="View.Background" Value="Yellow"/>
            </VisualState.Setters>
        </VisualState>
        <VisualState x:Name="SimpleShellContentState.SettingsPage">
            <VisualState.Setters>
                <Setter TargetName="tabBar" Property="View.Background" Value="Blue"/>
            </VisualState.Setters>
        </VisualState>
    </VisualStateGroup>
</VisualStateManager.VisualStateGroups>
```

When a user navigates to a `ShellContent` with a `HomePage` route, the view named `tabBar` will have a yellow background, and when to a `ShellContent` with a `SettingsPage` route, the view named `tabBar` will have a blue background.

## Page type states

States in the `SimplePageState.[Page type name]` format:

```xml
<VisualStateManager.VisualStateGroups>
    <VisualStateGroup x:Name="SimplePageStates">
        <VisualState x:Name="SimplePageState.HomePage">
            <VisualState.Setters>
                <Setter TargetName="tabBar" Property="View.Background" Value="Purple"/>
            </VisualState.Setters>
        </VisualState>
        <VisualState x:Name="SimplePageState.SettingsPage">
            <VisualState.Setters>
                <Setter TargetName="tabBar" Property="View.Background" Value="Orange"/>
            </VisualState.Setters>
        </VisualState>
    </VisualStateGroup>
</VisualStateManager.VisualStateGroups>
```

When a user navigates to a `HomePage` page, the view named `tabBar` will have a purple background, and when to a `SettingsPage` page, the view named `tabBar` will have a orange background. 

### Navigation stack states

When a user navigates to a page that is part of the shell hierarchy, `SimpleShell` goes to the `RootPage` state, otherwise `SimpleShell` goes to the `RegisteredPage` state:

```xml
<VisualStateManager.VisualStateGroups>
    <VisualStateGroup x:Name="SimplePageTypeStates">
        <VisualState x:Name="SimplePageTypeState.RegisteredPage">
            <VisualState.Setters>
                <Setter TargetName="backButton" Property="Button.IsVisible" Value="true"/>
            </VisualState.Setters>
        </VisualState>
        <VisualState x:Name="SimplePageTypeState.RootPage">
            <VisualState.Setters>
                <Setter TargetName="backButton" Property="Button.IsVisible" Value="false"/>
            </VisualState.Setters>
        </VisualState>
    </VisualStateGroup>
</VisualStateManager.VisualStateGroups>
```
