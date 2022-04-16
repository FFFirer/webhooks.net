using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Data.Entities
{
    public class Platform : Entity<Guid>
    {
        public string? DisplayName { get; set; }
        public string? Key { get; set; }
        public string? Url { get; set; }
    }
}
