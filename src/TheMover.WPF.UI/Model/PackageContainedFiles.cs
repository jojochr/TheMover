using System.Windows.Media;
using TheMover.WPF.UI.Helper;

namespace TheMover.WPF.UI.Model {
    public class PackageContainedFile(Icon icon, string packageRelativePath) : OnPropertyChangedImplementer {
        private Icon _Icon = icon;
        public ImageSource Icon {
            get => _Icon.ToImageSource();
        }

        private string _PackageRelativePath = packageRelativePath;
        public string PackageRelativePath {
            get => _PackageRelativePath;
            set {
                _PackageRelativePath = value;
                OnPropertyChanged();
            }
        }
    }
}
