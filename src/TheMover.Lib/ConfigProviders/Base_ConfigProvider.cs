using System;
using System.Collections.Generic;

using TheMover.Primitives;

namespace TheMover.ConfigProviders {
    abstract class Base_ConfigProvider {
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

    #region Custom Exceptions
    internal class ConfigReadingException : Exception {
        public ConfigReadingException() {
        }

        public ConfigReadingException(string message)
            : base(message) {
        }

        public ConfigReadingException(string message, Exception inner)
            : base(message, inner) {
        }
    }
    internal class ConfigWritingException : Exception {
        public ConfigWritingException() {
        }
        public ConfigWritingException(string message)
            : base(message) {
        }
        public ConfigWritingException(string message, Exception inner)
            : base(message, inner) {
        }
    }
    #endregion Custom Exceptions
}

