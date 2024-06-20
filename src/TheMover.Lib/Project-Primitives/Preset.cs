using System.Collections.Generic;

namespace TheMover.Primitives{
    internal struct Preset {
        internal Preset(string presetName, List<string> sourceFiles, string destnationPath) {
            PresetName = presetName;
            SourceFiles = sourceFiles;
            DestiantionPath = destnationPath;
        }

        /// <summary>
        /// The name of this Preset.
        /// </summary>
        internal string PresetName { private set; get; }

        /// <summary>
        /// A list of Fullpaths.
        /// </summary>
        internal List<string> SourceFiles { private set; get; }

        /// <summary>
        /// The Destinationpath of this Preset.
        /// </summary>
        internal string DestiantionPath { private set; get; }
    }
}