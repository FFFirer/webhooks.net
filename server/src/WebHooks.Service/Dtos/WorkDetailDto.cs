using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Data.Entities;
using WebHooks.Data.Gitee;
using WebHooks.Service.Gitee.Dtos;

namespace WebHooks.Service.Dtos
{
    public class WorkDetailDto
    {
        public WorkDto? Work { get; set; }
        public BuildScript? Script { get; set; }
        public GiteeWebHookConfigDto? Config { get; set; }
    }
}
