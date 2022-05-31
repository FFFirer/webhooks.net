using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Data.Entities
{
    /// <summary>
    /// 构建脚本
    /// </summary>
    public class BuildScript : Entity<int>
    {
        public Guid WorkId { get; set; }
        public int SortNumber { get; set; }
        public List<string> Scripts { get; set; } = new List<string>();
    }
}
