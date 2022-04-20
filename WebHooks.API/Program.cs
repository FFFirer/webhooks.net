using WebHooks.EntityFrameworkCore.Pgsql;
using WebHooks.Service.Extensions;

var builder = WebApplication.CreateBuilder(args);

// ���ݿ������ַ���
var connectionString = builder.Configuration.GetConnectionString("Default");
// ��ӿ�����
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocument();
// ע���������
builder.Services.AddWebHooksBasicService();
// ע�����ݿ����������
builder.Services.AddPgsqlDataContext(connectionString);

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
