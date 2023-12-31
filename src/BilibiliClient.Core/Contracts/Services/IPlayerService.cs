﻿using Bilibili.App.Card.V1;
using BilibiliClient.Core.Models.Https.Api;
using BilibiliClient.Core.Models.Https.App;

namespace BilibiliClient.Core.Contracts.Services;

public interface IPlayerService
{
    Task<VideoPlayUrlResult?> GetPlayUrl(RecommendCardItem recommendCardItem);
    Task<VideoPlayUrlResult?> GetPlayUrl(Card card);
}