using System.Collections.Generic;
using System.Linq;
using TheMover.UI.Models;
using TheMover.UI.Services;

namespace TheMover.UI.ViewModels.Design {
    public partial class DesignPackageOperationsViewModel : PackageOperationsViewModel {
        public DesignPackageOperationsViewModel() {
            MovablePackages = new(GetDummyPackages(5, 6));
            SelectedPackage = MovablePackages.FirstOrDefault();
        }

        private static IEnumerable<MovablePackage> GetDummyPackages(int packageAmount = 2, int fileAmount = 3) {
            List<MovablePackage> files = [];
            for(int i = 1; i <= packageAmount; i++) {
                files.Add(new MovablePackage(packageName: $"Package {i}", packagePathRelativeToRepo: "Just some test path", files: GetDummyFiles(fileAmount)));
            }
            return files;
        }

        private static IEnumerable<PackageContainedFile> GetDummyFiles(int amount) {
            List<PackageContainedFile> files = [];
            for(int i = 1; i <= amount; i++) {
                files.Add(new PackageContainedFile(icon: FileIconService.MissingFileIcon, packageRelativePath: $"SomeFilePath{i}.txt"));
            }
            return files;
        }

    }
}
