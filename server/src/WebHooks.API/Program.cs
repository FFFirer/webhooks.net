using WebHooks.API;
using WebHooks.API.Filters;
using WebHooks.API.ResultWrapper;
using WebHooks.Data.Extensions;
using WebHooks.Service;
using WebHooks.Service.Extensions;
using WebHooks.Service.Modules;

var DefaultCorsPolicyName = "default";
var builder = WebApplication.CreateBuilder(args);

// ���ݿ������ַ���
var connectionString = builder.Configuration.GetConnectionString("Default");

// ���API������
builder.Services.AddControllers(config =>
{
    config.Filters.AddService<ApiExceptionFilter>();
    config.Filters.AddService<ResultWrapperFilter>();
});

builder.Services.AddEndpointsApiExplorer();

// NSwag
builder.Services.AddSwaggerDocument();

// ע���������
builder.Services.AddWebHooksBasicService();

// ע�����ݿ����������
builder.Services.AddPgsqlDataContext(connectionString);
builder.Services.AddScoped<ApiExceptionFilter>();
builder.Services.AddScoped<ResultWrapperFilter>();
builder.Services.AddScoped<IResultWrapper, CustomResultWrapper>();

builder.Services.InstallModule<WorkRunnerModule>();
builder.Services.InstallModule<ExternalRunnerModule>();   // ��װ��չ����ģ��

// ��������
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
