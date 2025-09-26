using Finsight.Core;
using Finsight.Core.Dao.EF;
using Finsight.WebApi.Extensions;
using Microsoft.EntityFrameworkCore;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Host.UseNLog();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Строка подключения 'DefaultConnection' не настроена.");

builder.Services
    .AddCore();

builder.Services.AddJwtAuthentication(builder.Configuration, builder.Environment);

builder.Services.AddWebApiServices(builder.Configuration);

builder.Services.AddDbContext<EfContext>(c => c.UseSqlServer(connectionString));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<EfContext>();
    await dbContext.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandling();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
