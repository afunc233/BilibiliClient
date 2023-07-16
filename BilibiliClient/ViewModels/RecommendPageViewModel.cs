using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BilibiliClient.Core.Contracts.Api;
using BilibiliClient.Core.Models.Https.App;
using BilibiliClient.Models;

namespace BilibiliClient.ViewModels;

public class RecommendPageViewModel : AbsPageViewModel
{
    public override NavBarType NavBarType => NavBarType.Recommend;
    public ObservableCollection<RecommendCardItem> RecommendDataList { get; } = new();

    private string _idx = "0";
    private readonly IAppApi _appApi;

    public RecommendPageViewModel(IAppApi appApi)
    {
        _appApi = appApi;
    }

    public override async Task OnNavigatedTo(object? parameter = null)
    {
        await base.OnNavigatedTo(parameter);
        IsLoading = true;
        var homeRecommend = await _appApi.GetRecommend(new RecommendModel()
        {
            Idx = _idx,
        });
        if (homeRecommend is { Items: not null })
        {
            homeRecommend.Items.ForEach(RecommendDataList.Add);
            _idx = homeRecommend.Items.LastOrDefault()?.Idx.ToString() ?? "0";
        }


        var aa = await _appApi.RegionIndex();
        if (aa != null)
        {
        }

        IsLoading = false;
    }
}