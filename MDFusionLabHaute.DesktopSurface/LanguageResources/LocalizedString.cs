using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Extensions;

namespace MDFusionLabHaute.DesktopSurface.LanguageResources
{
    public class LocalizedString
    {

        #region Declaration
        public static LocalizedString Instance { get; } = new LocalizedString();
        public string _selectedResource = "ENGLanguageResource";

        #endregion

        private LocalizedString()
        {

        }

        #region Methods

        //public void setCulture(string cultureCode)
        //{
        //    var newCulture = new CultureInfo(cultureCode);
        //    LocalizeDictionary.Instance.Culture = newCulture;
        //}


        //public string this[string key]
        //{
        //    get
        //    {
        //        var result = LocalizeDictionary.Instance.GetLocalizedObject(Assembly.GetCallingAssembly().GetName().Name, "LanguageResources", key, LocalizeDictionary.Instance.Culture);
        //        return result as string;
        //    }
        //}

        public void setResource(string resource)
        {
            _selectedResource = resource;
        }

        public static T GetLocalizedValue<T>(string key,string selectedResource)
        {
            var result = LocExtension.GetLocalizedValue<T>(Assembly.GetCallingAssembly().GetName().Name + ":" + selectedResource + ":" + key);
            return result;
        }
        #endregion
    }
}