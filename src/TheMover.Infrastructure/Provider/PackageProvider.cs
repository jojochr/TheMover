using System.Globalization;
using System.IO.Compression;
using TheMover.Domain.Logging;
using TheMover.Domain.Model;
using TheMover.Domain.Model.Primitives;
using TheMover.Infrastructure.Services;

namespace TheMover.Infrastructure.Provider {
    /// <summary>
    /// This contains the Logic to do all the Package-Operations needed.<br/>
    /// This Class is meant to be used as a Singleton. It however does not provide a "GetInstance"-Method so please use your Dependency-Injection-Framework of choice.
    /// </summary>
    public class PackageProvider {

        #region ctor
        private PackageProvider(Logger logger) {
            _Logger = logger;
        }

        /// <summary> This can be used to retrieve an initialized Instance of the <see cref="PackageProvider"/>-Class</summary>
        public static Result<PackageProvider, PackageProviderInitializationException> GetInitializedPackageProvider(Logger logger) {
            try {
                return new PackageProvider(logger);
            } catch(Exception e) {
                return new PackageProviderInitializationException($"Thrown in \"{nameof(GetInitializedPackageProvider)}\"\r\nAn Error occured while initializing the {nameof(PackageProvider)}",
                                                                  innerException: e);
            }
        }
        #endregion ctor

        #region prop
        public readonly DirectoryInfo PackageDirectory = Directory.CreateDirectory(_DefaultPackagePath);
        private static readonly string _DefaultPackagePath = Path.Combine(Directory.GetCurrentDirectory(), path2: "packages");

        private static readonly TimeSpan _FileSystemTimeout = TimeSpan.FromSeconds(value: 10);

        private readonly Logger _Logger;

        private static object _Padlock = new object();
        #endregion prop

        #region Get Packages
        /// <summary>
        /// This retrieves Information about what packages are stored currently<br></br>
        /// Error handling is not fully complete -> May throw if Directory is not accessible or something
        /// </summary>
        public Result<List<MovablePackage>, Exception> GetMovablePackages() {
            List<FileInfo> zipFilesInPackagesDirectory;
            {
                var zippedFilesResult = GetZippedPackagesFromIO();

                // Return if error
                if(false == zippedFilesResult.Dump(out var _, out Exception? error, out bool _)) {
                    return error!; // Error must be non nul if we end up here
                }

                zipFilesInPackagesDirectory = zippedFilesResult.Match(files => files, error => new());
            }

            List<MovablePackage> packages = [];
            // Return if error
            foreach(var file in zipFilesInPackagesDirectory) {
                GetPackageFromZipFile(file).Match(success: packages.Add, failure: _Logger.Log); // For now just log errors
            }

            return packages;
        }

        /// <summary>Gets all the package-archives files from <see cref="PackageDirectory"/><br></br>
        /// Remember that all those zip archives should be disposed in the end.
        /// </summary>
        private Result<List<FileInfo>, Exception> GetZippedPackagesFromIO() {
            var unfilteredFiles = PackageDirectory.GetFiles();

            var filteredItems = new List<FileInfo>();

            foreach(FileInfo file in unfilteredFiles) {
                if(file.Exists == false) {
                    continue;
                }

                if(string.IsNullOrEmpty(file.Extension)) {
                    continue;
                }

                // If the file has another Extension than .zip
                if(0 != string.Compare(file.Extension, ".zip", ignoreCase: true, culture: CultureInfo.InvariantCulture)) {
                    continue;
                }

                filteredItems.Add(file);
            }

            return filteredItems;
        }

        private Result<MovablePackage, Exception> GetPackageFromZipFile(FileInfo zipFile) {
            // Wait for File access before starting
            if(zipFile.IsBlockedBySomeProcess()) {
                _ = zipFile.WaitForFileAccess(_FileSystemTimeout);
            }

            List<FileInfo> archivedFiles = new();

            // I do not care about this exception for now. This code may explode...
            using(var zipArchive = ZipFile.OpenRead(zipFile.FullName)) {
                if(zipArchive is null) {
                    try {
                        File.Delete(zipFile.FullName);
                        _Logger.Log(new LogMessage("Deleted empty Zip-Packagefile.", LogMessageSeverity.Information));
                        return new Exception("Empty Zip file in Package repository detected and deleted.");
                    } catch(Exception e) {
                        return new Exception("Error while deleting an Empty ZipArchive", e);
                    }
                }

                foreach(ZipArchiveEntry entry in zipArchive.Entries) {
                    try {
                        archivedFiles.Add(new FileInfo(Path.Combine(zipFile.FullName, entry.FullName)));
                    } catch {
                        continue;
                    }
                }
            }

            return new MovablePackage(zipFile.Name, zipFile, archivedFiles);
        }
        #endregion Get Packages

