using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TheMover.ConfigReaders;
using TheMover.Datastructures;

namespace TheMover.Lib.ConfigProviders {
    internal class JSON_ConfigProvider : Base_ConfigProvider {
        internal override Result<List<Preset>, ConfigReadingException> ReadConfig() {

        }
        internal override Option<ConfigWritingException> SaveConfig(List<Preset> presetsToSave) => throw new NotImplementedException();

    }
}
