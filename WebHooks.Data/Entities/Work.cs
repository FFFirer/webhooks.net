    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Data.Entities
{
    /// <summary>
    /// 工作
    /// </summary>
    public class Work : Entity<Guid>
    {
        public Guid? ProjectId { get; set; }

        public Guid? GroupId { get; set; }

        public string? DisplayName { get; set; }

        /// <summary>
        /// 工作目录
        /// </summary>
        public string? WorkingDirectory { get; set; }
    }
}
