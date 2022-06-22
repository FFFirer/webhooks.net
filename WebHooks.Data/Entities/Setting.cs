using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Data.Entities
{
    public class Setting : Entity<Guid>
    {
        public virtual string Key { get; set; } = default!;

        public virtual string? Value { get; set; }

        public virtual string? Description { get; set; }

        public virtual string? Section { get; set; }

        public virtual string? SubSection { get; set; }
    }
}
