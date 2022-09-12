using Microsoft.EntityFrameworkCore;
using ShoppingList.Items.Api;
using ShoppingList.Items.Data.Database;

var builder = WebApplication.CreateBuilder(args);

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
