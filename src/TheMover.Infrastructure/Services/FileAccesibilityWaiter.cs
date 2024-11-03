namespace TheMover.Infrastructure.Services {
    internal static class FileAccesibilityWaiter {
        /// <summary>This is a method that may block for the time specified in <paramref name="timeout"/></summary>
        /// <returns>True -> If the file is accessible right now<br/>False -> if we ran into a timeout and the file is still inaccessible</returns>
        public static bool WaitForFileAccess(this FileInfo fileToWaitFor, TimeSpan timeout) {
            bool hasBeenCanceled = false;
            CancellationTokenSource source = new CancellationTokenSource();
            Timer timeOutTimer = new((o) => {
                source.Cancel(false);
                hasBeenCanceled = true;
            }, null, timeout, Timeout.InfiniteTimeSpan);

            Task.Run(() => WaitTillFileIsAvailableAsync(fileToWaitFor, source.Token), source.Token).GetAwaiter().GetResult();

            // If we had to cancel we were not successful and the file is still locked
            return !hasBeenCanceled;
        }

        /// <summary>CAUTION!!! This will block until some file is accessible. Please only use this with a cancellation Token and use it.</summary>
        /// <param name="file">File to be monitored</param>
        private static async Task WaitTillFileIsAvailableAsync(FileInfo file, CancellationToken token) {
            bool fileIsBlocked = IsBlockedBySomeProccess(file);

            using var watcher = new FileSystemWatcher(file.DirectoryName!, $"*{file.Name}{file.Extension}") {
                EnableRaisingEvents = true,
            };

            // On file changes check if we are still blocked
            watcher.Changed += (_, _) => fileIsBlocked = file.IsBlockedBySomeProccess();

            do {
                if(fileIsBlocked == false || token.IsCancellationRequested) {
                    return;
                }

                // Every timespan X check if we are still blocked and return if not
                await Task.Delay(TimeSpan.FromMilliseconds(100));
            } while(true);
        }

        /// <summary>Returns if a File is currently blocked by any Process</summary>
        /// <param name="fileToBeChecked"></param>
        /// <returns>True -> If the file is currently blocked<br/>False -> If the file is accessible right now</returns>
        public static bool IsBlockedBySomeProccess(this FileInfo fileToBeChecked) {
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
            try {
                using FileStream inputStream = File.Open(fileToBeChecked.FullName, FileMode.Open, FileAccess.Read, FileShare.None);
                return false;
            } catch(Exception) {
                return true;
            }
        }
    }
}
