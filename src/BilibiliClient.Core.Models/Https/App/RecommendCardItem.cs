using System.Text.Json.Serialization;

namespace BilibiliClient.Core.Models.Https.App;

public class RecommendCardItemPlayerArgs
{
    /// <summary>
    ///  "aid": 658284353,
    /// </summary>
    [JsonPropertyName("aid")]
    public int aid { get; set; }

    /// <summary>
    ///      "cid": 1192479953,
    /// </summary>

    [JsonPropertyName("cid")]
    public int cid { get; set; }

    /// <summary>
    ///      "type": "av",
    /// </summary>

    [JsonPropertyName("type")]
    public string? type { get; set; }

    /// <summary>
    ///     "duration": 422
    /// </summary>

    [JsonPropertyName("duration")]
    public int duration { get; set; }
}

public class RecommendCardItemArgs
{
    /// <summary>
    /// "up_id": 12861708,
    /// </summary>
    [JsonPropertyName("up_id")]
    public long up_id { get; set; }

    /// <summary>
    ///     "up_name": "街森",
    /// </summary>

    [JsonPropertyName("up_name")]
    public string? up_name { get; set; }

    /// <summary>
    ///      "rid": 201,
    /// </summary>

    [JsonPropertyName("rid")]
    public int rid { get; set; }

    /// <summary>
    ///      "rname": "科学科普",
    /// </summary>

    [JsonPropertyName("rname")]
    public string? rname { get; set; }

    /// <summary>
    ///      "aid": 658284353
    /// </summary>

    [JsonPropertyName("aid")]
    public int aid { get; set; }
}

public class RecommendCardItemThreePointActionDetail
{
    /// <summary>
    ///  "id": 4,
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    ///              "name": "UP主:街森",
    /// </summary>
    [JsonPropertyName("name")]
    public string? name { get; set; }

    /// <summary>
    ///              "toast": "将减少相似内容推荐"
    /// </summary>
    [JsonPropertyName("toast")]
    public string? toast { get; set; }
}

public class RecommendCardItemThreePoint
{
    /// <summary>
    ///     "dislike_reasons": [],
    /// </summary>
    [JsonPropertyName("dislike_reasons")]
    public List<RecommendCardItemThreePointActionDetail>? DislikeReasonList { get; set; }


    /// <summary>
    ///     "feedbacks": [],
    /// </summary>
    [JsonPropertyName("feedbacks")]
    public List<RecommendCardItemThreePointActionDetail>? FeedbackList { get; set; }
    //     "watch_later": 1
}

/// <summary>
/// "three_point_v2": [
///     {
///         "title": "添加至稍后再看",
///         "type": "watch_later",
///         "icon": "https://i0.hdslb.com/bfs/activity-plat/static/ce06d65bc0a8d8aa2a463747ce2a4752/NyPAqcn0QF.png"
///     },
///     {
///         "title": "反馈",
///         "subtitle": "(选择后将优化首页此类内容)",
///         "reasons": [
///             {
///                 "id": 1,
///                 "name": "恐怖血腥",
///                 "toast": "将优化首页此类内容"
///             },
///             {
///                 "id": 2,
///                 "name": "色情低俗",
///                 "toast": "将优化首页此类内容"
///             },
///             {
///                 "id": 3,
///                 "name": "封面恶心",
///                 "toast": "将优化首页此类内容"
///             },
///             {
///                 "id": 4,
///                 "name": "标题党/封面党",
///                 "toast": "将优化首页此类内容"
///             }
///         ],
///         "type": "feedback"
///     },
///     {
///         "title": "不感兴趣",
///         "subtitle": "(选择后将减少相似内容推荐)",
///         "reasons": [
///             {
///                 "id": 4,
///                 "name": "UP主:街森",
///                 "toast": "将减少相似内容推荐"
///             },
///             {
///                 "id": 2,
///                 "name": "分区:科学科普",
///                 "toast": "将减少相似内容推荐"
///             },
///             {
///                 "id": 1,
///                 "name": "不感兴趣",
///                 "toast": "将减少相似内容推荐"
///             }
///         ],
///         "type": "dislike"
///     }
/// ],
/// </summary>
public class RecommendCardItemThreePointV2
{
    /// <summary>
    ///  "title": "添加至稍后再看",
    /// </summary>
    [JsonPropertyName("title")]
    public string? title { get; set; }

    /// <summary>
    ///          "type": "watch_later",
    /// </summary>
    [JsonPropertyName("type")]
    public string? type { get; set; }

    /// <summary>
    ///          "icon": "https://i0.hdslb.com/bfs/activity-plat/static/ce06d65bc0a8d8aa2a463747ce2a4752/NyPAqcn0QF.png"
    /// </summary>
    [JsonPropertyName("icon")]
    public string? icon { get; set; }

