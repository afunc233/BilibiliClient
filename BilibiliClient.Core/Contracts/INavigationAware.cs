namespace BilibiliClient.Core.Contracts;

public interface INavigationAware
{
    void OnNavigatedTo(object? parameter = null);

    void OnNavigatedFrom();
}