using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Data.Gitee
{
    public enum GiteeWebHookAuthentication
    {
        /// <summary>
        /// 密码
        /// </summary>
        Srcret = 1,

        /// <summary>
        /// 签名密钥
        /// </summary>
        SignatureKey = 2
    }
}
