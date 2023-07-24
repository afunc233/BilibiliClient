using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;

namespace BilibiliClient.ViewModels;

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