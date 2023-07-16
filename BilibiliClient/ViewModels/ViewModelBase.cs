using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ReactiveUI;

namespace BilibiliClient.ViewModels;

public class ViewModelBase : ReactiveObject
{
    protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, newValue))
        {
            return false;
        }

        this.RaisePropertyChanging(propertyName);
        field = newValue;
        this.RaisePropertyChanged(propertyName);
        return true;
    }
}