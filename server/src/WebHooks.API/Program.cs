using WebHooks.API;
using WebHooks.API.Filters;
using WebHooks.API.ResultWrapper;
using WebHooks.Data.Extensions;
using WebHooks.Service;
using WebHooks.Service.Extensions;
using WebHooks.Service.Modules;

var DefaultCorsPolicyName = "default";
var builder = WebApplication.CreateBuilder(args);

// 数据库连接字符串
var connectionString = builder.Configuration.GetConnectionString("Default");

// 添加API控制器
builder.Services.AddControllers(config =>
{
    config.Filters.AddService<ApiExceptionFilter>();
    config.Filters.AddService<ResultWrapperFilter>();
});

builder.Services.AddEndpointsApiExplorer();

// NSwag
builder.Services.AddSwaggerDocument();

// 注册基础服务
builder.Services.AddWebHooksBasicService();

// 注册数据库访问上下文
builder.Services.AddPgsqlDataContext(connectionString);
builder.Services.AddScoped<ApiExceptionFilter>();
builder.Services.AddScoped<ResultWrapperFilter>();
builder.Services.AddScoped<IResultWrapper, CustomResultWrapper>();

builder.Services.InstallModule<WorkRunnerModule>();
builder.Services.InstallModule<ExternalRunnerModule>();   // 安装扩展配置模块

// 跨域配置
var allowOrigins = new List<string>();
builder.Configuration.GetSection("AllowOrigins").Bind(allowOrigins);
builder.Services.AddCors(setup =>
{
    setup.AddPolicy(DefaultCorsPolicyName, config =>
    {
        config.WithOrigins(allowOrigins.ToArray())
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
}

app.UseHttpsRedirection();

app.UseCors(DefaultCorsPolicyName);

app.UseAuthorization();

app.MapControllers();

app.Run();
