using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Service.Dtos;

namespace WebHooks.Service.Interfaces
{
    internal interface IPlatformService
    {
        Task<PlatformDto> ListAsync();


    }
}
