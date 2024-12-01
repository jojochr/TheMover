namespace TheMover.Domain.Logging {
    public abstract class ErrorBase {
        public Guid Identifier { get; private set; } = Guid.NewGuid();
        public abstract ErrorSeverity Severity { get; protected set; }
        public abstract ErrorKey ErrorKey { get; protected set; }
        public abstract string Description { get; protected set; }
        public abstract string LongMessage { get; protected set; }
        public IDictionary<string, object> ErrorAttributes { get; protected set; } = new Dictionary<string, object>();
    }
}
