using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AvaFFmpegPlayer;
using AvaFFmpegPlayer.Controls;
using AvaFFmpegPlayer.FFmpeg;
using Bilibili.App.Card.V1;
using BilibiliClient.Core.Contracts;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Models.Https.Api;
using BilibiliClient.Core.Models.Https.App;
using BilibiliClient.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BilibiliClient.ViewModels;

public partial class PlayerPageViewModel(IPlayerService playerService) : ViewModelBase, IPageViewModel
{
    NavBarType IPageViewModel.NavBarType => NavBarType.None;

    public ViewModelBase? Header { get; protected init; }

    [ObservableProperty] private string _title = "加载中...";
    [ObservableProperty] private bool _isLoading;

    [ObservableProperty] private VideoPlayUrlResult? _videoPlayUrlResult;

    private readonly IPlayerService _playerService = playerService;

    private MediaContainer? _container;
    private AvaPresenter? _presenter;

    async Task INavigationAware.OnNavigatedTo(object? parameter)
    {
        await Task.CompletedTask;
        if (parameter is RecommendCardItem recommendCardItem)
        {
            VideoPlayUrlResult = await _playerService.GetPlayUrl(recommendCardItem);
        }
        else if (parameter is Card card)
        {
            VideoPlayUrlResult = await _playerService.GetPlayUrl(card);
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

    async partial void OnVideoPlayUrlResultChanging(VideoPlayUrlResult? oldValue, VideoPlayUrlResult? newValue)
    {
        if (_container != null)
        {
            _container.Close();
            _container = null;
        }
        if (newValue != null)
        {
            await Task.CompletedTask;
            _presenter = new AvaPresenter()
            {
                Window = _videoView,
            };

            var options = new ProgramOptions()
            {
                InputVideoPath =
                    newValue.Dash?.Video?.FirstOrDefault(it => !string.IsNullOrWhiteSpace(it.BaseUrl))?.BaseUrl,
                // InputAudioPath = newValue.Dash.Audio.FirstOrDefault(it => !string.IsNullOrWhiteSpace(it.BaseUrl))
                //     .BaseUrl
            };
            // var options = new ProgramOptions()
            // {
            //     InputFileName = @"D:\Steam\steamapps\workshop\content\431960\2952911832\top 100 endless engines 4k.mp4",
            // };

            options.FormatOptions["probesize"] = (50 * (long)1024 * 1024).ToString();
            options.FormatOptions["analyzeduration"] = (10 * (long)1000 * 1000).ToString();

            options.FormatOptions["reconnect"] = "1";
            options.FormatOptions["reconnect_streamed"] = "1";
            options.FormatOptions["reconnect_delay_max"] = "7";

            options.FormatOptions["user_agent"] = DefaultUserAgentString;
            options.FormatOptions["referer"] = Referer;
            // options.FormatOptions["stimeout"] = (2 * 1000).ToString();
            // options.FormatOptions["buffer_size"] = (102400).ToString();

            // options.FormatOptions["rtsp_transport"] = "tcp";

            // var localFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "download");
            // if (!Directory.Exists(localFolderPath))
            // {
            //     Directory.CreateDirectory(localFolderPath);
            // }
            //
            // var localFilePath = Path.Combine(localFolderPath, Path.GetFileName(Path.GetTempFileName()) + ".m4s");
            //
            // _httpClient.DefaultRequestHeaders.Referrer = new Uri(Referer);
            // _httpClient.DefaultRequestHeaders.Add("User-Agent", DefaultUserAgentString);
            //
            //
            // using var responseMessage = await _httpClient.GetAsync(options.InputVideoPath);
            //
            // if (responseMessage.IsSuccessStatusCode)
            // {
            //     await using var fs = File.Create(localFilePath);
            //     await (await responseMessage.Content.ReadAsStreamAsync()).CopyToAsync(fs);
            // }

            // options.InputVideoPath = @"https://res.vcinema.com.cn/guanwangshipin2.mp4";
            // options.InputVideoPath = "https://ksldfp-c1.vcinema.cn/202110/ZqsqrCJW/nTaVIZpiKC.m3u8?auth_key=1696174884-0-0-e89e5040be1bd46793717663fda9a817&secret=384cbb4e2c80553d2b1341f4d9271647-117.147.4.100-65713901";
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