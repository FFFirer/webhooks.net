using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using Moq;
using System;
using System.Management.Automation;
using System.Reflection;
using System.Text.RegularExpressions;

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

                shell.ExecuteAsync("$PSVersionTable.PSVersion");
                shell.ExecuteAsync("Get-ExperimentalFeature");
                shell.ExecuteAsync("Enable-ExperimentalFeature -Name \"PSAnsiRendering\"");
                shell.ExecuteAsync("[ExperimentalFeature]::IsEnabled(\"PSAnsiRendering\")");
                shell.ExecuteAsync("echo $Env:__SuppressAnsiEscapeSequences");
                shell.ExecuteAsync("$PSStyle.OutputRendering");
                shell.ExecuteAsync("ls");
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
            //message = Regex.Replace(message, AnsiColorPattern, "");   // 去除Ansi Escape Codes
            //v = v.TrimEnd('\n');                                    // 去除行尾回车
            Logger.LogMessage(message);
        }

        private string AnsiColorPattern = @"\u001b(.*?)m";
    }
}