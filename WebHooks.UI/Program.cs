using WebHooks.EntityFrameworkCore.Pgsql;
using WebHooks.Service.Extensions;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");

// Add services to the container.
builder.Services.AddRazorPages();
// 添加服务注册
builder.Services.AddWebHooksBasicService();
// 添加数据库访问注册
builder.Services.AddPgsqlDataContext(connectionString);

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

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
