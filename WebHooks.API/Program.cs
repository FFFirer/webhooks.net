using WebHooks.API;
using WebHooks.API.Filters;
using WebHooks.API.ResultWrapper;
using WebHooks.EntityFrameworkCore.Pgsql;
using WebHooks.Service.Extensions;

var builder = WebApplication.CreateBuilder(args);

// 数据库连接字符串
var connectionString = builder.Configuration.GetConnectionString("Default");
// 添加控制器
builder.Services.AddControllers(config =>
{
    config.Filters.AddService<ApiExceptionFilter>();
    config.Filters.AddService<ResultWrapperFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocument();
// 注册基础服务
builder.Services.AddWebHooksBasicService();
// 注册数据库访问上下文
builder.Services.AddPgsqlDataContext(connectionString);
builder.Services.AddScoped<ApiExceptionFilter>();
builder.Services.AddScoped<ResultWrapperFilter>();
builder.Services.AddScoped<IResultWrapper, CustomResultWrapper>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
