﻿namespace AvaFFmpegPlayer.Primitives;

public enum ClockSource
{
    /// <summary>
    /// The default choice.
    /// </summary>
    Audio,

    /// <summary>
    /// Synchronize to video clock.
    /// </summary>
    Video,

    /// <summary>
    /// Synchronize to external clock.
    /// </summary>
    External,
}
