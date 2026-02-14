using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Automation.Peers;
using Microsoft.UI.Xaml.Automation.Provider;
using Microsoft.UI.Xaml;

namespace SimpleToolkit.SimpleButton.Platform;

public partial class SimpleButtonPanel : ContentPanel
{
    internal Action? Clicked { get; set; }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new SimpleButtonAutomationPeer(this);
    }

    private partial class SimpleButtonAutomationPeer(FrameworkElement owner) : FrameworkElementAutomationPeer(owner), IInvokeProvider
    {
        public void Invoke()
        {
            ((SimpleButtonPanel)Owner).Clicked?.Invoke();
        }

        protected override IList<AutomationPeer>? GetChildrenCore()
        {
            return null;
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Button;
        }

        protected override object GetPatternCore(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.Invoke)
                return this;

            return base.GetPatternCore(patternInterface);
        }
    }
}