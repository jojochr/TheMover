namespace TheMover.UI.ViewModels.Design {
    public partial class DesignMainWindowViewModel : MainWindowViewModel {
        public DesignMainWindowViewModel() {
            PackageOperationsViewModel = new DesignPackageOperationsViewModel();
        }
    }
}
