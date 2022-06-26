using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Service.Dtos
{
    public class GroupDto : Dto<Guid>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
