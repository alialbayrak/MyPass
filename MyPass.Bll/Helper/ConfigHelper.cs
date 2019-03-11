using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPass.Bll.Helper
{
    public class ConfigHelper
    {
        public static T GetSetting<T>(string key) {

            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T));

        }

    }
}
