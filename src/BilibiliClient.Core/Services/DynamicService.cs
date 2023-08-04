using BilibiliClient.Core.Api.Contracts.Api;
using BilibiliClient.Core.Api.Models;
using BilibiliClient.Core.Contracts.Services;

namespace BilibiliClient.Core.Services;

internal class DynamicService : IDynamicService
{
    private bool _hasMore = true;
    private readonly IGrpcApi _grpcApi;

    private string? _offset;
    private string? _baseline;

    public DynamicService(IGrpcApi grpcApi)
    {
        _grpcApi = grpcApi;
        ResetParam();
    }

    public void ResetParam()
    {
        _baseline = _offset = string.Empty;
        _hasMore = true;
    }

    private DynamicDataType _lastDynamicDataType = DynamicDataType.Video;

    public async Task<List<object>> LoadNextPage(DynamicDataType dynamicDataType)
    {
        var dataList = dynamicDataType switch
        {
            DynamicDataType.All => await LoadNextVideoPage(dynamicDataType != _lastDynamicDataType),
            DynamicDataType.Video => await LoadNextAllPage(dynamicDataType != _lastDynamicDataType),
            _ => new List<object>(0)
        };
        _lastDynamicDataType = dynamicDataType;

        return dataList;
    }

    private async Task<List<object>> LoadNextAllPage(bool clearBefore)
    {
        if (clearBefore)
        {
            ResetParam();
        }

        var dynAllReply = await _grpcApi.GetDynamicAll(_offset, _baseline);
        var list = new List<object>();
        if (dynAllReply?.DynamicList?.List != null && dynAllReply.DynamicList.List.Any())
        {
            list.AddRange(dynAllReply.DynamicList.List);
            _offset = dynAllReply.DynamicList.HistoryOffset;
            _baseline = dynAllReply.DynamicList.UpdateBaseline;
            _hasMore = dynAllReply.DynamicList.HasMore;
        }
        else
        {
            _hasMore = false;
        }

        return list;
    }

    private async Task<List<object>> LoadNextVideoPage(bool clearBefore)
    {
        if (clearBefore)
        {
            ResetParam();
        }

        var dynamicVideo = await _grpcApi.GetDynamicVideo(_offset, _baseline);
        var list = new List<object>();
        if (dynamicVideo?.DynamicList?.List != null && dynamicVideo.DynamicList.List.Any())
        {
            list.AddRange(dynamicVideo.DynamicList.List);
            
            _offset = dynamicVideo.DynamicList.HistoryOffset;
            _baseline = dynamicVideo.DynamicList.UpdateBaseline;
            _hasMore = dynamicVideo.DynamicList.HasMore;
        }
        else
        {
            _hasMore = false;
        }

        return list;
    }

    public bool HasMore => _hasMore;
}