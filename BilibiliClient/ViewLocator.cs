using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Threading;
using BilibiliClient.ViewModels;

namespace BilibiliClient;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (!Dispatcher.UIThread.CheckAccess())
        {
            return Dispatcher.UIThread.Invoke(() => Build(data));
        }
        
        var fullName = data?.GetType().FullName;
        if (!string.IsNullOrWhiteSpace(fullName))
        {
            var viewTypeName = fullName.Replace("ViewModel", "View");
            var type = Type.GetType(viewTypeName);
            if (type == null || !type.IsSubclassOf(typeof(Control)))
            {
                // viewTypeName = fullName.Replace($"{nameof(Pumpkin)}", $"{nameof(PumpkinDesktop)}").Replace("ViewModel", "View");
                type = Type.GetType(viewTypeName);
            }
            if (type != null)
            {
                var control = this.GetAppRequiredService<Control>(type);
                if (control != null)
                {
                    return control;
                }
                return (Control)Activator.CreateInstance(type)!;
            }
        }

        return new TextBlock { Text = "View Not Found: " + fullName };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}