using TheMover.CommandLineOptions;
using TheMover.Datastructures;

namespace TheMover {
    internal class TheMover {
        #region Constructors

        internal TheMover(string[] args) {

            ValidateArgs(ref args);

            ProgramLoop(ref args);

            Console.WriteLine("Exiting TheMover Constructor");
            Console.ReadLine();
        }

        #endregion  Constructors
        private Option<ArgumentParsingException> ValidateArgs(ref readonly string[] args) {

            /// #Todo
            /// - Hier den Option Type richtig nutzen
            /// - Die Validation fertigstellen
            /// - Und dann ist ungefähr der ganze rest fällig :)
            /// Letzte session habe ich den Grundstein gelegt mit den Major and minor argument typen und einer implementierung meines Configproviders

            for (int i = 0; i < args.Length; i++) {
                if (i == 0) {
                    CheckIfValidMajor(args[i]);
                    continue;
                }

            }

            var invalidargs = args.Where((a) => !CommandLineArg.GetAllArgs().Contains(a));

            if (invalidargs.Any()) {
                Console.WriteLine("Invalidargs:");
                foreach (var arg in invalidargs) {
                    Console.WriteLine($"{arg}");
                }
                return false;
            }

            return true;
        }

        private bool CheckIfValidMajor(string argName) {
            if (argName == MajorOption.Remove) {
                return true;
            } else if (argName == MajorOption.Add) {
                return true;
            } else if (argName == MinorOption.Help) {
                return true;
            } else
                return false;
        }

        private void ProgramLoop(ref readonly string[] args) {
            if (args[0] == CommandLineArg.Command_Add) {
                AddCommand();
            } else if (args[0] == CommandLineArg.Command_Remove) {
                RemoveCommand();
            } else if (args[0] == CommandLineArg.Option_Help) {
                DisplayHelp();
            }

            Console.WriteLine("Program ending...");
        }

        private void AddCommand() {
            Console.WriteLine("Adding a Preset");
            return;
        }

        private void RemoveCommand() {
            Console.WriteLine("Removing a Prese");
            return;
        }

        private void DisplayHelp() {
            Console.WriteLine("Displaying helpful information");
            return;
        }

        #region Custom Exceptions
        public class ArgumentParsingException : Exception {
            public ArgumentParsingException() { }
            public ArgumentParsingException(string message) : base(message) { }
            public ArgumentParsingException(string message, Exception inner) : base(message, inner) { }
        }
        #endregion Custom Exceptions

    }
}