    /// <summary>
    ///         "subtitle": "(选择后将优化首页此类内容)",
    /// </summary>
    [JsonPropertyName("subtitle")]
    public string? subtitle { get; set; }

    /// <summary>
    /// "reasons": [],
    /// </summary>
    [JsonPropertyName("reasons")]
    public List<RecommendCardItemThreePointActionDetail>? reasonList { get; set; }
}

public class RecommendCardItemAvatar
{
    /// <summary>
    /// "cover": "https://i0.hdslb.com/bfs/face/9658f6843edb35ad133988a3d2ed0fb2d1eb6350.jpg",
    /// </summary>
    [JsonPropertyName("cover")]
    public string? cover { get; set; }

    /// <summary>
    ///      "uri": "bilibili://space/12861708",
    /// </summary>

    [JsonPropertyName("uri")]
    public string? uri { get; set; }

    /// <summary>
    ///      "event": "up_click",
    /// </summary>
    [JsonPropertyName("event")]
    public string? Event { get; set; }

    /// <summary>
    ///      "event_v2": "up-click",
    /// </summary>

    [JsonPropertyName("event_v2")]
    public string? Event_v2 { get; set; }

    /// <summary>
    ///      "up_id": 12861708
    /// </summary>

    [JsonPropertyName("up_id")]
    public long up_id { get; set; }
}

public class RecommendCardItemButton
{
    /// <summary>
    ///  "text": "+ 关注",
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    /// <summary>
    ///  "param": "39839686",
    /// </summary>
    [JsonPropertyName("param")]
    public string? Param { get; set; }

    /// <summary>
    ///  "event": "up_follow",
    /// </summary>
    [JsonPropertyName("event")]
    public string? Event { get; set; }

    /// <summary>
    ///  "type": 2,
    /// </summary>
    [JsonPropertyName("type")]
    public int type { get; set; }

    /// <summary>
    ///  "event_v2": "up-follow"
    /// </summary>
    [JsonPropertyName("event_v2")]
    public string? event_v2 { get; set; }
}

/// <summary>
///      "mask": {
///     "avatar": {
///         "cover": "https://i0.hdslb.com/bfs/face/9658f6843edb35ad133988a3d2ed0fb2d1eb6350.jpg",
///         "text": "街森",
///         "uri": "bilibili://space/12861708",
///         "event": "up_click",
///         "event_v2": "up-click",
///         "up_id": 12861708
///     },
///     "button": {
///         "text": "+ 关注",
///         "param": "12861708",
///         "event": "up_follow",
///         "type": 2,
///         "event_v2": "up-follow"
///     }
/// },
/// </summary>
public class RecommendCardItemMask
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("avatar")]
    public RecommendCardItemAvatar? Avatar { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("button")]
    public RecommendCardItemButton? Button { get; set; }
}

public class RecommendCardItem
{
    /// <summary>
    ///     "card_type": "large_cover_v1",
    /// </summary>
    [JsonPropertyName("card_type")]
    public string? card_type { get; set; }

    /// <summary>
    /// "card_goto": "av",
    /// </summary>

    [JsonPropertyName("card_goto")]
    public string? card_goto { get; set; }

    /// <summary>
    ///  "goto": "av",
    /// </summary>

    [JsonPropertyName("goto")]
    public string? Goto { get; set; }

    /// <summary>
    ///  "param": "658284353",
    /// </summary>

    [JsonPropertyName("param")]
    public string? Param { get; set; }

    /// <summary>
    ///  "bvid": "BV1Bh4y1Z7kb",
    /// </summary>

    [JsonPropertyName("bvid")]
    public string? Bvid { get; set; }

    /// <summary>
    ///  "cover": "http://i1.hdslb.com/bfs/archive/a3bf4448a4dfb8ad1289e651f0089c69b97cd9b4.jpg",
    /// </summary>

    [JsonPropertyName("cover")]
    public string? Cover { get; set; }

