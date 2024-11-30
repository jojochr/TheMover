using TheMover.Domain.Logging;
using TheMover.Infrastructure.Provider;

namespace TheMover.Infrastructure.Services {
    /// <summary>This Class can be used to look out for changes in the package Repository</summary>
    public sealed class RepositoryWatcher {

        #region ctor
        public RepositoryWatcher(PackageProvider packageProvider, Logger logger) {
            _Logger = logger;
            _DirectoryToWatch = packageProvider.PackageDirectory;
            _FileSystemWatcher = GetNewFileSystemWatcher(packageProvider.PackageDirectory);
            SubscribeToFileSystemEvents();
        }


        private static FileSystemWatcher GetNewFileSystemWatcher(DirectoryInfo directoryInfo) {
            return new FileSystemWatcher(directoryInfo.FullName) {
                                                                     EnableRaisingEvents = true
                                                                   , IncludeSubdirectories = true
                                                                   , NotifyFilter = NotifyFilters.Attributes
                                                                                  | NotifyFilters.CreationTime
                                                                                  | NotifyFilters.DirectoryName
                                                                                  | NotifyFilters.FileName
                                                                                  | NotifyFilters.LastAccess
                                                                                  | NotifyFilters.LastWrite
                                                                                  | NotifyFilters.Security
                                                                                  | NotifyFilters.Size
                                                                 };
        }
        #endregion

        private Logger _Logger;
        private DirectoryInfo _DirectoryToWatch;
        private FileSystemWatcher _FileSystemWatcher;

        public void SetNewDirectoryToWatch(DirectoryInfo directoryInfo) {
            _DirectoryToWatch = directoryInfo;
            _FileSystemWatcher = GetNewFileSystemWatcher(directoryInfo);
            SubscribeToFileSystemEvents();
        }

        private void SubscribeToFileSystemEvents() {
            _FileSystemWatcher.Changed += RaiseRepositoryChanged;
            _FileSystemWatcher.Created += RaiseRepositoryChanged;
            _FileSystemWatcher.Deleted += RaiseRepositoryChanged;
            _FileSystemWatcher.Renamed += RaiseRepositoryChanged;
            _FileSystemWatcher.Error += HandleFileSystemWatcherError;
        }

        private void HandleFileSystemWatcherError(object sender, ErrorEventArgs errorArgs) {
            _FileSystemWatcher = GetNewFileSystemWatcher(_DirectoryToWatch);
            SubscribeToFileSystemEvents();
        }

        public delegate void RepositoryChangedEventHandler();

        public event RepositoryChangedEventHandler? RepositoryChanged;
        private void RaiseRepositoryChanged(object? o, EventArgs? args) => RepositoryChanged?.Invoke();
    }
}
