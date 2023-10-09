using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BilibiliClient.Core.Messages;

public class LoginStateMessage(LoginStateEnum value, string? errorMessage = null) : ValueChangedMessage<LoginStateEnum>(value)
{
    public string? ErrorMessage { get; } = errorMessage;
}

public enum LoginStateEnum
{
    QRCodeExpire,
    StopQRCodePoll,
    LoginSuccess,
    Fail,
}