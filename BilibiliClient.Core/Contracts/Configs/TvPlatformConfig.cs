﻿using BilibiliClient.Core.Contracts.Models;

namespace BilibiliClient.Core.Contracts.Configs;

public class TvPlatformConfig : IPlatformConfig
{
    public ApiPlatform ApiPlatform => ApiPlatform.Tv;
    public string Platform => "tv";
    public string AppKey => "4409e2ce8ffd12b8";
    public string AppSecret => "59b43e04ad6965f34319062b478f83dd";
}