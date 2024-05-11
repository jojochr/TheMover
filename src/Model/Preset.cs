using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TheMover.Model {
    internal struct Preset {
        /// <summary>
        /// #Todo
        /// </summary>
        /// <param name="presetName"></param>
        /// <param name="sourceFiles"></param>
        /// <param name="destnationPath"></param>
        /// <exception cref="ArgumentException"></exception>
        internal Preset(string presetName, List<string> sourceFiles, string destnationPath) {
            PresetName = presetName;
            SourceFiles = sourceFiles;
            DestiantionPath = destnationPath;

            if (sourceFiles.Count == 0) {
                throw new ArgumentException("At least one SourceFile must be specified!");
            }
        }

        /// <summary>
        /// The name of this Preset.
        /// </summary>
        internal string PresetName { private set; get; }
        
        /// <summary>
        /// A list of Fullpaths that ist guaranteed to at least contain one item.
        /// </summary>
        internal List<string> SourceFiles { private set; get; }

        /// <summary>
        /// The Destinationpath of this Preset.
        /// </summary>
        internal string DestiantionPath { private set; get; }
    }
}
