using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Service.Dtos;

namespace WebHooks.Service.Git.Dtos
{
    public class GitConfigDto : Dto<int>
    {
        public Guid WorkId { get; set; }
        public string? AddressType { get; set; }
        public string? RepositoryAddress { get; set; }
        public string? Branch { get; set; }
        public string? Tag { get; set; }
    }
}
