using System.Text.Json.Serialization;

namespace ShikimoriToAnimelib.Models.Animelib.Search
{
    public class Cover
    {
        [JsonPropertyName("filename")]
        public string? Filename { get; set; }

        [JsonPropertyName("thumbnail")]
        public string? Thumbnail { get; set; }

        [JsonPropertyName("default")]
        public string? Default { get; set; }

        [JsonPropertyName("md")]
        public string? Md { get; set; }
    }
}
