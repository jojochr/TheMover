namespace TheMover.CommandLineOptions {
    internal record MajorOption {
        private MajorOption(string name, string info, MinorOption[] allowedOptions) {
            Name = name;
            Info = info;
            AllowedOptions = allowedOptions;

            // Register this Major-Command in the list of all commands
            AllAllowedArgs.Add(name);
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
        internal static MajorOption Add {
            get {
                return new MajorOption("add"
                    , "Help of add command"
                    , []
                    );
            }
        }
        internal static MajorOption Remove {
            get {
                return new MajorOption("remove"
                    , "Help of remove command"
                    , []
                    );
            }
        }
        #endregion Create Major Commands

        /// <summary>
        /// In this array the allowed <seealso cref="MinorOption"/>s for this <seealso cref="MajorOption"/> are specified
        /// </summary>
        public MinorOption[] AllowedOptions { get; private set; }

        /// <summary>
        /// Globally available List that contains the name of every existing Minor and Major commands.
        /// </summary>
        internal static readonly List<string> AllAllowedArgs = new List<string>();

        public static implicit operator string(MajorOption arg) => arg.Name;
        public override string ToString() => Name;
    }
}
