using BilibiliClient.Core.Utils;

namespace BilibiliClient.Core.Test.Utils;

public class AppSignerTest
{
    [Fact]
    public void Test1()
    {
        List<(string, string)> paramList = new()
        {
            ("id", "114514"),
            ("str", "1919810"),
            ("test", "いいよ，こいよ")
        };

        var appSigner = new AppSigner(AppSigner.AppSignerKeySec.TestAppSignerKeySec);

        var result = appSigner.appSign(paramList);

        // https://github.com/SocialSisterYi/bilibili-API-collect/blob/master/docs/misc/sign/APP.md
        Assert.Equal("01479cf20504d865519ac50f33ba3a7d", result);
    }
}