using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Newtonsoft.Json;

using TheMover.Primitives;

namespace TheMover.ConfigProviders {
    internal class JSON_ConfigProvider : Base_ConfigProvider {
        #region For Config-File reading
        private const string CONFIG_PATH = "./config.json";

        private StreamReader GetFileReader() {
            return new StreamReader(path: CONFIG_PATH, encoding: Encoding.UTF8);
        }

        private StreamWriter GetFileWriter() {
            // Deletes the existing file, create if the file does not exist and write UTF8
            return new StreamWriter(path: CONFIG_PATH, append: false, encoding: Encoding.UTF8);
        }
        #endregion For Config-File reading


        internal override Result<List<Preset>, ConfigReadingException> ReadConfig() {
            string fileContents;
            using(var reader = GetFileReader()) {
                fileContents = reader.ReadToEnd();
            }

            List<Preset> result;
            try {
                result = JsonConvert.DeserializeObject<List<Preset>>(fileContents);
            } catch(Exception e) {
                return new ConfigReadingException("Failed to deserialize JSON-File", e);
            }

            return result ?? new List<Preset>();
        }

        internal override Option<ConfigWritingException> SaveConfig(List<Preset> presetsToSave) {
            string serializedList = JsonConvert.SerializeObject(presetsToSave);

            try {
                using(var writer = GetFileWriter()) {
                    writer.Write(serializedList);
                }
            } catch(Exception e) {
                return new ConfigWritingException("Failed to save the already deserialized JSON-Object to file", e);
            }

            return Option<ConfigWritingException>.None;
        }
    }
}
