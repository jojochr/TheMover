using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TheMover.UI.Models {
    internal partial class MovablePackage(string packageName, string packagePathRelativeToRepo, IEnumerable<PackageContainedFile> files) : ObservableObject {
        [ObservableProperty]private string _PackageName = packageName;
        [ObservableProperty]private string _PackagePath = packagePathRelativeToRepo;
        [ObservableProperty]private ObservableCollection<PackageContainedFile> _ContainedFiles = new(files);
    }
}
