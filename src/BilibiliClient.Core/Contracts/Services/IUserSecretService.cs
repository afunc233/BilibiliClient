using BilibiliClient.Core.Api.Configs;
using BilibiliClient.Core.Api.Models;

namespace BilibiliClient.Core.Contracts.Services;

public interface IUserSecretService
{
    /// <summary>
    /// 加载用户数据
    /// </summary>
    /// <returns></returns>
    Task LoadUserSecret();

    /// <summary>
    /// 保存用户数据
    /// </summary>
    /// <returns></returns>
    Task SaveUserSecret(UserSecretConfig? userSecretConfig = null);
}