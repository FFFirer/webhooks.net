using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using Moq;
using System;
using System.Management.Automation;

namespace WebHooks.Core.Commands.Tests
{
    [TestClass]
    public class WebShellTests
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILoggerFactory _loggerFactory;

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
            PSStyle.Instance.OutputRendering = OutputRendering.PlainText;

            var logger = _loggerFactory.CreateLogger("Test");
            var output = new Mock<IWebShellOutput>();

            output.Setup(a => a.WriteLine(It.IsAny<object>(), It.IsAny<string>()))
                .Callback((object obbj,string msg) =>
                {
                    Logger.LogMessage($"OUTPUT> {msg}");
                }).Verifiable();

            try
            {
                var testWebShellOutput = new TestWebShellOutput();
                var shell = new WebShell(logger, testWebShellOutput);

                shell.Execute("$PSVersionTable.PSVersion");
                shell.Execute("Get-ExperimentalFeature");
                shell.Execute("Enable-ExperimentalFeature -Name \"PSAnsiRendering\"");
                shell.Execute("[ExperimentalFeature]::IsEnabled(\"PSAnsiRendering\")");
                shell.Execute("echo $Env:__SuppressAnsiEscapeSequences");
                shell.Execute("$PSStyle.OutputRendering");
                shell.Execute("ls");
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex.ToString());
                Assert.Fail();
            }
        }
    }

    public class TestWebShellOutput : IWebShellOutput
    {
        public void Clear()
        {
            throw new NotImplementedException();
        }

        public string Get()
        {
            throw new NotImplementedException();
        }

        public void WriteLine(object? sender, string message)
        {
            message = Regex.Replace(message, AnsiColorPattern, "");   // 去除Ansi Escape Codes
            //v = v.TrimEnd('\n');                                    // 去除行尾回车
            Logger.LogMessage(message);
        }

        private readonly string AnsiColorPattern = @"\u001b(.*?)m";
    }
}