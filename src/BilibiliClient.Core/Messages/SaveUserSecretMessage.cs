using BilibiliClient.Core.Api.Models;

namespace BilibiliClient.Core.Messages;

public class SaveUserSecretMessage : CommunityToolkit.Mvvm.Messaging.Messages.ValueChangedMessage<UserSecretConfig?>
{
    public SaveUserSecretMessage(UserSecretConfig? value = null) : base(value)
    {
    }
}