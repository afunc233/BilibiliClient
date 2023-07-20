using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BilibiliClient.Core.Messages;

public class LoginStateMessage : ValueChangedMessage<LoginStateEnum>
{
    public string? ErrorMessage { get; }

    public LoginStateMessage(LoginStateEnum value, string? errorMessage = null) : base(value)
    {
        this.ErrorMessage = errorMessage;
    }
}

public enum LoginStateEnum
{
    QRCodeExpire,
    StopQRCodePoll,
    LoginSuccess,
    Fail,
}