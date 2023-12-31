﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Messages;
using BilibiliClient.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using Microsoft.Extensions.Hosting;
using IApplicationLifetime = Avalonia.Controls.ApplicationLifetimes.IApplicationLifetime;

namespace BilibiliClient.Services;

public class AppTrayIconHostService : IHostedService
{
    /// <summary>
    /// https://www.cnblogs.com/hejiale010426/p/17085178.html
    /// </summary>
    private TrayIcon? _notifyIcon;

    private readonly string _iconFolderPath = Path.Combine(AppContext.BaseDirectory, "Assets", "TrayIcons");
    private bool _isStop;
    private readonly IWindowManagerService _windowManagerService;
    private readonly IApplicationLifetime _applicationLifetime;
    private readonly IMessenger _messenger;

    public AppTrayIconHostService(IWindowManagerService windowManagerService,
        IMessenger messenger,
        IApplicationLifetime applicationLifetime)
    {
        _windowManagerService = windowManagerService;
        _messenger = messenger;
        _applicationLifetime = applicationLifetime;

        InitTrayIcon();

        _isStop = false;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var files = FileSort(Directory.GetFiles(_iconFolderPath));

        _ = Task.Factory.StartNew(async () =>
        {
            var cacheDic = new Dictionary<string, (Bitmap, WindowIcon)>();
            var currentFile = files.FirstOrDefault();
            while (!_isStop)
            {
                try
                {
                    if (_notifyIcon == null)
                    {
                        break;
                    }

                    if (!File.Exists(currentFile))
                    {
                        currentFile = files.FirstOrDefault(it => !string.IsNullOrWhiteSpace(it))!;
                    }

                    if (string.IsNullOrWhiteSpace(currentFile) || !File.Exists(currentFile))
                    {
                        break;
                    }

                    // 添加托盘图标,
                    var file = currentFile;
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        try
                        {
                            if (!cacheDic.TryGetValue(file, out var iconBitmap))
                            {
                                iconBitmap.Item1 = new Bitmap(file);

                                iconBitmap.Item2 = new WindowIcon(iconBitmap.Item1);
                                cacheDic.TryAdd(file, iconBitmap);
                            }

                            _notifyIcon.Icon = iconBitmap.Item2;
                            // _messenger.Send(new GlobalIconMessage(iconBitmap.Item1));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }, DispatcherPriority.SystemIdle, cancellationToken);

                    currentFile = string.Equals(currentFile, files.LastOrDefault())
                        ? files.FirstOrDefault()
                        : files.Skip(files.IndexOf(currentFile) + 1).FirstOrDefault();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    // 修改动态渲染速度
                    await Task.Delay(40, cancellationToken);
                }
            }
        }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _isStop = true;
        if (_notifyIcon != null)
        {
            _notifyIcon.IsVisible = false;
            _notifyIcon.Dispose();
            _notifyIcon = null;
        }

        await Task.Delay(TimeSpan.FromMilliseconds(500), cancellationToken);
    }


    private void InitTrayIcon()
    {
        _notifyIcon = new TrayIcon
        {
            IsVisible = true,
            ToolTipText = "bilibili ~"
        };

        var menu = _notifyIcon.Menu = new NativeMenu();

        var mainMenuItem = new NativeMenuItem("打开主界面");
        mainMenuItem.Click += (_, _) => { _windowManagerService.OpenInNewWindow(typeof(MainViewModel).FullName!); };
        menu.Add(mainMenuItem);
        var exitMenuItem = new NativeMenuItem("退出应用");
        exitMenuItem.Click += (_, _) =>
        {
            (_applicationLifetime as IClassicDesktopStyleApplicationLifetime)?.TryShutdown();
        };
        menu.Add(exitMenuItem);

        _notifyIcon.Clicked += (_, _) => { _windowManagerService.OpenInNewWindow(typeof(MainViewModel).FullName!); };
    }

    #region 文件名排序

    // 文件名排序
    private static string[] FileSort(string[] files)
    {
        var cacheDic = new Dictionary<string, int>();
        for (var i = 0; i < files.Length - 1; i++)
        {
            for (var j = 0; j < files.Length - 1 - i; j++)
            {
                if (!CustomSort(cacheDic, files[j], files[j + 1])) continue;
                // var temp = files[j];
                // files[j] = files[j + 1];
                // files[j + 1] = temp;
                // 下面的是新语法糖
                (files[j], files[j + 1]) = (files[j + 1], files[j]);
            }
        }

        return files;
    }

    private static bool CustomSort(Dictionary<string, int> cacheDic, string str1, string str2)
    {
        if (!cacheDic.TryGetValue(str1, out var result1))
        {
            result1 = Convert.ToInt32(System.Text.RegularExpressions.Regex.Replace(str1, @"[^0-9]+", ""));
            cacheDic.TryAdd(str1, result1);
        }

        if (!cacheDic.TryGetValue(str2, out var result2))
        {
            result2 = Convert.ToInt32(System.Text.RegularExpressions.Regex.Replace(str2, @"[^0-9]+", ""));
            cacheDic.TryAdd(str2, result2);
        }

        if (result1 > result2)
            return true;
        else
            return false;
    }

    #endregion
}