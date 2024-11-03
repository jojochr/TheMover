using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TheMover.UI.Helper {
    public class OnPropertyChangedImplementer : INotifyPropertyChanged {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null!) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
