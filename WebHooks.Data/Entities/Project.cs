using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Data.Entities
{
    public class Project : Entity<Guid>
    {
        public Guid? PlatformId { get; set; }


        public string? Name { get; set; }

        public string? Description { get; set; }

        /// <summary>
        /// 项目地址
        /// </summary>
        public string? Url { get; set; }
    }
}
