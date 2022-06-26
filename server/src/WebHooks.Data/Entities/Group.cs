using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Data.Entities
{
    public class Group : Entity<Guid>
    {
        public string? Name { get; set; }
        public string? Description { get; set; } 
    }
}
