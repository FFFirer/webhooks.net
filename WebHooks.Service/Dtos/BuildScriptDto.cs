using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Service.Dtos
{
    public class BuildScriptDto : Dto<int>
    {
        public Guid WorkId { get; set; }
        public int SortNumber { get; set; }
        public List<string> Scripts { get; set; } = new List<string>();
    }
}
