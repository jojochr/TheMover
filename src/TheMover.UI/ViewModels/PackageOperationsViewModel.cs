using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using ReactiveUI;
using TheMover.Infrastructure.Provider;
using TheMover.UI.Models;
using TheMover.UI.Services;

namespace TheMover.UI.ViewModels {
    public partial class PackageOperationsViewModel : ObservableObject {
#pragma warning disable CS8618, CS9264
        /// <summary>This is just used for DesignData, and it initializes everything with null.</summary>
        protected PackageOperationsViewModel() { }
#pragma warning restore CS8618, CS9264

        public PackageOperationsViewModel(PackageProvider provider, FrontendMovablePackageBuilder builder) {
            _PackageProvider = provider;
            _PackageBuilder = builder;
            MovablePackages = GetFrontendPackages();
        }

        private readonly PackageProvider _PackageProvider;
        private readonly FrontendMovablePackageBuilder _PackageBuilder;

        [ObservableProperty]private MovablePackage? _SelectedPackage;
        [ObservableProperty]private ObservableCollection<MovablePackage> _MovablePackages;

        private ObservableCollection<MovablePackage> GetFrontendPackages() {
            return _PackageProvider.GetMovablePackages()
                                   .Match<ObservableCollection<MovablePackage>>
                                        (success: (packages) => new ObservableCollection<MovablePackage>(packages.Select(x => _PackageBuilder.BuildFromDomainPackage(x))),
                                         //Todo: Log this error
                                         failure: (error) => []);
        }

        #region Commands
        #region Move Package
        [RelayCommand]
        private void MovePackageToDirectoryX(MovablePackage parameter) {
            // Todo: Show error message and log
            // if(false == _PackageProvider.PackageExists(movablePackage.PackageName, out _)) {
            //     _ = MessageBox.Show($"The Package {movablePackage.PackageName} does not exist, something messed up during refresh.", "Package does not exist", MessageBoxButton.OK, MessageBoxImage.Error);
            // }


            // Todo: Find the avalonia way of selecting file locations
            // FolderBrowserDialog openFileDlg = new();
            // var result = openFileDlg.ShowDialog();
            // if(string.IsNullOrEmpty(result.ToString())) {
            //     return;
            // }
            //
            // Exception? possibleException = _PackageProvider.MovePackage(movablePackage.PackageName, openFileDlg.SelectedPath).Reduce(null!); // Allow null, because we reduce to nullable Exception
            // // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            // if(possibleException is not null) {
            //     _ = MessageBox.Show(possibleException.Message, caption: "Move Package Error", MessageBoxButton.OK, MessageBoxImage.Error);
            // }
        }
        #endregion

        #region Edit Package
        [RelayCommand]
        private void EditPackage() { }
        #endregion

        #region Remove Package
        [RelayCommand]
        private void RemovePackage(MovablePackage movablePackage) {
            // Todo: Show error if package does not exist
            // if(false == _PackageProvider.PackageExists(movablePackage.PackageName, out _)) {
            //     _ = MessageBox.Show($"The Package {movablePackage.PackageName} does not exist, something messed up during refresh.", "Package does not exist", MessageBoxButton.OK, MessageBoxImage.Error);
            //     return;
            // }

            // Todo: Show error if package can not be deleted
            // Exception? possibleException = _PackageProvider.DeletePackage(movablePackage.PackageName).Reduce(null!);
            // // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            // if(possibleException is not null) {
            //     _ = MessageBox.Show(possibleException.Message, "Package Deletion Error", MessageBoxButton.OK, MessageBoxImage.Error);
            // }
        }
        #endregion

        #region Export Package
        [RelayCommand]
        private void ExportPackageAsZip(MovablePackage movablePackage) {
            // Todo: Show error if package does not exist
            // if(false == _PackageProvider.PackageExists(movablePackage.PackageName, out _)) {
            //     _ = MessageBox.Show($"The Package {movablePackage.PackageName} does not exist, something messed up during refresh.", "Package does not exist", MessageBoxButton.OK, MessageBoxImage.Error);
            //     return;
            // }

            // Todo: Find the avalonia way of selecting file locations
            // string pathToDownloadsFolder = KnownFolderProvider.GetPath(KnownFolderProvider.KnownFolder.Downloads);
            // FolderBrowserDialog openFileDlg = new() { InitialDirectory = pathToDownloadsFolder };
            //
            // var result = openFileDlg.ShowDialog();
            // string pathToExportTo = string.IsNullOrEmpty(result.ToString()) == false
            //                             ? openFileDlg.SelectedPath
            //                             : pathToDownloadsFolder;
            //
            // Exception? possibleException = _PackageProvider.ExportPackage(movablePackage.PackageName, pathToExportTo).Reduce(null!); // Allow null, because we reduce to nullable Exception
            // // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            // if(possibleException is not null) {
            //     _ = MessageBox.Show(possibleException.Message, "Package Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
            // }
        }
        #endregion

        #region Import Package
        [RelayCommand]
        private void ImportPackage() {
            // Todo: Find the avalonia way of selecting file locations
            // OpenFileDialog openFileDlg = new() { InitialDirectory = KnownFolderProvider.GetPath(KnownFolderProvider.KnownFolder.Downloads), Filter = "Zip Archives (*.zip)|*.zip", Multiselect = false };
            // var result = openFileDlg.ShowDialog();
            // if(result.ToString().Length == 0) {
            //     return;
            // }
            //
            // FileInfo? fileInfoToImport = null;
            // if(File.Exists(openFileDlg.FileName)) {
            //     fileInfoToImport = new FileInfo(openFileDlg.FileName);
            // }
            //
            // if(fileInfoToImport is null) {
            //     return;
            // }
            //
            // Exception? possibleException = _PackageProvider.ImportPackage(fileInfoToImport).Reduce(null!);
            // // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            // if(possibleException is not null) {
            //     _ = MessageBox.Show(possibleException.Message, "Package Import Error", MessageBoxButton.OK, MessageBoxImage.Error);
            // }
        }
        #endregion

        #region Refresh
        [RelayCommand]
        private void Refresh() {
            MovablePackages = GetFrontendPackages();
        }
        #endregion
        #endregion Commands

    }
}
