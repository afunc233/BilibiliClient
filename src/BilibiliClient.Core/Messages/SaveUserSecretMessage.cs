using BilibiliClient.Core.Api.Models;

namespace BilibiliClient.Core.Messages;

public class SaveUserSecretMessage(UserSecretConfig? value = null) : CommunityToolkit.Mvvm.Messaging.Messages.ValueChangedMessage<UserSecretConfig?>(value)
{
}