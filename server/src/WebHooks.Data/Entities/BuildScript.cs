using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public List<string> Script { get; set; } = new List<string>();

        [NotMapped]
        private string? _script { get; set; }

        public override string ToString()
        {
            if (_script != null) { return _script; }

            if (Script == null)
            {
                _script = string.Empty;
                return _script;
            }

            var scriptBuilder = new StringBuilder();
            foreach (var script in Script)
            {
                scriptBuilder.AppendLine(script);
            }
            _script = scriptBuilder.ToString();
            return _script;
        }
    }
}
