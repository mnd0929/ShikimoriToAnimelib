using System.Text.Json.Serialization;

namespace ShikimoriToAnimelib.Models.Animelib.Search
{
    public class SearchRoot
    {
        [JsonPropertyName("data")]
        public List<SearchData>? Data { get; set; }

        [JsonPropertyName("links")]
        public Links? Links { get; set; }

        [JsonPropertyName("meta")]
        public Meta? Meta { get; set; }
    }
}
