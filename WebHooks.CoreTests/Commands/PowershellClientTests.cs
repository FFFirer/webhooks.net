﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebHooks.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation.Runspaces;
using System.Runtime.InteropServices;
using WebHooks.CoreTests;
using System.Management.Automation;
using Microsoft.Extensions.Logging;
using WebHooks.Models;
using System.Reflection;

namespace WebHooks.Core.Commands.Tests
{
    [TestClass()]
    public class PowershellClientTests
    {
        [TestMethod()]
        public void PreLoadCommandsTest()
        {
            var initialState = InitialSessionState.CreateDefault();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                initialState.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.RemoteSigned;
            }

            var logger = new TestLogger();

            var client = PowershellClient.Create(logger);

            // 反射调用私有方法
            var clientType = client.GetType();
            var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
            var method = clientType.GetMethod("PreLoadCommands", bindingFlags);

            if(method == null)
            {
                throw new Exception("没有找到方法！");
            }

            var _ = method.Invoke(client, new object[] { initialState });

            var _program = new WebHooksProgram();
            var _host = new WebHooksHost(_program);

            using (var runspace = RunspaceFactory.CreateRunspace(_host, initialState))
            {
                runspace.Open();

                var powershell = PowerShell.Create(runspace);
                
                powershell.AddStatement().AddCommand("Get-GitBranch")
                    .AddParameter("Directory", "./")
                    .AddParameter("Branch", "develop")
                    .AddParameter("RepoUrl", "https://gitee.com/fffirer/WebHooks.NET.git")
                    .AddParameter("Tag", "");

                try
                {
                    var results = powershell.Invoke();

                    if (results != null)
                    {
                        foreach (var result in results)
                        {
                            logger.LogDebug(result.ToString());
                        }
                    }

                    Assert.AreEqual(0, _program.ExitCode);
                }
                catch (AssertFailedException)
                {

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "");

                    Assert.Fail();
                }
            }

           
        }
    
        /// <summary>
        /// 测试明亮调用
        /// </summary>
        [TestMethod]
        public void InvokeClientTest()
        {
            var logger = new TestLogger();
            try
            {
                var initialState = InitialSessionState.CreateDefault();

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    initialState.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.RemoteSigned;
                }

                var client = PowershellClient.Create(logger);

                var addCommands = (PowerShell shell) =>
                {
                    shell.AddStatement().AddCommand("Get-GitBranch")
                    .AddParameter("Directory", "./")
                    .AddParameter("Branch", "develop")
                    .AddParameter("RepoUrl", "https://gitee.com/fffirer/WebHooks.NET.git")
                    .AddParameter("Tag", "");
                };

                var (exitcode, results) = client.InvokeAsync(addCommands).Result;

                Assert.AreEqual(0, exitcode);
            }
            catch (AssertFailedException)
            {

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "测试终止");
                Assert.Fail();
            }
        }
    }
}