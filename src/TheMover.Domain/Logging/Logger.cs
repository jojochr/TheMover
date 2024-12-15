namespace TheMover.Domain.Logging {
    public class Logger(bool useConsole = false, bool useLogDirectory = true) {
        private readonly ConsoleLogger? _ConsoleLogger = useConsole ? new ConsoleLogger() : null;
        private readonly FileLogger? _FileLogger = useLogDirectory ? new FileLogger() : null;

        private readonly object _Padlock = new object();

        public void Log(LogMessage message) {
            lock(_Padlock) {
                _ConsoleLogger?.Log(message);
                _FileLogger?.Log(message);
            }
        }

        private class ConsoleLogger {
            public ConsoleLogger() {
                ConsoleHelper.CreateConsole();
            }

            public void Log(LogMessage message) {
                // Todo: Extend Logging to console:
                //  - Color
                //  - Better Message
                //  - Timestamp
                //  - Guid
                //  - Separate Log entries by blank lines
                Console.WriteLine(message.Message);
            }
        }

        private class FileLogger {
            private static readonly string _DefaultLogPath = Path.Combine(Directory.GetCurrentDirectory(), path2: "log");
            private readonly DirectoryInfo _LogDirectory = Directory.CreateDirectory(_DefaultLogPath);

            public void Log(LogMessage message) { }
        }
    }
}
