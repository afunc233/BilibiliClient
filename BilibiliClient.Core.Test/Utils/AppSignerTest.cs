using BilibiliClient.Core.Utils;

namespace BilibiliClient.Core.Test.Utils;

public class AppSignerTest
{
    [Fact]
    public void Test1()
    {
        List<KeyValuePair<string, string>> paramList = new()
        {
            new KeyValuePair<string, string>("id", "114514"),
            new KeyValuePair<string, string>("str", "1919810"),
            new KeyValuePair<string, string>("test", "いいよ，こいよ")
        };

        var appSigner = new AppSigner(AppSigner.AppSignerKeySec.TestAppSignerKeySec);

        var result = appSigner.appSign(paramList);

        // https://github.com/SocialSisterYi/bilibili-API-collect/blob/master/docs/misc/sign/APP.md
        Assert.Equal("01479cf20504d865519ac50f33ba3a7d", result);
    }
}