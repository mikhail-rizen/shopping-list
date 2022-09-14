using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Elasticsearch;
using ShoppingList.Items.Api;
using ShoppingList.Items.Data.Database;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((b, loggerConfiguration) =>
{
    string environment = b.HostingEnvironment.EnvironmentName;
    loggerConfiguration
        .ReadFrom.Configuration(b.Configuration)
        .Enrich.WithProperty("Environment", environment)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(b.Configuration["Elastic:Url"]))
        {
            AutoRegisterTemplate = true,
            IndexFormat = $"logs-{Assembly.GetExecutingAssembly().GetName().Name!.ToLower().Replace(".", "-")}-{environment.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM-dd}",
            //IndexFormat = "my-logs",
            BatchAction = ElasticOpType.Create
        })
        //.WriteTo.File("logs/log.txt")
        //.WriteTo.Console(new CompactJsonFormatter())
        ;
        
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ItemsContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("pgSql"), options => options.MigrationsAssembly("ShoppingList.Items.Api"));
});

ServiceConfiguration.ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ItemsContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
