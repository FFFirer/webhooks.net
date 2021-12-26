using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebHooks.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

namespace WebHooks.Core.Commands.Tests
{
    [TestClass()]
    public class WebShellTests
    {
        private IServiceProvider _serviceProvider;
        private ILoggerFactory _loggerFactory;

        public WebShellTests()
        {
            _serviceProvider = new ServiceCollection()
                .AddLogging().BuildServiceProvider();

            _loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
        }

        /// <summary>
        /// 测试单条命令的执行
        /// </summary>
        [TestMethod()]
        public void ExecuteTest()
        {
            var logger = _loggerFactory.CreateLogger("Test");
            var output = new Mock<IWebShellOutput>();

            output.Setup(a => a.WriteLine(It.IsAny<object>(), It.IsAny<string>()))
                .Callback((object obbj,string msg) =>
                {
                    Logger.LogMessage($"OUTPUT> {msg}");
                }).Verifiable();

            try
            {
                var shell = new WebShell(logger, output.Object);

                shell.Execute("ls");
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex.ToString());
                Assert.Fail();
            }
        }
    }
}