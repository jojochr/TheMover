namespace TheMover.Infrastructure.Provider {
    public class PackageProviderInitializationException(string message, Exception innerException) : Exception(message, innerException);
}
