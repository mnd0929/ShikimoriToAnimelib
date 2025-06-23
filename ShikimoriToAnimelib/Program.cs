using Microsoft.Extensions.Logging;
using ShikimoriToAnimelib.Logging;
using System.Diagnostics;

namespace ShikimoriToAnimelib
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("\n\tMigrating anime collection from Shikimori to Animelib\n");

            string shikimoriJsonPath = null!;
            string animelibKey = null!;

            do
            {
                Console.Write("Path to exported Shikimori collection file (JSON): ");
                shikimoriJsonPath = Console.ReadLine()!;
            }
            while (string.IsNullOrWhiteSpace(shikimoriJsonPath) || !File.Exists(shikimoriJsonPath));
            
            do
            {
                Console.Write("Animelib authorization key: ");
                animelibKey = Console.ReadLine()!;
            }
            while (string.IsNullOrWhiteSpace(animelibKey));

            Console.Clear();

            using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddProvider(new ConsoleLoggerProvider(level => level >= LogLevel.Debug));
            });

            ILogger logger = loggerFactory.CreateLogger<Program>();

            MigrationController migrationController = new MigrationController(logger, 500)
            {
                ShikimoriCollectionFilePath = shikimoriJsonPath,
                AnimelibSearchUrl = "https://api.cdnlibs.org/api/anime?q={0}",
                AnimelibBookmarksUrl = "https://api.cdnlibs.org/api/bookmarks",
                AnimelibKey = animelibKey,
                BookmarkMatching = new Dictionary<string, int>
                {
                    { "watching", 21 },
                    { "planned", 22 },
                    { "dropped", 23 },
                    { "completed", 24 },
                    { "rewatching", 26 },
                    { "on_hold", 27 },
                }
            };

            Stopwatch migrationStopwatch = Stopwatch.StartNew();
            await migrationController.RunMigration();
            migrationStopwatch.Stop();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"\n\tBookmarks transfer completed ({migrationStopwatch.Elapsed.TotalSeconds}s)");
            Console.WriteLine($"\tBookmarks transferred: {migrationController.TotalBookmarksNumber - migrationController.UnaddedBookmarks.Count} / {migrationController.TotalBookmarksNumber}");

            if (migrationController.UnaddedBookmarks.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nThe following entries could not be added to bookmarks:");
                Console.WriteLine("\n\t" + string.Join("\n\t", migrationController.UnaddedBookmarks));
            }

            Console.ResetColor();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
        }
    }
}
