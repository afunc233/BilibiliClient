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
    /// "uri": "bilibili://video/658284353?cid=1192479953&player_height=1080&player_preload=%7B%22expire_time%22%3A1689352367%2C%22cid%22%3A1192479953%2C%22quality%22%3A16%2C%22file_info%22%3A%7B%2216%22%3A%7B%22infos%22%3A%5B%7B%22filesize%22%3A12343202%2C%22timelength%22%3A421717%7D%5D%7D%2C%2264%22%3A%7B%22infos%22%3A%5B%7B%22filesize%22%3A42726650%2C%22timelength%22%3A422000%7D%5D%7D%7D%2C%22video_codecid%22%3A7%2C%22video_project%22%3Atrue%2C%22url%22%3A%22http%3A%2F%2Fupos-sz-mirror08h.bilivideo.com%2Fupgcxcode%2F53%2F99%2F1192479953%2F1192479953_nb3-1-16.mp4%3Fe%3Dig8euxZM2rNcNbRVhwdVhwdlhWdVhwdVhoNvNC8BqJIzNbfqXBvEuENvNC8aNEVEtEvE9IMvXBvE2ENvNCImNEVEIj0Y2J_aug859r1qXg8gNEVE5XREto8z5JZC2X2gkX5L5F1eTX1jkXlsTXHeux_f2o859IB_%5Cu0026uipk%3D5%5Cu0026nbs%3D1%5Cu0026deadline%3D1689355967%5Cu0026gen%3Dplayurlv2%5Cu0026os%3D08hbv%5Cu0026oi%3D1972576764%5Cu0026trid%3D1313d718f2644e148fa9e20ef376266fU%5Cu0026mid%3D0%5Cu0026platform%3Diphone%5Cu0026upsig%3D3e8d4e6d95581e95d450acc76b82f657%5Cu0026uparams%3De%2Cuipk%2Cnbs%2Cdeadline%2Cgen%2Cos%2Coi%2Ctrid%2Cmid%2Cplatform%5Cu0026bvc%3Dvod%5Cu0026nettype%3D0%5Cu0026orderid%3D0%2C3%5Cu0026buvid%3D%5Cu0026build%3D5520400%5Cu0026bw%3D29318%5Cu0026logo%3D80000000%22%2C%22accept_formats%22%3A%5B%7B%22quality%22%3A64%2C%22format%22%3A%22mp4720%22%2C%22description%22%3A%22%E9%AB%98%E6%B8%85%20720P%22%2C%22new_description%22%3A%22720P%20%E9%AB%98%E6%B8%85%22%2C%22display_desc%22%3A%22720P%22%2C%22need_login%22%3Atrue%7D%2C%7B%22quality%22%3A16%2C%22format%22%3A%22mp4%22%2C%22description%22%3A%22%E6%B5%81%E7%95%85%20360P%22%2C%22new_description%22%3A%22360P%20%E6%B5%81%E7%95%85%22%2C%22display_desc%22%3A%22360P%22%7D%5D%2C%22backup_url%22%3A%5B%22http%3A%2F%2Fupos-sz-mirror08c.bilivideo.com%2Fupgcxcode%2F53%2F99%2F1192479953%2F1192479953_nb3-1-16.mp4%3Fe%3Dig8euxZM2rNcNbRVhwdVhwdlhWdVhwdVhoNvNC8BqJIzNbfqXBvEuENvNC8aNEVEtEvE9IMvXBvE2ENvNCImNEVEIj0Y2J_aug859r1qXg8gNEVE5XREto8z5JZC2X2gkX5L5F1eTX1jkXlsTXHeux_f2o859IB_%5Cu0026uipk%3D5%5Cu0026nbs%3D1%5Cu0026deadline%3D1689355967%5Cu0026gen%3Dplayurlv2%5Cu0026os%3D08cbv%5Cu0026oi%3D1972576764%5Cu0026trid%3D1313d718f2644e148fa9e20ef376266fU%5Cu0026mid%3D0%5Cu0026platform%3Diphone%5Cu0026upsig%3Da4c757d7d467b01b2c3a2828ee589324%5Cu0026uparams%3De%2Cuipk%2Cnbs%2Cdeadline%2Cgen%2Cos%2Coi%2Ctrid%2Cmid%2Cplatform%5Cu0026bvc%3Dvod%5Cu0026nettype%3D0%5Cu0026orderid%3D1%2C3%5Cu0026buvid%3D%5Cu0026build%3D5520400%5Cu0026bw%3D29318%5Cu0026logo%3D40000000%22%2C%22http%3A%2F%2Fupos-sz-mirrorhwb.bilivideo.com%2Fupgcxcode%2F53%2F99%2F1192479953%2F1192479953_nb3-1-16.mp4%3Fe%3Dig8euxZM2rNcNbRVhwdVhwdlhWdVhwdVhoNvNC8BqJIzNbfqXBvEuENvNC8aNEVEtEvE9IMvXBvE2ENvNCImNEVEIj0Y2J_aug859r1qXg8gNEVE5XREto8z5JZC2X2gkX5L5F1eTX1jkXlsTXHeux_f2o859IB_%5Cu0026uipk%3D5%5Cu0026nbs%3D1%5Cu0026deadline%3D1689355967%5Cu0026gen%3Dplayurlv2%5Cu0026os%3Dhwbbv%5Cu0026oi%3D1972576764%5Cu0026trid%3D1313d718f2644e148fa9e20ef376266fU%5Cu0026mid%3D0%5Cu0026platform%3Diphone%5Cu0026upsig%3D006b1c11f469f4e9b440cca0faf3a85f%5Cu0026uparams%3De%2Cuipk%2Cnbs%2Cdeadline%2Cgen%2Cos%2Coi%2Ctrid%2Cmid%2Cplatform%5Cu0026bvc%3Dvod%5Cu0026nettype%3D0%5Cu0026orderid%3D2%2C3%5Cu0026buvid%3D%5Cu0026build%3D5520400%5Cu0026bw%3D29318%5Cu0026logo%3D40000000%22%5D%2C%22union_player%22%3A%7B%22biz_type%22%3A1%2C%22dimension%22%3A%7B%22width%22%3A1920%2C%22height%22%3A1080%7D%2C%22aid%22%3A658284353%7D%7D&player_rotate=0&player_width=1920",
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