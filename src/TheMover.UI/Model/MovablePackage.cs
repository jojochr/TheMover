using System.Collections.ObjectModel;
using System.IO;
using TheMover.Infrastructure.Provider;
using TheMover.UI.Helper;
using TheMover.UI.Provider;

namespace TheMover.UI.Model {
    public class MovablePackage : OnPropertyChangedImplementer {
        #region ctor
        public MovablePackage(string packageName, string packagePathRelativeToRepo, IEnumerable<PackageContainedFile> files) {
            _PackageName = packageName;
            _PackagePath = packagePathRelativeToRepo;
            _ContainedFiles = new ObservableCollection<PackageContainedFile>(files);
        }

        public MovablePackage(Domain.Model.MovablePackage package) {
            _PackageName = package.PackageName;

            // This may throw an exception, but that should have happened on initialization
            PackageProvider provider = PackageProvider.GetInstance();
            _PackagePath = Path.GetRelativePath(provider.PackageDirectory.FullName, package.PackageFile.FullName);

            _ContainedFiles = new();
            foreach(FileInfo file in package.Files) {
                _ContainedFiles.Add(new PackageContainedFile(FileIconProvider.GetFileIcon(file.FullName), Path.GetRelativePath(_PackagePath, file.FullName)));
            }
        }
        #endregion

        private string _PackageName;
        public string PackageName {
            get => _PackageName;
            set {
                _PackageName = value;
                OnPropertyChanged();
            }
        }

        private string _PackagePath;
        public string PackagePath {
            get => _PackagePath;
            set {
                _PackagePath = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<PackageContainedFile> _ContainedFiles;
        public ObservableCollection<PackageContainedFile> ContainedFiles {
            get => _ContainedFiles;
            set {
                _ContainedFiles = value;
                OnPropertyChanged();
            }
        }
    }
}
