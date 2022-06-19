using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Data.Entities
{
    public class Entity<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; } = default!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifedAt { get; set; }
    }
}
