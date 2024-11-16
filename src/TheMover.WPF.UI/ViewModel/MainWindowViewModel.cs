using System.IO;
using System.Windows;
using TheMover.Infrastructure.Provider;
using TheMover.Infrastructure.Services;
using TheMover.WPF.UI.Helper;
using TheMover.WPF.UI.Model;
using TheMover.WPF.UI.Provider;
using MessageBox = System.Windows.MessageBox;

namespace TheMover.WPF.UI.ViewModel {
    public class MainWindowViewModel {

        #region ctor
        public MainWindowViewModel() {
            _RepositoryWatcher.RepositoryChanged += () => Refresh(null);

            #region init Commands
            MovePackageToDirectoryXCommand = new RelayCommand(MovePackageToDirectoryX, (_) => PackagesListViewModel.SelectedPackage is not null);
            EditPackageCommand = new RelayCommand(EditPackage, (_) => PackagesListViewModel.SelectedPackage is not null);
            RemovePackageCommand = new RelayCommand(RemovePackage, (_) => PackagesListViewModel.SelectedPackage is not null);
            ExportPackageAsZipCommand = new RelayCommand(ExportPackageAsZip, (_) => PackagesListViewModel.SelectedPackage is not null);
            ImportPackageCommand = new RelayCommand(ImportPackage);
            RefreshCommand = new RelayCommand(Refresh);
            #endregion

        }
        #endregion

        #region prop
        private readonly RepositoryWatcher _RepositoryWatcher = RepositoryWatcher.GetInstance();
        private readonly PackageProvider _PackageProvider = PackageProvider.GetInstance();
        public PackagesListViewModel PackagesListViewModel { get; protected init; } = new();
        #endregion

        #region Commands
        #region Move Package
        public RelayCommand MovePackageToDirectoryXCommand { get; protected set; }
        private void MovePackageToDirectoryX(object? parameter) {
            if(parameter is not MovablePackage movablePackage) {
                return;
            }

            if(false == _PackageProvider.PackageExists(movablePackage.PackageName, out _)) {
                _ = MessageBox.Show($"The Package {movablePackage.PackageName} does not exist, something messed up during refresh.", "Package does not exist", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            FolderBrowserDialog openFileDlg = new();
            var result = openFileDlg.ShowDialog();
            if(string.IsNullOrEmpty(result.ToString())) {
                return;
            }

            Exception? possibleException = _PackageProvider.MovePackage(movablePackage.PackageName, openFileDlg.SelectedPath).Reduce(null!); // Allow null, because we reduce to nullable Exception
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if(possibleException is not null) {
                _ = MessageBox.Show(possibleException.Message, caption: "Move Package Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion Move Package

        #region Edit Package
        public RelayCommand EditPackageCommand { get; protected set; }
        private void EditPackage(object? parameter) {
            _ = MessageBox.Show("The Edit Package Button does not work for now.");
        }
        #endregion

        #region Remove Package
        public RelayCommand RemovePackageCommand { get; protected set; }

        private void RemovePackage(object? parameter) {
            if(parameter is not MovablePackage movablePackage) {
                return;
            }

            if(false == _PackageProvider.PackageExists(movablePackage.PackageName, out _)) {
                _ = MessageBox.Show($"The Package {movablePackage.PackageName} does not exist, something messed up during refresh.", "Package does not exist", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Exception? possibleException = _PackageProvider.DeletePackage(movablePackage.PackageName).Reduce(null!);
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if(possibleException is not null) {
                _ = MessageBox.Show(possibleException.Message, "Package Deletion Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Export Package
        public RelayCommand ExportPackageAsZipCommand { get; protected set; }

        private void ExportPackageAsZip(object? parameter) {
            if(parameter is not MovablePackage movablePackage) {
                return;
            }

            if(false == _PackageProvider.PackageExists(movablePackage.PackageName, out _)) {
                _ = MessageBox.Show($"The Package {movablePackage.PackageName} does not exist, something messed up during refresh.", "Package does not exist", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string pathToDownloadsFolder = KnownFolderProvider.GetPath(KnownFolderProvider.KnownFolder.Downloads);
            FolderBrowserDialog openFileDlg = new() { InitialDirectory = pathToDownloadsFolder };

            var result = openFileDlg.ShowDialog();
            string pathToExportTo = string.IsNullOrEmpty(result.ToString()) == false
                                        ? openFileDlg.SelectedPath
                                        : pathToDownloadsFolder;

            Exception? possibleException = _PackageProvider.ExportPackage(movablePackage.PackageName, pathToExportTo).Reduce(null!); // Allow null, because we reduce to nullable Exception
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if(possibleException is not null) {
                _ = MessageBox.Show(possibleException.Message, "Package Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Import Package
        public RelayCommand ImportPackageCommand { get; protected set; }

        private void ImportPackage(object? parameter) {
            OpenFileDialog openFileDlg = new() { InitialDirectory = KnownFolderProvider.GetPath(KnownFolderProvider.KnownFolder.Downloads), Filter = "Zip Archives (*.zip)|*.zip", Multiselect = false };
            var result = openFileDlg.ShowDialog();
            if(result.ToString().Length == 0) {
                return;
            }

            FileInfo? fileInfoToImport = null;
            if(File.Exists(openFileDlg.FileName)) {
                fileInfoToImport = new FileInfo(openFileDlg.FileName);
            }

            if(fileInfoToImport is null) {
                return;
            }

            Exception? possibleException = _PackageProvider.ImportPackage(fileInfoToImport).Reduce(null!);
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if(possibleException is not null) {
                _ = MessageBox.Show(possibleException.Message, "Package Import Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Refresh
        public RelayCommand RefreshCommand { get; protected set; }

        private void Refresh(object? _) {
            PackagesListViewModel.Refresh();
        }
        #endregion
        #endregion Commands

    }
}
