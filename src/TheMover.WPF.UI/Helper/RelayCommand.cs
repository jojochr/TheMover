using System.Windows.Input;

namespace TheMover.WPF.UI.Helper {
    public class RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null) : ICommand {
        public bool CanExecute(object? parameter) => canExecute is null || canExecute(parameter);
        public void Execute(object? parameter) => execute(parameter);

        public event EventHandler? CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
