using System.Linq;
using System.Threading.Tasks;
using AvaFFmpegPlayer;
using AvaFFmpegPlayer.Controls;
using AvaFFmpegPlayer.FFmpeg;
using BilibiliClient.Core.Contracts;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Models.Https.Api;
using BilibiliClient.Core.Models.Https.App;
using BilibiliClient.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BilibiliClient.ViewModels;

public partial class PlayerPageViewModel : ViewModelBase, IPageViewModel
{
    NavBarType IPageViewModel.NavBarType => NavBarType.None;

    public ViewModelBase? Header { get; protected init; }

    [ObservableProperty] private string _title = "加载中...";
    [ObservableProperty] private bool _isLoading;

    [ObservableProperty] private VideoPlayUrlResult? _videoPlayUrlResult;

    private readonly IPlayerService _playerService;

    private MediaContainer? _container;
    private AvaPresenter? _presenter;

    public PlayerPageViewModel(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    async Task INavigationAware.OnNavigatedTo(object? parameter)
    {
        await Task.CompletedTask;
        if (parameter is RecommendCardItem recommendCardItem)
        {
            VideoPlayUrlResult = await _playerService.GetPlayUrl(recommendCardItem);
        }
    }

    async Task INavigationAware.OnNavigatedFrom()
    {
        _presenter?.Stop();
        _presenter?.Container?.Close();
        await Task.CompletedTask;
    }

    public const string DefaultUserAgentString =
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36 Edg/92.0.902.62";

    public const string Referer = "https://www.bilibili.com";

    partial void OnVideoPlayUrlResultChanging(VideoPlayUrlResult? oldValue, VideoPlayUrlResult? newValue)
    {
        if (newValue != null)
        {
            _presenter = new AvaPresenter()
            {
                Window = _videoView,
            };

            var options = new ProgramOptions()
            {
                InputFileName =
                    newValue.Dash.Video.FirstOrDefault(it => !string.IsNullOrWhiteSpace(it.BaseUrl)).BaseUrl,
            };
            // var options = new ProgramOptions()
            // {
            //     InputFileName = @"D:\Steam\steamapps\workshop\content\431960\2952911832\top 100 endless engines 4k.mp4",
            // };

            options.FormatOptions["user_agent"] = DefaultUserAgentString;
            options.FormatOptions["referer"] = Referer;

            _container = MediaContainer.Open(options, _presenter);
            _presenter.Start();
        }
    }

    private VideoView? _videoView;

    public void SetVideoView(VideoView videoView)
    {
        _videoView = videoView;
    }
}