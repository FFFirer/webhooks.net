using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebHooks.Gitee.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Gitee.Helpers.Tests
{
    [TestClass()]
    public class EncryptHelperTests
    {
        [TestMethod()]
        [DataRow("","","")]
        public void CheckGiteeSignTest(string timestamp, string secret, string expect)
        {
            var fact = GiteeHelper.CheckGiteeSign(timestamp, secret);

            Assert.AreEqual(expect, fact);
        }
    }
}