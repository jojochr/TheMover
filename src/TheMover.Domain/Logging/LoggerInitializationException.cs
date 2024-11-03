namespace TheMover.Domain.Logging {
    public class LoggerInitializationException : Exception {
        public LoggerInitializationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
