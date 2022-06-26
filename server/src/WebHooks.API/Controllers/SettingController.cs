using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebHooks.Service.Dtos;
using WebHooks.Service.Interfaces;

namespace WebHooks.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly ISettingService _settings;

        public SettingController(ISettingService settings)
        {
            _settings = settings;
        }   

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public async Task<BasicSetting> GetBasicSetting()
        {
            return await _settings.GetBasicSettingAsync();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="basicSetting"></param>
        /// <returns></returns>
        public async Task SaveBasicSetting(BasicSetting basicSetting)
        {
            await _settings.SaveBasicSettingAsync(basicSetting);
        }

    }
}
