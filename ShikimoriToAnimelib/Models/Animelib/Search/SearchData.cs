using System.Text.Json.Serialization;

namespace ShikimoriToAnimelib.Models.Animelib.Search
{
    public class SearchData
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("rus_name")]
        public string? RusName { get; set; }

        [JsonPropertyName("eng_name")]
        public string? EngName { get; set; }

        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [JsonPropertyName("slug")]
        public string? Slug { get; set; }

        [JsonPropertyName("slug_url")]
        public string? SlugUrl { get; set; }

        [JsonPropertyName("cover")]
        public Cover? Cover { get; set; }

        [JsonPropertyName("ageRestriction")]
        public AgeRestriction? AgeRestriction { get; set; }

        [JsonPropertyName("site")]
        public int? Site { get; set; }

        [JsonPropertyName("type")]
        public Type? Type { get; set; }

        [JsonPropertyName("releaseDate")]
        public string? ReleaseDate { get; set; }

        [JsonPropertyName("rating")]
        public Rating? Rating { get; set; }

        [JsonPropertyName("status")]
        public Status? Status { get; set; }

        [JsonPropertyName("releaseDateString")]
        public string? ReleaseDateString { get; set; }

        [JsonPropertyName("shiki_href")]
        public string? ShikiUrl { get; set; }

        [JsonPropertyName("shiki_rate")]
        public double? ShikiRate { get; set; }
    }
}
