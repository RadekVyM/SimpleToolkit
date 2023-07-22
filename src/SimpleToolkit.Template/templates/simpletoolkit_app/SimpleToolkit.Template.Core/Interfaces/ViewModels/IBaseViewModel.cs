using System.ComponentModel;

namespace SimpleToolkit.Template.Core.Interfaces.ViewModels;

public interface IBaseViewModel : INotifyPropertyChanged
{
    void OnPropertyChanged(string propertyName);
}