using File_Switcher.Model;

/// <summary>
/// Checklist for stuff to implement on every Config Reader: (This may get added to the interface at some point)<br></br>
/// - Pop ups if things go wrong<br></br>
/// - Pop up with progress bar and notification if successful<br></br>
/// <br></br>
/// These things are not part of the interface yet, because the only purpose of the interface at this point ist for the LandingViewModel to call stuff<br></br>
/// I dont want the ViewModel to also worry about showing message boxes.<br></br>
/// The neccessity of error messages may be File-Format specific, and is up to the implementer to get right, with no enforcement on an interface level for now.
/// </summary>
public interface IConfigReader {
  public abstract List<Preset> ReadConfig();
  public abstract void SaveConfig(List<Preset> presetsToSave);
}