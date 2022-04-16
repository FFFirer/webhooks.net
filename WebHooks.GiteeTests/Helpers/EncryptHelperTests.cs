using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebHooks.Gitee.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using WebHooks.Core.Gitee.Helpers;

namespace WebHooks.Gitee.Helpers.Tests
{
    [TestClass()]
    public class EncryptHelperTests
    {
        [TestMethod()]
        [DataRow("123","123", "5wlGC2Sp7wlQ+W+zrWo7+4Fgv8USkBdxJvfPz7XAuDg=")]
        public void CheckGiteeSignTest(string timestamp, string secret, string expect)
        {
            var factBytes = GiteeHelper.CalcGiteeSign(timestamp, secret);

            var fact = Convert.ToBase64String(factBytes);

            Logger.LogMessage("expect:\t{0}", expect);
            Logger.LogMessage("fact:\t{0}", fact);

            Assert.AreEqual(expect, fact);
        }
    }
}