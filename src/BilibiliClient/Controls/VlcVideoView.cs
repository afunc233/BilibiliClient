using System;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Platform;
using LibVLCSharp;
using LibVLCSharp.Shared;

namespace BilibiliClient.Controls;

/// <summary>Avalonia VideoView for Windows, Linux and Mac.</summary>
public class VlcVideoView : NativeControlHost
{
    private IPlatformHandle? _platformHandle;
    private MediaPlayer? _mediaPlayer;

    /// <summary>MediaPlayer Data Bound property</summary>
    /// <summary>
    /// Defines the <see cref="P:LibVLCSharp.Avalonia.VideoView.MediaPlayer" /> property.
    /// </summary>
    public static readonly DirectProperty<VlcVideoView, MediaPlayer?> MediaPlayerProperty =
        AvaloniaProperty.RegisterDirect<VlcVideoView, MediaPlayer>(nameof(MediaPlayer),
            (Func<VlcVideoView, MediaPlayer>)(o => o.MediaPlayer),
            (Action<VlcVideoView, MediaPlayer>)((o, v) => o.MediaPlayer = v), (MediaPlayer)null, BindingMode.TwoWay,
            false);

    /// <summary>Gets or sets the MediaPlayer that will be displayed.</summary>
    public MediaPlayer? MediaPlayer
    {
        get => this._mediaPlayer;
        set
        {
            if (this._mediaPlayer == value)
                return;
            this.Detach();
            this._mediaPlayer = value;
            this.Attach();
        }
    }

    private void Attach()
    {
        if (this._mediaPlayer == null || this._platformHandle == null || !this.IsInitialized)
            return;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            this._mediaPlayer.Hwnd = this._platformHandle.Handle;
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            this._mediaPlayer.XWindow = (uint)(int)this._platformHandle.Handle;
        }
        else
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return;
            this._mediaPlayer.NsObject = this._platformHandle.Handle;
        }
    }

    private void Detach()
    {
        if (this._mediaPlayer == null)
            return;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            this._mediaPlayer.Hwnd = IntPtr.Zero;
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            this._mediaPlayer.XWindow = 0U;
        }
        else
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return;
            this._mediaPlayer.NsObject = IntPtr.Zero;
        }
    }

    /// <inheritdoc />
    protected override IPlatformHandle CreateNativeControlCore(IPlatformHandle parent)
    {
        this._platformHandle = base.CreateNativeControlCore(parent);
        if (this._mediaPlayer == null)
            return this._platformHandle;
        this.Attach();
        return this._platformHandle;
    }

    /// <inheritdoc />
    protected override void DestroyNativeControlCore(IPlatformHandle control)
    {
        this.Detach();
        base.DestroyNativeControlCore(control);
        if (this._platformHandle == null)
            return;
        this._platformHandle = (IPlatformHandle)null;
    }
}