using TheMover.UI.Model;

namespace TheMover.UI.ViewModel.Design {
    public class DesignPackagesListViewModel : PackagesListViewModel {
        public DesignPackagesListViewModel() {
            MovablePackages = new(GetDummyPackages(5,6));
            SelectedPackage = MovablePackages.FirstOrDefault();
        }

        private static IEnumerable<MovablePackage> GetDummyPackages(int packageAmount = 2, int fileAmount = 3) {
            List<MovablePackage> files = new();
            for(int i = 1; i <= packageAmount; i++) {
                files.Add(new MovablePackage($"Package {i}", "Just some test path", GetDummyFiles(fileAmount)));
            }
            return files;
        }

        private static IEnumerable<PackageContainedFile> GetDummyFiles(int amount) {
            List<PackageContainedFile> files = new();
            for(int i = 1; i <= amount; i++) {
                files.Add(new(null!, $"SomeFilePath{i}.txt"));
            }
            return files;
        }
    }
}
