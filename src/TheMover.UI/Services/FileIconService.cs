using System;
using System.IO;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace TheMover.UI.Services {
    public class FileIconService {
        public FileIconService() { }

        private const string C_MISSING_FILE_ICON_NAME = @"MissingFileIcon.bmp";
        private static readonly object _Lock = new object();

        internal static readonly Bitmap MissingFileIcon = new Bitmap(AssetLoader.Open(new Uri(uriString: $"avares://TheMover.UI/Assets/{C_MISSING_FILE_ICON_NAME}")));

        internal Bitmap GetFileIcon(string path) {
            if(Path.Exists(path) == false) {
                return MissingFileIcon;
            }

            try {
                // Just to be sure :)
                lock(_Lock) {
                    FileStream fileToGetIconFrom = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                    return new Bitmap(fileToGetIconFrom);
                }
            } catch {
                //Todo: Log an error here
                return MissingFileIcon;
            }
        }
    }
}
