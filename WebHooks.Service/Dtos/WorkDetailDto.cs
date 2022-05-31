using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Data.Entities;
using WebHooks.Data.Gitee;

namespace WebHooks.Service.Dtos
{
    public class WorkDetailDto
    {
        public WorkDto? Work { get; set; }
        public List<BuildScript>? Scripts { get; set; }
        public GiteeWebhookConfig? Config { get; set; }
    }
}
