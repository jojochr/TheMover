using System.IO;
using System.Reflection;
using System.Windows;
using MessageBox = System.Windows.MessageBox;

namespace TheMover.UI.Provider {
    internal static class FileIconProvider {
        internal static readonly Icon MissingFileIcon = GetMissingFileIconFromAssembly();

        public static Icon GetFileIcon(string path) {
            if(Path.Exists(path) == false) {
                return MissingFileIcon;
            }

            return Icon.ExtractAssociatedIcon(path) ?? MissingFileIcon;
        }

        #region Helper
        private const string c_MissingFileIconName = @"MissingFileIcon.ico";

        private static Icon GetMissingFileIconFromAssembly() {
            Assembly asm = Assembly.GetExecutingAssembly();

            foreach(string resourceName in asm.GetManifestResourceNames()) {
                if(resourceName.EndsWith(c_MissingFileIconName) == false) {
                    continue;
                }

                Stream directStream = asm.GetManifestResourceStream(resourceName)!;
                return new Icon(directStream);
            }

            _ = MessageBox.Show($"\"{c_MissingFileIconName}\"-Icon was not found.", "Icon not found", MessageBoxButton.OK);

            throw new Exception();
        }
        #endregion Helper
    }
}
