﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Models.Gitee.Options
{
    public class GiteeOptions
    {
        public Dictionary<string, GiteeWebHookOption> PushWebHookOptions { get; set; } = new Dictionary<string, GiteeWebHookOption>();
    }
}
