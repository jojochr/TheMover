using System.Collections.Generic;
using System.IO;
using System.Linq;
using TheMover.Domain.Logging;
using TheMover.Infrastructure.Provider;
using TheMover.UI.Models;
using DomainPackage = TheMover.Domain.Model.MovablePackage;

namespace TheMover.UI.Services {
    public class FrontendMovablePackageBuilder(Logger logger, PackageProvider packageProvider, FileIconService fileIconService) {
        internal MovablePackage BuildFromDomainPackage(DomainPackage package) {
            string name = package.PackageName;
            string packageRelativePath = Path.GetRelativePath(packageProvider.PackageDirectory.FullName, package.PackageFile.FullName);
            IEnumerable<PackageContainedFile> containedFiles = package.Files.Select((fileInfo) => new PackageContainedFile(fileIconService.GetFileIcon(fileInfo.FullName), fileInfo.FullName));

            return new MovablePackage(name, packageRelativePath, containedFiles);
        }
    }
}
