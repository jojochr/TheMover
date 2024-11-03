using System.Collections.ObjectModel;
using BaseMovablePackage = TheMover.Domain.Model.MovablePackage;
using TheMover.UI.Helper;
using TheMover.Infrastructure.Provider;
using TheMover.UI.Model;

namespace TheMover.UI.ViewModel {
    public class PackagesListViewModel : OnPropertyChangedImplementer {
        #region ctor
        public PackagesListViewModel() {
            _PackageProvider = PackageProvider.GetInstance();
            MovablePackages = GetFrontendPackages();
        }
        #endregion

        #region prop
        private readonly PackageProvider _PackageProvider;

        private MovablePackage? _SelectedPackage;
        public MovablePackage? SelectedPackage {
            get => _SelectedPackage;
            set {
                _SelectedPackage = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<MovablePackage> _MovablePackages = null!;
        public ObservableCollection<MovablePackage> MovablePackages {
            get => _MovablePackages;
            protected set {
                _MovablePackages = value;

                lock(_MovablePackages) {
                    if(_MovablePackages.Count == 0) {
                        SelectedPackage = null;
                    }

                    if(_MovablePackages.Count > 0 && SelectedPackage is null) {
                        SelectedPackage = _MovablePackages.First();
                    }
                }

                OnPropertyChanged();
            }
        }
        #endregion

        private ObservableCollection<MovablePackage> GetFrontendPackages() {
            List<BaseMovablePackage> packages = _PackageProvider.GetMovablePackages().Match(x => x, (_) => new());
            return new ObservableCollection<MovablePackage>(packages.Select(package => new MovablePackage(package)));
        }

        public void Refresh() {
            MovablePackages = GetFrontendPackages();
        }
    }
}
