﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using Avalonia.Styling;
using BilibiliClient.Models;
using FluentAvalonia.Styling;

namespace BilibiliClient.ViewModels;

public partial class SettingPageViewModel : AbsPageViewModel
{
    public override NavBarType NavBarType => NavBarType.Setting;

    public ThemeVariant[] AppThemes { get; } =
        new[] { ThemeVariant.Light, ThemeVariant.Dark /*, FluentAvaloniaTheme.HighContrastTheme*/ };


    public FlowDirection[] AppFlowDirections { get; } =
        new[] { FlowDirection.LeftToRight, FlowDirection.RightToLeft };

    public ThemeVariant CurrentAppTheme
    {
        get => _currentAppTheme ?? AppThemes.First();
        set
        {
            if (Application.Current == null)
            {
                return;
            }

            if (SetProperty(ref _currentAppTheme, value)
                && Application.Current.ActualThemeVariant != value)
            {
                Application.Current.RequestedThemeVariant = value;
            }
        }
    }

    private ThemeVariant? _currentAppTheme;


    public FlowDirection CurrentFlowDirection
    {
        get => _currentFlowDirection;
        set
        {
            if (SetProperty(ref _currentFlowDirection, value))
            {
                var lifetime = Application.Current?.ApplicationLifetime;
                if (lifetime is IClassicDesktopStyleApplicationLifetime cdl)
                {
                    var win = cdl.MainWindow;
                    if (win != null)
                    {
                        if (win.FlowDirection == value)
                            return;
                        win.FlowDirection = value;
                    }
                }
                else if (lifetime is ISingleViewApplicationLifetime single)
                {
                    var mainWindow = TopLevel.GetTopLevel(single.MainView);
                    if (mainWindow == null || mainWindow.FlowDirection == value)
                        return;
                    mainWindow.FlowDirection = value;
                }
            }
        }
    }

    private FlowDirection _currentFlowDirection;


    public bool UseCustomAccent
    {
        get => _useCustomAccentColor;
        set
        {
            if (SetProperty(ref _useCustomAccentColor, value))
            {
                var faTheme = Application.Current?.Styles[0] as FluentAvaloniaTheme;
                if (value && faTheme != null)
                {
                    if (faTheme.TryGetResource("SystemAccentColor", null, out var curColor))
                    {
                        _customAccentColor = (Color)curColor;
                        _listBoxColor = _customAccentColor;

                        OnPropertyChanged(nameof(CustomAccentColor));
                        OnPropertyChanged(nameof(ListBoxColor));
                    }
                    else
                    {
                        // This should never happen, if it does, something bad has happened
                        throw new Exception("Unable to retreive SystemAccentColor");
                    }
                }
                else
                {
                    // Restore system color
                    _customAccentColor = default;
                    _listBoxColor = default;
                    OnPropertyChanged(nameof(CustomAccentColor));
                    OnPropertyChanged(nameof(ListBoxColor));
                    UpdateAppAccentColor(null);
                }
            }
        }
    }

    private bool _useCustomAccentColor;

    // This is bound to the ListBox of predefined colors. It must be nullable or CompiledBindings will get angry
    // if we set a color here that isn't in the predef colors as SelectingItemsControl will try to bind back
    // null as the SelectedItem 
    public Color? ListBoxColor
    {
        get => _listBoxColor;
        set
        {
            SetProperty(ref _listBoxColor, value);

            if (value != null)
            {
                _customAccentColor = value.Value;
                OnPropertyChanged(nameof(CustomAccentColor));

                UpdateAppAccentColor(value.Value);
            }
        }
    }

    private Color? _listBoxColor;


    // This is the custom accent color as chosen by the ColorPicker and is not one of the predefined colors
    public Color CustomAccentColor
    {
        get => _customAccentColor;
        set
        {
            if (SetProperty(ref _customAccentColor, value))
            {
                _listBoxColor = value;
                OnPropertyChanged(nameof(ListBoxColor));
                UpdateAppAccentColor(value);
            }
        }
    }

    private Color _customAccentColor = Colors.SlateBlue;

    public List<Color>? PredefinedColors { get; private set; }

    public string? CurrentVersion =>
        this.GetType().Assembly.GetName().Version?.ToString();

    public string? CurrentAvaloniaVersion =>
        typeof(Application).Assembly.GetName().Version?.ToString();

    private void GetPredefColors()
    {
        PredefinedColors = new List<Color>
        {
            Color.FromRgb(255, 185, 0),
            Color.FromRgb(255, 140, 0),
            Color.FromRgb(247, 99, 12),
            Color.FromRgb(202, 80, 16),
            Color.FromRgb(218, 59, 1),
            Color.FromRgb(239, 105, 80),
            Color.FromRgb(209, 52, 56),
            Color.FromRgb(255, 67, 67),
            Color.FromRgb(231, 72, 86),
            Color.FromRgb(232, 17, 35),
            Color.FromRgb(234, 0, 94),
            Color.FromRgb(195, 0, 82),
            Color.FromRgb(227, 0, 140),
            Color.FromRgb(191, 0, 119),
            Color.FromRgb(194, 57, 179),
            Color.FromRgb(154, 0, 137),
            Color.FromRgb(0, 120, 212),
            Color.FromRgb(0, 99, 177),
            Color.FromRgb(142, 140, 216),
            Color.FromRgb(107, 105, 214),
            Color.FromRgb(135, 100, 184),
            Color.FromRgb(116, 77, 169),
            Color.FromRgb(177, 70, 194),
            Color.FromRgb(136, 23, 152),
            Color.FromRgb(0, 153, 188),
            Color.FromRgb(45, 125, 154),
            Color.FromRgb(0, 183, 195),
            Color.FromRgb(3, 131, 135),
            Color.FromRgb(0, 178, 148),
            Color.FromRgb(1, 133, 116),
            Color.FromRgb(0, 204, 106),
            Color.FromRgb(16, 137, 62),
            Color.FromRgb(122, 117, 116),
            Color.FromRgb(93, 90, 88),
            Color.FromRgb(104, 118, 138),
            Color.FromRgb(81, 92, 107),
            Color.FromRgb(86, 124, 115),
            Color.FromRgb(72, 104, 96),
            Color.FromRgb(73, 130, 5),
            Color.FromRgb(16, 124, 16),
            Color.FromRgb(118, 118, 118),
            Color.FromRgb(76, 74, 72),
            Color.FromRgb(105, 121, 126),
            Color.FromRgb(74, 84, 89),
            Color.FromRgb(100, 124, 100),
            Color.FromRgb(82, 94, 84),
            Color.FromRgb(132, 117, 69),
            Color.FromRgb(126, 115, 95)
        };
    }

    private void UpdateAppAccentColor(Color? color)
    {
        var appTheme = (Application.Current?.Styles[0] as FluentAvaloniaTheme);
        if (appTheme != null)
        {
            appTheme.CustomAccentColor = color;
        }
    }

    public override async Task OnNavigatedTo(object? parameter = null)
    {
        await base.OnNavigatedTo(parameter);
        CurrentAppTheme = Application.Current?.ActualThemeVariant ?? AppThemes.First();

        GetPredefColors();
    }
}