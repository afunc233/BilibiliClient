using BilibiliClient.Core.Configs;

namespace BilibiliClient.Core.Messages;

public class SaveUserSecretMessage : CommunityToolkit.Mvvm.Messaging.Messages.ValueChangedMessage<UserSecretConfig>
{
    public SaveUserSecretMessage(UserSecretConfig value) : base(value)
    {
    }
}