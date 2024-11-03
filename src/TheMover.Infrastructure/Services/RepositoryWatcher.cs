using TheMover.Infrastructure.Provider;

namespace TheMover.Infrastructure.Services {
    public sealed class RepositoryWatcher {
        #region ctor

        private RepositoryWatcher() {
            _Instance = this;
            _FileSystemWatcher = GetNewFileSystemWatcher();
            SubscribeToFileSystemEvents();
        }

        private FileSystemWatcher _FileSystemWatcher;
        private static RepositoryWatcher? _Instance = null;

        public static RepositoryWatcher GetInstance() => _Instance ?? new RepositoryWatcher();
        private static FileSystemWatcher GetNewFileSystemWatcher() {
            var watcher = new FileSystemWatcher(PackageProvider.GetInstance().PackageDirectory.FullName) {
                EnableRaisingEvents = true,
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.Attributes
                             | NotifyFilters.CreationTime
                             | NotifyFilters.DirectoryName
                             | NotifyFilters.FileName
                             | NotifyFilters.LastAccess
                             | NotifyFilters.LastWrite
                             | NotifyFilters.Security
                             | NotifyFilters.Size
            };

            return watcher;
        }

        #endregion

        private void SubscribeToFileSystemEvents() {
            _FileSystemWatcher.Changed += RaiseRepositoryChanged;
            _FileSystemWatcher.Created += RaiseRepositoryChanged;
            _FileSystemWatcher.Deleted += RaiseRepositoryChanged;
            _FileSystemWatcher.Renamed += RaiseRepositoryChanged;
            _FileSystemWatcher.Error += HandleFileSystemWatcherError;
        }

        private void HandleFileSystemWatcherError(object sender, EventArgs args) {
            _FileSystemWatcher = GetNewFileSystemWatcher();
            SubscribeToFileSystemEvents();
        }

        public delegate void RepositoryChangedEventHandler();

        public event RepositoryChangedEventHandler? RepositoryChanged;

        private void RaiseRepositoryChanged(object? o, EventArgs? args) => RepositoryChanged?.Invoke();
    }
}
