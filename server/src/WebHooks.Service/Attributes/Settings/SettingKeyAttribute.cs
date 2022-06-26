using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Service.Attributes.Settings
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class SettingKeyAttribute : Attribute
    {
        public string KeyName { get; set; }

        public SettingKeyAttribute(string keyName)
        {
            this.KeyName = keyName;
        }
    }
}
