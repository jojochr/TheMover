using System.Collections.Immutable;

namespace TheMover.Domain.Model {
    public sealed class MovablePackage(string name, FileInfo packageFile, IEnumerable<FileInfo> files) {
        public string PackageName { get; private set; } = name;
        public FileInfo PackageFile { get; private set; } = packageFile;
        public ImmutableArray<FileInfo> Files { get; private set; } = [..files,];
    }
}
