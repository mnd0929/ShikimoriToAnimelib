using System.Text.Json.Serialization;

namespace ShikimoriToAnimelib.Models.Shikimori
{
    public class ShikimoriBookmark
    {
        [JsonPropertyName("target_title")]
        public string? TargetTitle { get; set; }

        [JsonPropertyName("target_title_ru")]
        public string? TargetTitleRu { get; set; }

        [JsonPropertyName("target_id")]
        public int TargetId { get; set; }

        [JsonPropertyName("target_type")]
        public string? TargetType { get; set; }

        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("rewatches")]
        public int Rewatches { get; set; }

        [JsonPropertyName("episodes")]
        public int Episodes { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}
