using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Xml;
using WebHooks.Data.DbContexts;

namespace WebHooks.EntityFrameworkCore.Pgsql
{
    public class WebHooksPgSqlContext : WebHooksDataContext
    {
        public WebHooksPgSqlContext() : base()
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var projectFolder = Path.Combine(Environment.CurrentDirectory, "WebHooks.EntityFrameworkCore.Pgsql.csproj");

            var connectionString = GetConnectionString(projectFolder);

            optionsBuilder.UseNpgsql(connectionString);
        }

        public static string GetConnectionString(string projectFilePath)
        {
            if (!File.Exists(projectFilePath))
            {
                throw new Exception($"未在[{projectFilePath}]找到项目文件");
            }

            var builder = new ConfigurationBuilder();

            string userSecretId;

            using (var fileStream = File.OpenRead(projectFilePath))
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(fileStream);

                var value = xmlDoc.SelectSingleNode("//UserSecretsId");

                if(value == null)
                {
                    throw new Exception("没有在项目文件中找到用户机密配置节点");
                }

                userSecretId = value.InnerText
                    .Replace("\t", "")
                    .Replace("\r", "")
                    .Replace("\n", "");
            }

            builder.AddUserSecrets(userSecretId);

            var connectionString = builder.Build().GetConnectionString("Default");

            return connectionString;
        }
    }
}