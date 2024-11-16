using Avalonia.Controls;
using TheMover.UI.ViewModels;

namespace TheMover.UI.Views;

public partial class MainWindow : Window {
    public MainWindow() {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}
