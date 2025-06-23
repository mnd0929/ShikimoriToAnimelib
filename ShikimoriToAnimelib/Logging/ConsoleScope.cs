namespace ShikimoriToAnimelib.Logging
{
    public class ConsoleScope : IDisposable
    {
        private static readonly AsyncLocal<Stack<object>> Scopes = new();

        public static string CurrentScopes
        {
            get
            {
                if (Scopes.Value == null || Scopes.Value.Count == 0)
                    return "";

                return string.Join(" > ", Scopes.Value.Reverse()) + ": ";
            }
        }

        public static IDisposable Push(object state)
        {
            Scopes.Value ??= new Stack<object>();
            Scopes.Value.Push(state);
            return new ConsoleScope();
        }

        public void Dispose()
        {
            if (Scopes.Value != null && Scopes.Value.Count > 0)
                Scopes.Value.Pop();
        }
    }
}
