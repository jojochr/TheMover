using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TheMover.UI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase {
    public MainWindowViewModel() {
        Greeting = "Welcome to Avalonia!";
        Task.Run(async () => {
                     await Task.Delay(TimeSpan.FromSeconds(3));
                     Dispatcher.UIThread.Post(() => Greeting = $"Hello from Async-Task!");
                 });
    }

    [ObservableProperty]
    private string _Greeting;
}
