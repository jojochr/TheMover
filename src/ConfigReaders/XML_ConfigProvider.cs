using System.Xml.Linq;

using FluentResults;
using TheMover.Datastructures;

namespace TheMover.ConfigReaders
{
    internal class XML_ConfigProvider : Base_ConfigProvider
    {
        internal XML_ConfigProvider()
        {
            _WriteOptions = new FileStreamOptions
            {
                Mode = FileMode.Truncate,
                Access = FileAccess.Write,
                Share = FileShare.ReadWrite,
                Options = FileOptions.None
            };

            _ReadOptions = new FileStreamOptions
            {
                Mode = FileMode.OpenOrCreate,
                Access = FileAccess.Read,
                Share = FileShare.ReadWrite,
                Options = FileOptions.None
            };
        }

        #region Konstanten für Config Pfad und Inhalt

        private FileStreamOptions _WriteOptions;
        private FileStreamOptions _ReadOptions;

        private const string _ConfigPath = "./Config.xml";

        private const string _ElementName_Preset = "Preset";
        private const string _AttributeName_PresetName = "PresetName";
        private const string _AttributeName_DestinationPath = "DestinationPath";

        private const string _ElementName_SourcePath = "SourcePath";

        #endregion Konstanten für Config Pfad und Inhalt

        internal override Result SaveConfig(List<Preset> presetsToSave)
        {
            var rootElement = new XElement(XName.Get("root"));

            foreach (var preset in presetsToSave)
            {
                var xmlPreset = new XElement(XName.Get(_ElementName_Preset));
                xmlPreset.Add(new XAttribute(XName.Get(_AttributeName_PresetName), preset.PresetName));
                xmlPreset.Add(new XAttribute(XName.Get(_AttributeName_DestinationPath), preset.DestiantionPath));

                foreach (var sourcePath in preset.SourceFiles)
                {
                    xmlPreset.Add(new XElement(XName.Get(_ElementName_SourcePath), sourcePath));
                }

                rootElement.Add(xmlPreset);
            }

            var resultDocument = new XDocument();
            resultDocument.Add(rootElement);

            using (var configWriter = new StreamWriter(_ConfigPath, _WriteOptions))
            {
                resultDocument.Save(configWriter);
            }

            return Result.Ok();
        }

        internal override Result<List<Preset>> ReadConfig()
        {
            if (!File.Exists(_ConfigPath))
            {
                return new List<Preset>();
            }

            // #Todo JCI - Configvalidation should happen here

            XElement[] presetsFromConfig;
            using (var configReader = new FileStream(_ConfigPath, _ReadOptions))
            {
                presetsFromConfig = XDocument.Load(configReader).Descendants(XName.Get(_ElementName_Preset)).ToArray();
            }

            var result = new List<Preset>();
            int errorCount = 0;
            for (int i = 0; i < presetsFromConfig.Length; i++)
            {
                bool error = false;

                string displayName;
                {
                    var presetNameElement = presetsFromConfig[i].Attribute(XName.Get(_AttributeName_PresetName));
                    if (null != presetNameElement)
                        displayName = presetNameElement.Value;
                    else displayName = "Unnamed Preset";
                }

                string destinationPath;
                {
                    var destinationElement = presetsFromConfig[i].Attribute(XName.Get(_AttributeName_DestinationPath));
                    if (null != destinationElement)
                        destinationPath = destinationElement.Value;
                    else
                    {
                        destinationPath = "";
                        error = true;
                    }
                }

                var sourceFiles = new List<string>();
                {
                    foreach (var sourceFileElement in presetsFromConfig[i].Descendants(XName.Get(_ElementName_SourcePath)))
                    {
                        if (null == sourceFileElement)
                        {
                            continue;
                        }

                        string sourcePath = sourceFileElement.Value;
                        if (sourcePath == string.Empty)
                        {
                            continue;
                        }

                        sourceFiles.Add(sourcePath);

                    }

                    if (sourceFiles.Count < 1)
                    {
                        error = true;
                    }
                }

                if (!error)
                {
                    result.Add(new Preset(displayName, sourceFiles, destinationPath));
                }
                else
                {
                    errorCount++;
                }
            }

            if (errorCount > 0)
            {
                string backupFileName = "FaultyConfig" + DateTime.Now.ToString("-yyyy-MM-dd_HH-mm-ss") + ".xml";
                var backupDirectoryInfo = Directory.CreateDirectory("./Backup");

                string backupFileFullName = string.Concat(backupDirectoryInfo.FullName, "/", backupFileName);

                File.Copy(_ConfigPath, backupFileFullName);
                Console.WriteLine($"{errorCount} errors detected while reading the config.\r\nBackup file of faulty config created under: {backupFileFullName}\r\nFaulty conig elements will be removed automatically");
            }
            return result;
        }

        #region Custom Exceptions

        internal class ConfigReadingException : Exception
        {

            public ConfigReadingException()
            {
            }

            public ConfigReadingException(string message)
                : base(message)
            {
            }

            public ConfigReadingException(string message, Exception inner)
                : base(message, inner)
            {
            }
        }

        #endregion Custom Exceptions

    }
}
