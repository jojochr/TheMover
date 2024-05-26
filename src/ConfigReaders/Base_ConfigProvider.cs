using FluentResults;
using TheMover.Datastructures;

internal abstract class Base_ConfigProvider {
    /// <summary>
    /// When implemented this should read a configuration
    /// </summary>
    /// <returns></returns>
    internal abstract Result<List<Preset>> ReadConfig();

    /// <summary>
    /// This saves the passed configurations.<br></br>
    /// </summary>
    /// <param name="presetsToSave"></param>
    /// <returns>true if successful</returns>
    internal abstract Result SaveConfig(List<Preset> presetsToSave);
}