using Bilibili.App.Interfaces.V1;
using BilibiliClient.Core.Contracts.Api;
using BilibiliClient.Core.Contracts.Services;

namespace BilibiliClient.Core.Services;

public class HistoryService : IHistoryService
{
    private bool _hasMore = true;
    private readonly Cursor _historyCursor;
    private readonly IGrpcApi _grpcApi;

    public HistoryService(IGrpcApi grpcApi)
    {
        _grpcApi = grpcApi;
        _historyCursor = new Cursor()
        {
            Max = 0
        };
    }

    public async Task<List<CursorItem>?> LoadNextPage()
    {
        var cursorItems = new List<CursorItem>();

        var cursorV2Reply = await _grpcApi.GetMyHistory(_historyCursor);

        if (cursorV2Reply != null && cursorV2Reply.Items.Any())
        {
            _historyCursor.Max = cursorV2Reply.Cursor.Max;
            _historyCursor.MaxTp = cursorV2Reply.Cursor.MaxTp;

            cursorItems.AddRange(cursorV2Reply.Items);
        }

        _hasMore = cursorV2Reply?.HasMore ?? false;
        return cursorItems;
    }

    public bool HasMore => _hasMore;

    public void ResetCursor()
    {
        _historyCursor.Max = 0;
        _historyCursor.MaxTp = 0;
    }
}