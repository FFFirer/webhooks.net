using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Service.Dtos
{
    public class WorkDto : Dto<Guid>
    {
        public string? DisplayName { get; set; }
    }
}
