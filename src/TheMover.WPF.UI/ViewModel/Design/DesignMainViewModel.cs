namespace TheMover.WPF.UI.ViewModel.Design {
    public class DesignMainWindowViewModel : MainWindowViewModel {
        public DesignMainWindowViewModel() {
            PackagesListViewModel = new DesignPackagesListViewModel();
        }
    }
}
