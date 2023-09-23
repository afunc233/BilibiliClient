namespace BilibiliClient.Core.Contracts.Services;

public interface IDialogService
{
    /// <summary>
    /// 打开一个 Dialog
    /// </summary>
    /// <param name="parameter"></param>
    /// <typeparam name="T">dialog ViewModel </typeparam>
    /// <typeparam name="TT">返回值类型</typeparam>
    /// <returns></returns>
    Task<TT?> ShowDialog<T, TT>(object? parameter = null) where T : class, IDialog<TT>;

    /// <summary>
    /// 关闭一个 Dialog
    /// </summary>
    /// <typeparam name="T">dialog ViewModel </typeparam>
    void CloseDialog<T>() where T : class, IDialog;
}