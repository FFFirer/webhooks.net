using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Data.Repositories.Interfaces;
using WebHooks.Service.Dtos;
using WebHooks.Service.Interfaces;

namespace WebHooks.Service
{
    public class SettingService : SettingAnalysisService, ISettingService
    {
        private readonly ISettingRepository _settings;

        public SettingService(ISettingRepository settings)
        {
            _settings = settings;
        }

        public async Task<BasicSetting> GetBasicSettingAsync()
        {
            return await GetAsync<BasicSetting>();
        }

        public async Task SaveBasicSettingAsync(BasicSetting basicSetting)
        {
            await SaveAsync(basicSetting);
        }


        /// <summary>
        /// 获取指定设置的值
        /// </summary>
        /// <typeparam name="TSetting"></typeparam>
        /// <returns></returns>
        internal async Task<TSetting> GetAsync<TSetting>() where TSetting : class, new()
        {
            var sectionName = GetSectionName<TSetting>();
            var settings = await _settings.GetOneAsync(sectionName);
            return ConvertTo<TSetting>(settings);
        }

        public async Task SaveAsync<TSetting>(TSetting setting) where TSetting : class, new()
        {
            var sectionName = GetSectionName<TSetting>();
            var oldSettingDetails = await _settings.GetOneAsync(sectionName);
            var newSettingDetails = GetDetails(setting);
            var currentSettings = MergeSettingDetails(oldSettingDetails, newSettingDetails);

            if(currentSettings == null)
            {
                return;
            }

            var effected = await _settings.SaveListAsync(currentSettings);
        }
    }
}
