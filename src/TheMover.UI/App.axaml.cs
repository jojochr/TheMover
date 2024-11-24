using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using TheMover.UI.Services;
using TheMover.UI.ViewModels;
using TheMover.UI.ViewModels.Design;
using TheMover.UI.Views;

namespace TheMover.UI;

public partial class App : Application {
    public override void Initialize() {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted() {
        var serviceProvider = DI_Setup.GetServiceProvider();

        MainWindowViewModel vm = Design.IsDesignMode ? new DesignMainWindowViewModel() : serviceProvider.GetRequiredService<MainWindowViewModel>();
        MainView mainWindow = new() { DataContext = vm, };
        RegisterMainWindow(mainWindow);

        // Call base version of this afterward
        base.OnFrameworkInitializationCompleted();
    }

    private void RegisterMainWindow(MainView mainWindow) {
        if(ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(index: 0);
            desktop.MainWindow = mainWindow;
        }
    }
}
