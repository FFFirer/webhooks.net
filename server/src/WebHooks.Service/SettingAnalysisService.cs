using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Data.Entities;
using WebHooks.Service.Attributes.Settings;

namespace WebHooks.Service
{
    public class SettingAnalysisService
    {
        #region 帮助扩展
        /// <summary>
        /// 获取段名称
        /// </summary>
        /// <typeparam name="TSetting"></typeparam>
        /// <returns></returns>
        protected virtual string GetSectionName<TSetting>() where TSetting : class, new()
        {
            var targetType = typeof(TSetting);
            var sectionAttribute = targetType.GetCustomAttribute<SettingSectionAttribute>();

            if (sectionAttribute == null || string.IsNullOrEmpty(sectionAttribute.SectionName))
            {
                return targetType.Name;
            }

            return sectionAttribute.SectionName;
        }

        /// <summary>
        /// 序列化为对象 
        /// </summary>
        /// <typeparam name="TSetting"></typeparam>
        /// <param name="settings"></param>
        /// <returns></returns>
        protected virtual TSetting ConvertTo<TSetting>(IList<Setting> settings) where TSetting : class, new()
        {
            var targetType = typeof(TSetting);
            var properties = targetType.GetProperties();

            var targetObject = new TSetting();

            foreach (var prop in properties)
            {
                var keyNameAttribute = prop.GetCustomAttribute<SettingKeyAttribute>();

                string keyName = prop.Name;

                if (keyNameAttribute != null)
                {
                    keyName = keyNameAttribute.KeyName;
                }

                var setting = settings.FirstOrDefault(x => x.Key.Equals(keyName))?.Value ?? string.Empty;

                if (prop.PropertyType == typeof(string))
                {
                    prop.SetValue(targetObject, setting);
                }
                else
                {
                    prop.SetValue(targetObject, Activator.CreateInstance(prop.PropertyType));
                }
            }

            return targetObject;
        }

        protected virtual IList<Setting> GetDetails<TSetting>(TSetting setting) where TSetting : class, new()
        {
            var targetType = typeof(TSetting);
            var details = new List<Setting>();
            var sectionName = GetSectionName<TSetting>();
            var properties = targetType.GetProperties();
            foreach (var prop in properties)
            {
                var detail = new Setting()
                {
                    Section = sectionName,
                    Key = prop.Name,
                };
                var keyNameAttribute = prop.GetCustomAttribute<SettingKeyAttribute>();

                if (keyNameAttribute != null)
                {
                    detail.Key = keyNameAttribute.KeyName;
                }

                var value = prop.GetValue(setting, null);

                detail.Value = value.ToString();
                details.Add(detail);
            }

            return details;
        }

        /// <summary>
        /// 合并修改(Key&Section)
        /// </summary>
        /// <param name="oldSettings"></param>
        /// <param name="newSettings"></param>
        protected virtual List<Setting>? MergeSettingDetails(IList<Setting> oldSettings, IList<Setting> newSettings)
        {
            var result = new List<Setting>();

            var toUpdate = oldSettings.Join(newSettings
                                            , a => new { a.Key, a.Section }
                                            , b => new { b.Key, b.Section }
                                            , (oldone, newonw) =>
                                            {
                                                oldone.Value = newonw.Value;
                                                oldone.Description = newonw.Description;
                                                return oldone;
                                            });

            result.AddRange(toUpdate.ToList());

            var toAdd = from newone in newSettings
                        join oldone in oldSettings on new { newone.Key, newone.Section } equals new { oldone.Key, oldone.Section } into allSettings
                        from setting in allSettings.DefaultIfEmpty()
                        where setting == null
                        select newone;

            result.AddRange(toAdd.ToList());

            return result;
        }

        protected virtual bool IsBuiltInType(Type type)
        {
            return (type == typeof(object) || Type.GetTypeCode(type) != TypeCode.Object);
        }
        #endregion

    }
}
