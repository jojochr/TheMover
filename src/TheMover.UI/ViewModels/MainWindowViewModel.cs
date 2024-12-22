using CommunityToolkit.Mvvm.ComponentModel;
using TheMover.Domain.Logging;

namespace TheMover.UI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase {
#pragma warning disable CS8618, CS9264
    /// <summary>This is just used for DesignData, and it initializes everything with null.</summary>
    public MainWindowViewModel() { }
#pragma warning restore CS8618, CS9264

    public MainWindowViewModel(PackageOperationsViewModel operationsViewModel) {
        _PackageOperationsViewModel = operationsViewModel;
    }

    [ObservableProperty]
    private PackageOperationsViewModel _PackageOperationsViewModel;
}
