namespace TheMover.Domain.Logging {
    public class LogMessage {
        public LogMessage(string message, LogMessageSeverity severity, ErrorKey? errorKey = null) {
            ErrorKey = errorKey;
            Message = message;
            Severity = severity;
        }

        /// <summary>This Constructor is used to create a <see cref="LogMessage"/> from an <see cref="ErrorBase"/></summary>
        public LogMessage(ErrorBase error) {
            Identifier = error.Identifier;
            ErrorKey = error.ErrorKey;
            Message = error.LongMessage;

            Severity = error.Severity switch {
                ErrorSeverity.Warning => LogMessageSeverity.Waring,
                ErrorSeverity.Error => LogMessageSeverity.Error,
                ErrorSeverity.Panic => LogMessageSeverity.Panic,
                var _ => LogMessageSeverity.Panic,
            };
        }

        /// <summary>In case you really need to panic because nothing makes sense just use this pattern</summary>
        public LogMessage(Exception e) {
            ErrorKey = Logging.ErrorKey.Panic;

            //Todo: Stringify Exception correctly
            Message = e.Message;

            Severity = LogMessageSeverity.Panic;
        }

        public readonly Guid Identifier = Guid.NewGuid();
        public readonly ErrorKey? ErrorKey;
        public readonly string Message;
        public readonly LogMessageSeverity Severity;
    }
}
