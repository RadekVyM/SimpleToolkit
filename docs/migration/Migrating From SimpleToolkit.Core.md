# Migrating from SimpleToolkit.Core

With the release of .NET 10, the *SimpleToolkit.Core* package has been **split into smaller, more focused packages** to reduce overhead and improve dependency management. Additionally, several features have been deprecated where .NET 10 and the .NET MAUI Community Toolkit now provide superior alternatives.

## 🏗️ The New Architecture

Instead of one *SimpleToolkit.Core* package, the package is now split into feature-specific modules:

* [SimpleToolkit.SimpleButton](../SimpleToolkit.SimpleButton)
* [SimpleToolkit.Helpers](../SimpleToolkit.Helpers)

## 🔄 Mapping the Changes

| Old Feature | Status | Migration Path |
| --- | --- | --- |
| `ContentButton` | **Replaced** | Use `SimpleButton` from the `SimpleToolkit.SimpleButton` package. The API remains identical. |
| `Icon` | **Removed** | Use `IconTintColorBehavior` from the .NET MAUI Community Toolkit. |
| `Popover` | Evaluating options | Keep using `SimpleToolkit.Core` or switch to different solution. |
| Safe area helpers | **Removed** + **Replaced** | Use .NET MAUI `SafeAreaEdges` API and `WindowInsetsProvider.GetInsets()` from the `SimpleToolkit.Helpers` package. |
| System bars helpers | **Removed** | Use `StatusBarBehavior` from the .NET MAUI Community Toolkit. |

## 💡 Upgrade Tips

### Replace `ContentButton` with `SimpleButton`

The `ContentButton` has been moved to the `SimpleToolkit.SimpleButton` package and renamed to `SimpleButton`. While the name has changed, the **API remains fully compatible**.

1. **Initialize**: Register the handlers in your `MauiProgram.cs`:
```csharp
builder.UseSimpleButton();
```

2. **Namespace**: Update your XAML namespace:
```xml
xmlns:sb="clr-namespace:SimpleToolkit.SimpleButton;assembly=SimpleToolkit.SimpleButton"

```

3. **Update Tags**: Replace all occurences of `ContentButton` with `SimpleButton`:
```xml
<sb:SimpleButton
    ...>
    ...
</sb:SimpleButton>
```

### Migrate `Icon` to Community Toolkit

To maintain a similar developer experience while leveraging the .NET MAUI Community Toolkit, you can implement a thin wrapper for the `IconTintColorBehavior`:

1. **Create a Compatibility Wrapper**:
``` csharp
public class Icon : Image
{
	private readonly IconTintColorBehavior tintBehavior;

	public static readonly BindableProperty TintColorProperty =
		BindableProperty.Create(nameof(TintColor), typeof(Color), typeof(Icon), propertyChanged: OnTintColorChanged);

	public virtual Color TintColor
	{
		get => (Color)GetValue(TintColorProperty);
		set => SetValue(TintColorProperty, value);
	}

	public Icon()
	{
		tintBehavior = new IconTintColorBehavior();
		Behaviors.Add(tintBehavior);
	}

	private static void OnTintColorChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is not Icon icon)
			return;

		icon.tintBehavior.TintColor = newValue as Color;
	}
}
```

2. **Swap References**: Point your existing `Icon` XAML namespaces to your local custom control.
