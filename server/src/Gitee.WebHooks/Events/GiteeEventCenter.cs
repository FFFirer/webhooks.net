using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitee.WebHooks.Events
{
    public class GiteeEventCenter
    {
        private GiteeEventCenter() { }

        public static GiteeEventCenter Instance { get; } = new GiteeEventCenter();


    }
}
