namespace TheMover.Infrastructure {
    public static class ExtensionMethods {
        public static string TrimFileExtenstion(this FileInfo file) => file.Name.Substring(0, file.Name.Length - file.Extension.Length);
        public static string TrimFileExtenstion(this string fileName) {
            if(string.IsNullOrWhiteSpace(fileName)) {
                return string.Empty;
            }

            FileInfo? fileInfo;

            try {
                fileInfo = new FileInfo(fileName);
            } catch {
                return string.Empty;
            }

            return fileInfo?.TrimFileExtenstion() ?? string.Empty;
        }

        public static void DisposeAll<T>(this IEnumerable<T> toDispose) where T : IDisposable {
            foreach(var thing in toDispose) {
                thing.Dispose();
            }
        }
    }
}
