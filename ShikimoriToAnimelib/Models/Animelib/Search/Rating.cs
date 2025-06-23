using System.Text.Json.Serialization;

namespace ShikimoriToAnimelib.Models.Animelib.Search
{
    public class Rating
    {
        [JsonPropertyName("average")]
        public string? Average { get; set; }

        [JsonPropertyName("averageFormated")]
        public string? AverageFormated { get; set; }

        [JsonPropertyName("votes")]
        public int? Votes { get; set; }

        [JsonPropertyName("votesFormated")]
        public string? VotesFormated { get; set; }

        [JsonPropertyName("user")]
        public int? User { get; set; }
    }
}