    /// <summary>
    ///  "title": "我亲身体验了脑机接口技术后，表示大受震撼！",
    /// </summary>

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// "uri": "",
    /// </summary>
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }


    /// <summary>
    /// "three_point": {
    /// },
    /// </summary>
    [JsonPropertyName("three_point")]
    public RecommendCardItemThreePoint? ThreePoint { get; set; }


    /// <summary>
    ///     "args": {},
    /// </summary>
    [JsonPropertyName("args")]
    public RecommendCardItemArgs? Args { get; set; }


    /// <summary>
    ///     "player_args": {},
    /// </summary>
    [JsonPropertyName("player_args")]
    public RecommendCardItemPlayerArgs? PlayerArgs { get; set; }

    /// <summary>
    ///  "idx": 1689348783,
    /// </summary>

    [JsonPropertyName("idx")]
    public int Idx { get; set; }

    /// <summary>
    ///     "mask": {},
    /// </summary>

    [JsonPropertyName("mask")]
    public RecommendCardItemMask? Mask { get; set; }


    /// <summary>
    /// "three_point_v2": [],
    /// </summary>
    [JsonPropertyName("three_point_v2")]
    public List<RecommendCardItemThreePointV2>? RecommendCardItemThreePointV2List { get; set; }

    /// <summary>
    ///     "avatar": {},
    /// </summary>
    [JsonPropertyName("avatar")]
    public RecommendCardItemAvatar? Avatar { get; set; }

    /// <summary>
    ///  "cover_left_text_1": "7:02",
    /// </summary>
    [JsonPropertyName("cover_left_text_1")]
    public string? cover_left_text_1 { get; set; }

    /// <summary>
    ///  "cover_left_text_2": "35.1万观看",
    /// </summary>

    [JsonPropertyName("cover_left_text_2")]
    public string? cover_left_text_2 { get; set; }

    /// <summary>
    ///  "cover_left_text_3": "793弹幕",
    /// </summary>

    [JsonPropertyName("cover_left_text_3")]
    public string? cover_left_text_3 { get; set; }

    /// <summary>
    ///  "desc": "街森 · 3天前",
    /// </summary>

    [JsonPropertyName("desc")]
    public string? desc { get; set; }

    /// <summary>
    ///  "official_icon": 16,
    /// </summary>

    [JsonPropertyName("official_icon")]
    public int official_icon { get; set; }

    /// <summary>
    ///  "can_play": 1
    /// </summary>

    [JsonPropertyName("can_play")]
    public int can_play { get; set; }
}

public class RecommendConfig
{
    /// <summary>
    /// "column": 1,
    /// </summary>
    [JsonPropertyName("column")]
    public int Column { get; set; }

    /// <summary>
    /// "autoplay_card": 2,
    /// </summary>
    [JsonPropertyName("autoplay_card")]
    public int AutoplayCard { get; set; }

    /// <summary>
    /// "feed_clean_abtest": 0,
    /// </summary>
    [JsonPropertyName("feed_clean_abtest")]
    public int FeedCleanAbtest { get; set; }

    /// <summary>
    /// "home_transfer_test": 0,
    /// </summary>
    [JsonPropertyName("home_transfer_test")]
    public int HomeTransferTest { get; set; }

    /// <summary>
    /// "auto_refresh_time": 1200,
    /// </summary>

    [JsonPropertyName("auto_refresh_time")]
    public int auto_refresh_time { get; set; }

    /// <summary>
    ///  "show_inline_danmaku": 1,
    /// </summary>

    [JsonPropertyName("show_inline_danmaku")]
    public int show_inline_danmaku { get; set; }

    /// <summary>
    ///  "toast": {},
    /// </summary>

    [JsonPropertyName("toast")]
    public object? toast { get; set; }

    /// <summary>
    ///  "is_back_to_homepage": true,
    /// </summary>

    [JsonPropertyName("is_back_to_homepage")]
    public bool is_back_to_homepage { get; set; }

    /// <summary>
    /// "enable_rcmd_guide": true,
    /// </summary>
    [JsonPropertyName("enable_rcmd_guide")]
    public bool enable_rcmd_guide { get; set; }

    /// <summary>
    ///  "inline_sound": 2,
    /// </summary>

    [JsonPropertyName("inline_sound")]
    public int inline_sound { get; set; }

    /// <summary>
    ///  "auto_refresh_time_by_appear": 1200,
    /// </summary>

    [JsonPropertyName("auto_refresh_time_by_appear")]
    public int auto_refresh_time_by_appear { get; set; }

    /// <summary>
    ///  "auto_refresh_time_by_active": 1200,
    /// </summary>

    [JsonPropertyName("auto_refresh_time_by_active")]
    public int auto_refresh_time_by_active { get; set; }

    /// <summary>
    ///  "visible_area": 80,
    /// </summary>

    [JsonPropertyName("visible_area")]
    public int visible_area { get; set; }

    /// <summary>
    ///  "card_density_exp": 1
    /// </summary>

    [JsonPropertyName("card_density_exp")]
    public int card_density_exp { get; set; }
}

public class HomeRecommendInfo
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("items")]
    public List<RecommendCardItem>? Items { get; set; }


    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("config")]
    public RecommendConfig? Config { get; set; }
}