using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFusionLabHaute.Common
{
    public class Common
    {
        public enum Language
        {
            ENG = 1,
            ESP = 2
        }

        public enum LanguageFiles
        {
            ENGLanguageResource = 1,
            ESPELanguageResource = 2
        }

        public enum ApplicationMode
        {
            PRODUCTION = 1,
            CUSTOMER = 2
        }
    }
}