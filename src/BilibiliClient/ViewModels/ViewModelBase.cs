using CommunityToolkit.Mvvm.ComponentModel;

namespace BilibiliClient.ViewModels;

/// <summary>
/// ReactiveObject ？
/// </summary>
public abstract class ViewModelBase : ObservableObject
{
    // protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string? propertyName = null)
    // {
    //     if (EqualityComparer<T>.Default.Equals(field, newValue))
    //     {
    //         return false;
    //     }
    //
    //     this.RaisePropertyChanging(propertyName);
    //     field = newValue;
    //     this.RaisePropertyChanged(propertyName);
    //     return true;
    // }
    //
    // protected void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    // {
    //     ((ReactiveObject)this).RaisePropertyChanged(propertyName);
    // }
}