using System;
using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using BilibiliClient.ViewModels;
using LibVLCSharp;
using LibVLCSharp.Shared;

namespace BilibiliClient.Views;

public partial class VlcPlayerWindow : Window
{
    private LibVLC? _libVlc;

    private MediaPlayer? _mediaPlayer;

    public VlcPlayerWindow()
    {
        InitializeComponent();

        this.Loaded += OnLoaded;
        _libVlc = new LibVLC();
        _mediaPlayer = VideoView.MediaPlayer = new MediaPlayer(_libVlc);

        _mediaPlayer.EncounteredError += MediaPlayerOnEncounteredError;

        this.DetachedFromVisualTree += OnDetachedFromVisualTree;
    }

    private void OnDetachedFromVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        _mediaPlayer?.Stop();
        _mediaPlayer?.Media?.Dispose();
        _libVlc?.Dispose();
        _libVlc = null;
    }

    private void MediaPlayerOnEncounteredError(object? sender, EventArgs e)
    {
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (this.DataContext is not PlayerViewModel playerViewModel) return;
        playerViewModel.PropertyChanged -= PlayerViewModelOnPropertyChanged;
        playerViewModel.PropertyChanged += PlayerViewModelOnPropertyChanged;
    }

    private void PlayerViewModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(PlayerViewModel.VideoPlayUrl):
                DoPlay();
                return;
        }
    }

    private void DoPlay()
    {
        if (_libVlc != null && this.DataContext is PlayerViewModel { VideoPlayUrl: not null } playerViewModel)
        {
            var videoUrl = playerViewModel.VideoPlayUrl?.Dash?.Video?.FirstOrDefault()?.BaseUrl;
            var audioUrl = playerViewModel.VideoPlayUrl?.Dash?.Audio?.FirstOrDefault()?.BaseUrl;
            var mediaPlayer = VideoView.MediaPlayer;
            if (mediaPlayer != null && !string.IsNullOrWhiteSpace(videoUrl))
            {
                Media media;
                if (!string.IsNullOrWhiteSpace(audioUrl))
                    media = new Media(_libVlc, videoUrl, FromType.FromLocation, $":input-slave={audioUrl}");
                else
                    media = new Media(_libVlc, videoUrl, FromType.FromLocation);

                mediaPlayer.Media = media;
                mediaPlayer.Play();
            }
            else
            {
                
            }
        }
    }
}