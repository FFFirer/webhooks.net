using NLog.Extensions.Logging;
using WebHooks.Core.Gitee.Services;
using WebHooks.Models.Gitee.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Logging
builder.Services.AddLogging(option =>
{
    option.ClearProviders();
    option.AddNLog(NLog.LogManager.Configuration);
});
    
var configManager = new ConfigurationManager();

var repoConfigDict = new Dictionary<string, GiteeWebHookOption>();

configManager.GetSection(GiteeWebHookOption._platform).Bind(repoConfigDict);

foreach (var key in repoConfigDict.Keys)
{
    builder.Services.Configure<GiteeWebHookOption>(key, (option) => { option = repoConfigDict[key]; });
}

builder.Services.AddSwaggerDocument((settings) =>
{
    settings.Version = "v1.0.0";
    settings.Title = "WebHooks for Gitee API";
});

builder.Services.AddScoped<IGiteeService, GiteeService>();
builder.Services.AddScoped<ICommandService, CommandService>();

builder.Host.UseSystemd();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

if (!app.Environment.IsProduction())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
}

app.UseRouting();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
});

app.UseAuthorization();

app.MapControllers();

app.Run();
