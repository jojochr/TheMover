using System.Collections.Generic;
using System.IO;
using System.Linq;
using TheMover.Infrastructure.Provider;
using TheMover.UI.Models;
using DomainPackage = TheMover.Domain.Model.MovablePackage;

namespace TheMover.UI.Services {
    public class FrontendMovablePackageBuilder(FileIconService fileIconService) {
        private FileIconService _FileIconService = fileIconService;
        // Todo: Add logger here
        // Todo: Replace with dependency inversion
        private readonly PackageProvider _PackageProvider = PackageProvider.GetInstance();

        internal MovablePackage BuildFromDomainPackage(DomainPackage package) {
            string name = package.PackageName;
            string packageRelativePath = Path.GetRelativePath(_PackageProvider.PackageDirectory.FullName, package.PackageFile.FullName);
            IEnumerable<PackageContainedFile> containedFiles = package.Files.Select((fileInfo) => new PackageContainedFile(_FileIconService.GetFileIcon(fileInfo.FullName), fileInfo.FullName));

            return new MovablePackage(name, packageRelativePath, containedFiles);
        }
    }
}
