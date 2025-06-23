using System.Text.Json.Serialization;

namespace ShikimoriToAnimelib.Models.Animelib.Search
{
    public class AgeRestriction
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("label")]
        public string? Label { get; set; }
    }
}
