using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Data.Entities;

namespace WebHooks.Data.AdditionalWork.Git
{
    public class GitConfig : Entity<int>
    {
        /// <summary>
        /// 关联的工作项编号
        /// </summary>
        public Guid WorkId { get; set; }
        public string? AddressType { get; set; }
        public string? RepositoryAddress { get; set; }
        public string? Branch { get; set; }
        public string? Tag { get; set; }

        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
