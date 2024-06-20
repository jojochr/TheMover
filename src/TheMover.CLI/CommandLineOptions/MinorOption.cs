namespace TheMover.CommandLineOptions {
    internal record MinorOption {
        private MinorOption(string name, string info) {
            Name = name;
            Info = info;

            // Register this Minor-Command in the list of all commands
            MajorOption.AllAllowedArgs.Add(name);
        }

        /// <summary>
        /// This Name is used to match the actual command line input
        /// </summary>
        internal string Name { get; private set; }

        /// <summary>
        /// This Info gets displayed when the --help option gets specified
        /// </summary>
        internal string Info { get; private set; }

        #region Create Major Commands
        internal static MinorOption Interactive =>
            new("-i", "Help for Interactive command");

        internal static MinorOption SourcePaths =>
            new("--SourcePaths", "Help for SourcePaths command");

        internal static MinorOption DestinationPath =>
            new("--DestinationPath", "Help for DestinationPath command");

        internal static MinorOption RenameTo =>
            new("--renameTo", "Help for renameTo command");

        internal static MinorOption Help =>
            new("--help", "");
        #endregion Create Major Commands

        public static implicit operator string(MinorOption arg) => arg.Name;
        public override string ToString() => Name;
    }
}
