using System.Diagnostics;
using TheMover.Domain.Model.Primitives;

namespace TheMover.Domain.Logging {
    public class Logger {
        #region ctor

        private Logger() {
            UseLogDirectory = true;
#if DEBUG
            _UseConsole = true;
#else
            _UseConsole = false;
#endif

            LogDirectory = Directory.CreateDirectory(DefaultLogPath);
        }

        private static Logger? _Instance;

        /// <summary>
        /// Gets the Instance of this Singleton<br></br>
        /// May throw an Exception if this got not correctly initialized.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="LoggerInitializationException"></exception>
        public static Logger GetInstance() {
            try {
                return _Instance ?? new Logger();
            } catch(Exception e) {
                throw new LoggerInitializationException($"Thrown in \"{nameof(GetInstance)}\"", e);
            }
        }

        /// <summary>
        /// This can be used to Initialize the Singleton to make sure everything is fine on application startup and handle/display Exceptions
        /// </summary>
        public static Option<LoggerInitializationException> InitializeLogger() {
            try {
                _Instance = new Logger();
                return Option<LoggerInitializationException>.None;
            } catch(Exception e) {
                return new LoggerInitializationException($"Thrown in \"{nameof(InitializeLogger)}\"\r\nAn Error occured while initializing the {nameof(Logger)}", innerException: e);
            }
        }
        #endregion ctor

        #region Constants

        public static readonly string DefaultLogPath = Path.Combine(Directory.GetCurrentDirectory(), "log");
        public readonly DirectoryInfo LogDirectory;

        #endregion Constants

        public bool UseLogDirectory;

        /// <summary>
        /// This may remain unused
        /// </summary>
        private bool _UseConsole;

        public void Log(LogMessage message) {
            if(_UseConsole) {
                ConsoleLogger.LogMessage(message);
            }
            if(UseLogDirectory) {
                FileLogger.LogMessage(message, LogDirectory);
            }
        }

        public void Log(Exception ex) {
        }


        #region Nested Logging classes
        private static class ConsoleLogger {
            public static void LogMessage(LogMessage message) {
                Debug.WriteLine(message.Message);
            }
        }

        private static class FileLogger {
            public static void LogMessage(LogMessage message, DirectoryInfo dirInfo) { }
        }

        #endregion Nested Logging classes
    }

}
