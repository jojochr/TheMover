using TheMover.Datastructures;
using static TheMover.ConfigReaders.XML_ConfigProvider;

internal abstract class Base_ConfigProvider {
    /// <summary>
    /// When implemented this should read a configuration
    /// </summary>
    /// <returns></returns>
   internal abstract Result<List<Preset>, ConfigReadingException> ReadConfig();

    /// <summary>
    /// This saves the passed configurations.<br></br>
    /// </summary>
    /// <param name="presetsToSave"></param>
    /// <returns></returns>
    internal abstract Option<ConfigWritingException> SaveConfig(List<Preset> presetsToSave);
}