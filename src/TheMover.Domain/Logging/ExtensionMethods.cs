namespace TheMover.Domain.Logging {
    internal static class ExtensionMethods {
        /// <summary>Interprets a double as unix timestamp and converts it to <see cref="DateTime"/></summary>
        public static DateTime ConvertFromUnixTimestamp(this double timestamp) => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestamp);

        /// <summary>Converts a <see cref="DateTime"/> into unix timestamp</summary>
        public static long ConvertToUnixTimestamp(this DateTime date) => new DateTimeOffset(date).ToUnixTimeSeconds();
    }
}
