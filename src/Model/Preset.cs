using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace File_Switcher.Model {
  public class Preset : INotifyPropertyChanged {
    public Preset(string displayName, List<string> sourceFiles, string destnationPath) {
      this.DisplayName = displayName;
      this.SourceFiles = sourceFiles;
      this.DestiantionPath = destnationPath;
    }

    #region Properties
    private string _DisplayName = "";
    public string DisplayName {
      get => _DisplayName;
      set { _DisplayName = value; OnPropertyChanged(); }
    }

    private List<string> _SourceFiles = new List<string>();
    public List<string> SourceFiles {
      get => _SourceFiles;
      set { _SourceFiles = value; OnPropertyChanged(); }
    }

    private string _DestiantionPath = "";
    public string DestiantionPath {
      get => _DestiantionPath;
      set { _DestiantionPath = value; OnPropertyChanged(); }
    }

    #endregion Properties

    #region Property Changed

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    #endregion Property Changed
  }
}
