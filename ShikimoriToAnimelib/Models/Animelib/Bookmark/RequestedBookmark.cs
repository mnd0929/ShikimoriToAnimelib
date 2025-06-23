using System.Text.Json.Serialization;

namespace ShikimoriToAnimelib.Models.Animelib.Bookmark
{
    public class RequestedBookmark
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }
    }
}
