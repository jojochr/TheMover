namespace TheMover.Domain.Logging {
    public class LogMessage {
        public LogMessage(string message, LogMessageSeverity severity) {
            Message = message;
            Severity = severity;
        }

        /// <summary>
        /// In case you really need to panic because nothing makes sense just use this pattern<br></br>
        /// </summary>
        /// <param name="e"></param>
        public LogMessage(Exception e) {
            // #Todo Stringify Exception correctly
            Message = e.Message;

            Severity = LogMessageSeverity.Panic;
        }

        public string Message { get; private set; }
        public LogMessageSeverity Severity { get; private set; }
    }
}
