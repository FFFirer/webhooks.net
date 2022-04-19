using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebHooks.EntityFrameworkCore.Pgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.EntityFrameworkCore.Pgsql.Tests
{
    [TestClass]
    public class WebHooksPgSqlContextTests
    {
        [TestMethod()]
        public void GetConnectionStringTest()
        {
            var context = new WebHooksPgSqlContext();
            var projectFilePath = @"D:\Playground\repos\WebHooks.NET\WebHooks.EntityFrameworkCore.Pgsql\WebHooks.EntityFrameworkCore.Pgsql.csproj";
            
            var connectString = context.GetConnectionString(projectFilePath);

            if (string.IsNullOrEmpty(connectString))
            {
                Assert.Fail();
            }
        }
    }
}