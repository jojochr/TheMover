using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using TheMover.Primitives;

namespace TheMover.ConfigProviders {
    internal class XML_ConfigProvider : Base_ConfigProvider {
        #region For Config-File reading
        private const string CONFIG_PATH = "./config.xml";
        private const string BACKUP_PATH = "./backup";

        private StreamReader GetFileReader() {
            return new StreamReader(path: CONFIG_PATH, encoding: Encoding.UTF8);
        }

        private StreamWriter GetFileWriter() {
            // Deletes the existing file, create if the file does not exist and write UTF8
            return new StreamWriter(path: CONFIG_PATH, append: false, encoding: Encoding.UTF8);
        }

        private const string _ElementName_Preset = "Preset";
        private const string _AttributeName_PresetName = "PresetName";
        private const string _AttributeName_DestinationPath = "DestinationPath";

        private const string _ElementName_SourcePath = "SourcePath";

        #endregion For Config-File reading


        internal override Option<ConfigWritingException> SaveConfig(List<Preset> presetsToSave) {
            var rootElement = new XElement(XName.Get("root"));

            foreach(var preset in presetsToSave) {
                var xmlPreset = new XElement(XName.Get(_ElementName_Preset));
                xmlPreset.Add(new XAttribute(XName.Get(_AttributeName_PresetName), preset.PresetName));
                xmlPreset.Add(new XAttribute(XName.Get(_AttributeName_DestinationPath), preset.DestiantionPath));

                foreach(var sourcePath in preset.SourceFiles) {
                    xmlPreset.Add(new XElement(XName.Get(_ElementName_SourcePath), sourcePath));
                }

                rootElement.Add(xmlPreset);
            }

            var resultDocument = new XDocument();
            resultDocument.Add(rootElement);

            using(var configWriter = GetFileWriter()) {
                resultDocument.Save(configWriter);
            }

            return Option<ConfigWritingException>.None;
        }

        internal override Result<List<Preset>, ConfigReadingException> ReadConfig() {
            if(!File.Exists(CONFIG_PATH)) {
                return new List<Preset>();
            }

            XElement[] presetsFromConfig;
            using(var configReader = GetFileReader()) {
                presetsFromConfig = XDocument.Load(configReader).Descendants(XName.Get(_ElementName_Preset)).ToArray();
            }

            var result = ValidatePresets(presetsFromConfig);

            // Create a backup before returning
            BackupOldPresetFile();

            return result;
        }

        /// <summary>
        /// Runs a few checks and deletes every <seealso cref="Preset"/> that does not pass
        /// </summary>
        /// <returns>A list of validated Presets</returns>
        public List<Preset> ValidatePresets(XElement[] presetsToValidate) {
            var result = new List<Preset>();

            foreach(var presetXElement in presetsToValidate) {
                bool error = false;

                if(!ParseAndValidate_PresetName_FromXMLElement(presetXElement).IsSome(out var displayName)) {
                    displayName = "Unnamed Preset";
                }

                if(!ParseAndValidate_DestinationPath_FromXMLElement(presetXElement).IsSome(out var destinationPath)) {
                    destinationPath = "";
                    error = true;
                }

                if(!ParseAndValidate_SourceFiles_FromXMLElement(presetXElement).IsSome(out var sourceFiles)) {
                    sourceFiles = new List<string>();
                    error = true;
                }

                // Only add the presets if no fatal errors occur.
                if(!error) {
                    result.Add(new Preset(displayName, sourceFiles, destinationPath));
                }
            }

            return result;
        }

        #region XML Validation
        private Option<string> ParseAndValidate_PresetName_FromXMLElement(XElement xmlElement) {
            var presetNameAttribute = xmlElement.Attribute(XName.Get(_AttributeName_PresetName));

            return presetNameAttribute?.Value ?? Option<string>.None;
        }
        private Option<string> ParseAndValidate_DestinationPath_FromXMLElement(XElement xmlElement) {
            var destinationAttribute = xmlElement.Attribute(XName.Get(_AttributeName_DestinationPath));

            return destinationAttribute?.Value ?? Option<string>.None;
        }
        private Option<List<string>> ParseAndValidate_SourceFiles_FromXMLElement(XElement xmlElement) {
            var sourcePaths = new List<string>();

            foreach(var sourceFileElement in xmlElement.Descendants(XName.Get(_ElementName_SourcePath))) {
                if(null == sourceFileElement) {
                    continue;
                }

                string sourcePath = sourceFileElement.Value;
                if(sourcePath == string.Empty) {
                    continue;
                }

                sourcePaths.Add(sourcePath);
            }

            if(sourcePaths.Count < 1) {
                return Option<List<string>>.None;
            }

            return sourcePaths;
        }
        #endregion XML Validation

        /// <summary>
        /// Can be used to create a backup of the Configurationfile<br></br>
        /// Throws an Exception if a Backup is not possible for whateverreason<br></br>
        /// -> I dont like, that i throw here. I will find a structural solution soon.
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void BackupOldPresetFile() {
            string backupFileName = "FaultyConfig" + DateTime.Now.ToString("-yyyy-MM-dd_HH-mm-ss") + ".xml";
            var backupDirectoryInfo = Directory.CreateDirectory(BACKUP_PATH);

            string backupFileFullName = string.Concat(backupDirectoryInfo.FullName, "/", backupFileName);

            File.Copy(CONFIG_PATH, backupFileFullName);
        }
    }
}