        #region Move Package
        /// <summary>
        /// This identifies a package via <paramref name="packageName"/> and moves it to <paramref name="destinationPath"/><br></br>
        /// This will create the directory <paramref name="destinationPath"/> if it does not exist already
        /// </summary>
        /// <param name="packageName">Identifier for the Package to be moved</param>
        /// <param name="destinationPath">The Path where the moved package should end up at</param>
        /// <returns>If an exception is returned it will either be a <see cref="ArgumentException"/> or any of exceptions thrown by Directory.CreateDirectory</returns>
        public Option<Exception> MovePackage(string packageName, string destinationPath) {
            if(false == PackageExists(packageName, out FileInfo? packageToMove) || packageToMove is null) {
                return new ArgumentException("The PackageName does not exist, that means the refresh messed up somewhere.", nameof(packageName));
            }

            // Return exception if the DestinationPath does not exist and cannot be created
            try {
                _ = Directory.CreateDirectory(destinationPath);
            } catch(Exception e) { return e; }

            // Wait for File access before starting
            if(packageToMove.IsBlockedBySomeProcess()) {
                _ = packageToMove.WaitForFileAccess(_FileSystemTimeout);
            }

            using(ZipArchive zipArchiveToMove = ZipFile.OpenRead(packageToMove.FullName)) {
                foreach(var entry in zipArchiveToMove.Entries) {
                    string targetFile = Path.Combine(destinationPath, entry.FullName);

                    if(Path.EndsInDirectorySeparator(targetFile)) {
                        _ = Directory.CreateDirectory(targetFile);
                        continue;
                    }

                    entry.ExtractToFile(targetFile, true);
                }
            }

            return Option<Exception>.None;
        }
        #endregion Move Packages

        #region Delete Package
        /// <summary>This identifies a package via <paramref name="packageName"/> and deletes it from the Repository</summary>
        /// <param name="packageName">Identifier for the Package to be moved</param>
        /// <returns>If an exception is returned it will either be a <see cref="ArgumentException"/></returns>
        public Option<Exception> DeletePackage(string packageName) {
            if(false == PackageExists(packageName, out FileInfo? packageToDelete) || packageToDelete is null) {
                return new ArgumentException("The PackageName does not exist, that means the refresh messed up somewhere.", nameof(packageName));
            }

            // Wait for File access before starting
            if(packageToDelete.IsBlockedBySomeProcess()) {
                _ = packageToDelete.WaitForFileAccess(_FileSystemTimeout);
            }

            // For now we wont check if we are still blocked. We may just explode here
            File.Delete(packageToDelete.FullName);

            return Option<Exception>.None;
        }
        #endregion

        #region Export Package
        /// <summary>
        /// This identifies a package via <paramref name="packageName"/> and exports it to <paramref name="destinationPath"/><br/>
        /// Exporting means to copy the Zip-File, that contains all the files to the <paramref name="destinationPath"/><br/>
        /// This will create the directory <paramref name="destinationPath"/> if it does not exist already.
        /// </summary>
        /// <param name="packageName">Identifier for the Package to be moved</param>
        /// <param name="destinationPath">The Path where the moved package should end up at</param>
        /// <returns>If an exception is returned it will either be a <see cref="ArgumentException"/> or any of exceptions thrown by Directory.CreateDirectory or File.Copy</returns>
        public Option<Exception> ExportPackage(string packageName, string destinationPath) {
            if(false == PackageExists(packageName, out FileInfo? packageToExport) || packageToExport is null) {
                return new ArgumentException("The PackageName does not exist, that means the refresh messed up somewhere.", nameof(packageName));
            }

            // Return exception if the DestinationPath does not exist and cannot be created
            try {
                _ = Directory.CreateDirectory(destinationPath);
            } catch(Exception e) { return e; }

            // Wait for File access before starting
            if(packageToExport.IsBlockedBySomeProcess()) {
                _ = packageToExport.WaitForFileAccess(_FileSystemTimeout);
            }

            // Return exception if the package can not be copied
            try {
                File.Copy(packageToExport.FullName, Path.Combine(destinationPath, packageToExport.Name), overwrite: true);
            } catch(Exception e) { return e; }

            return Option<Exception>.None;
        }
        #endregion

        #region Import Package
        /// <summary>This takes a given file, checks if it is a .zip file (Meaning a valid package file) and then copies it into the package Repository<br/></summary>
        /// <param name="sourceFile">The file that is to be imported to the repository</param>
        /// <returns>If an exception is returned it will either be a <see cref="ArgumentException"/> or any of exceptions thrown by File.Copy</returns>
        public Option<Exception> ImportPackage(FileInfo sourceFile) {
            if(false == string.Equals(sourceFile.Extension, ".zip", StringComparison.InvariantCultureIgnoreCase)) {
                throw new ArgumentException("Provided File is not a zip file.", nameof(sourceFile));
            }

            if(sourceFile.Name.Length == 0) {
                throw new ArgumentException("Name of the exported file is not valid.", nameof(sourceFile));
            }

            if(PackageExists(sourceFile.Name, out _)) {
                return new ArgumentException($"The PackageName {sourceFile.Name} already exists.", nameof(sourceFile));
            }

            // Wait for File access before starting
            if(sourceFile.IsBlockedBySomeProcess()) {
                _ = sourceFile.WaitForFileAccess(_FileSystemTimeout);
            }

            // Return exception if the package can not be copied
            try {
                File.Copy(sourceFile.FullName, Path.Combine(PackageDirectory.FullName, sourceFile.Name));
            } catch(Exception e) { return e; }

            return Option<Exception>.None;
        }
        #endregion

        #region Helper
        /// <summary>
        /// This method can be used to check if a package exists.
        /// </summary>
        /// <param name="packageNameToFind"></param>
        /// <param name="packageToFind">If the package is not found this will be null, if this method returns true you can assume this to be non null</param>
        /// <returns>True -> If the package exists<br></br>False -> Package does not exist</returns>
        public bool PackageExists(string packageNameToFind, out FileInfo? packageToFind) {
            packageNameToFind = packageNameToFind.TrimFileExtenstion();
            packageToFind = GetZippedPackagesFromIO().Match(x => x, err => [])
                                                     .FirstOrDefault(file => packageNameToFind == file.Name.TrimFileExtenstion());

            return packageToFind is not null;
        }
        #endregion Helper

    }
}
