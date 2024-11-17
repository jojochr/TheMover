using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TheMover.UI.Models {
    public partial class PackageContainedFile(Bitmap icon, string packageRelativePath) : ObservableObject {
        [ObservableProperty]private Bitmap _Icon = icon;
        [ObservableProperty]private string _PackageRelativePath = packageRelativePath;
    }
}
