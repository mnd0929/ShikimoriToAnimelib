using System.Text.Json.Serialization;

namespace ShikimoriToAnimelib.Models.Animelib.Bookmark
{
    public class AddBookmarkRequest
    {
        [JsonPropertyName("media_type")]
        public string? MediaType { get; set; }

        [JsonPropertyName("media_slug")]
        public string? MediaSlug { get; set; }

        [JsonPropertyName("bookmark")]
        public RequestedBookmark? Bookmark { get; set; }

        [JsonPropertyName("meta")]
        public RequestedBookmarkMeta? Meta { get; set; }
    }
}
