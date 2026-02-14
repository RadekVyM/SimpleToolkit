# SimpleToolkit.Helpers

A collection of essential cross-platform helpers for .NET MAUI to bridge the gap between shared code and native platform APIs.

## 📐 WindowInsetsProvider

Provides a unified way to retrieve current window insets (Safe Area on iOS/MacCatalyst and System Bars/Display Cutouts on Android). This ensures your UI doesn't get hidden behind notches, status bars, or navigation bars:

```csharp
Thickness insets = WindowInsetsProvider.GetInsets();
```