namespace BilibiliClient.Core.Contracts;

public interface INavigationAware
{
    Task OnNavigatedTo(object? parameter = null);

    Task OnNavigatedFrom();
}