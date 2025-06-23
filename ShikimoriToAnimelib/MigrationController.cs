using Microsoft.Extensions.Logging;
using ShikimoriToAnimelib.Models.Animelib.Bookmark;
using ShikimoriToAnimelib.Models.Animelib.Search;
using ShikimoriToAnimelib.Models.Shikimori;
using System.Net.Http.Json;
using System.Text.Json;

namespace ShikimoriToAnimelib
{
    public class MigrationController
    {
        public string? ShikimoriCollectionFilePath { get; set; }

        public string? AnimelibSearchUrl { get; set; }

        public string? AnimelibBookmarksUrl { get; set; }

        public string? AnimelibKey { get; set; }

        public Dictionary<string, int>? BookmarkMatching { get; set; }

        public int Interval { get; set; }

        public int TotalBookmarksNumber { get; private set; }

        public readonly List<string> UnaddedBookmarks;

        private readonly ILogger? Logger;

        public MigrationController(ILogger logger, int interval = 700)
        {
            UnaddedBookmarks = new List<string>();
            Interval = interval;
            Logger = logger;
        }

        public async Task RunMigration()
        {
            UnaddedBookmarks.Clear();

            List<ShikimoriBookmark> shikimoriBookmarks = ReadShikimoriCollectionFile(ShikimoriCollectionFilePath!);

            await TransportCollectionToAnimelib(shikimoriBookmarks);
        }

        private List<ShikimoriBookmark> ReadShikimoriCollectionFile(string path)
        {
            using (Logger?.BeginScope("Processing Shikimori collection file"))
            {
                Logger?.LogInformation("Reading a collection file");
                string json = File.ReadAllText(path!);

                Logger?.LogInformation("Deserializing a collection");
                return JsonSerializer.Deserialize<List<ShikimoriBookmark>>(json)!;
            }
        }

        private async Task TransportCollectionToAnimelib(List<ShikimoriBookmark> shikimoriBookmarks)
        {
            using (Logger?.BeginScope("Migration"))
            {
                TotalBookmarksNumber = shikimoriBookmarks.Count;

                for (int i = 0; i < shikimoriBookmarks.Count; i++)
                {
                    ShikimoriBookmark shikimoriBookmark = shikimoriBookmarks[i];
                    Logger?.LogInformation($"\tProgress: {i + 1}/{shikimoriBookmarks.Count}");
                    Logger?.LogInformation($"\tName: {shikimoriBookmark.TargetTitle}");

                    await TransportBookmarkToAnimelib(shikimoriBookmark);
                    await Task.Delay(Interval);
                }
            }
        }

        private async Task TransportBookmarkToAnimelib(ShikimoriBookmark shikimoriBookmark)
        {
            using (Logger?.BeginScope("Transporting the bookmark"))
            {
                SearchRoot searchRoot = await SearchByAnimelib(shikimoriBookmark.TargetTitle!);
                if (searchRoot.Data?.Count < 1)
                {
                    Logger?.LogError($"Anime not found in Animelib database: {shikimoriBookmark.TargetTitle}");
                    RegisterUnaddedBookmark(shikimoriBookmark);

                    return;
                }
                Logger?.LogInformation($"Results count: {searchRoot.Data?.Count.ToString() ?? "0"}");

                SearchData searchData = searchRoot.Data?.FirstOrDefault()!;
                Logger?.LogInformation($"Relevance: {searchData?.EngName ?? "Unknown"} ~ {shikimoriBookmark.TargetTitle}");
                Logger?.LogInformation($"SlugUrl: {searchData?.SlugUrl}");

                await AddBookmark(searchData!, shikimoriBookmark);
            }
        }

        private async Task<SearchRoot> SearchByAnimelib(string query)
        {
            using (Logger?.BeginScope("Search by Animelib"))
            {
                string requestUrl = string.Format(AnimelibSearchUrl!, query);

                Logger?.LogInformation($"Sending a request to {requestUrl}");
                string searchJson = null!;
                using (var httpClient = new HttpClient())
                {
                    searchJson = await httpClient.GetStringAsync(requestUrl);
                }

                Logger?.LogInformation($"Deserializing the response");
                return JsonSerializer.Deserialize<SearchRoot>(searchJson)!;
            }
        }

        private async Task AddBookmark(SearchData searchData, ShikimoriBookmark shikimoriBookmark)
        {
            using (Logger?.BeginScope("Adding a bookmark"))
            {
                Logger?.LogInformation($"Generating a request to add a bookmark");
                AddBookmarkRequest addBookmarkRequest = new AddBookmarkRequest
                {
                    MediaType = "anime",
                    MediaSlug = searchData.SlugUrl,
                    Bookmark = new RequestedBookmark 
                    { 
                        Status = BookmarkMatching![shikimoriBookmark.Status!]
                    },
                    Meta = new RequestedBookmarkMeta(),
                };
                Logger?.LogInformation($"Bookmark request: {addBookmarkRequest.MediaSlug} -> {addBookmarkRequest.Bookmark.Status}");
                Logger?.LogInformation($"Request body: {await JsonContent.Create(addBookmarkRequest).ReadAsStringAsync()}");

                Logger?.LogInformation($"Sending a request");
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AnimelibKey!);
                    httpClient.DefaultRequestHeaders.Add("Site-Id", "5");
                    HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(AnimelibBookmarksUrl, JsonContent.Create(addBookmarkRequest));

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        Logger?.LogInformation($"\t*** Bookmark added successfully");
                    }
                    else
                    {
                        Logger?.LogError($"Error adding to bookmarks: {(int)httpResponseMessage.StatusCode}");
                        Logger?.LogError($"Server answer: {await httpResponseMessage.Content.ReadAsStringAsync()}");

                        RegisterUnaddedBookmark(shikimoriBookmark);
                    }
                }
            }
        }

        private void RegisterUnaddedBookmark(ShikimoriBookmark shikimoriBookmark) =>
            UnaddedBookmarks.Add($"{shikimoriBookmark.TargetTitleRu} ({shikimoriBookmark.TargetTitle})");
    }
}
