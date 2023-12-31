﻿using BilibiliClient.Core.Models.Https.App;

namespace BilibiliClient.Core.Api.Contracts.Api;

internal interface IAppApi
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="from">从 什么位置开始 好像用处不是很大 ？</param>
    /// <param name="limit">限制多少条数据 好像用处不是很大 ？</param>
    /// <returns></returns>
    ValueTask<object?> SearchSquare(int from = 0, int limit = 50);

    // x/v2/feed/index
    ValueTask<HomeRecommendInfo?> GetRecommend(RecommendModel recommendModel);


    ValueTask<object?> RegionIndex();


    /// <summary>
    /// 
    /// </summary>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    ValueTask<object?> GetMyInfo(string accessToken);
}