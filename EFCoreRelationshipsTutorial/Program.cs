global using EFCoreRelationshipsTutorial.Data;
global using Microsoft.EntityFrameworkCore;
using EFCoreRelationshipsTutorial.Helpers;

var builder = WebApplication.CreateBuilder(args);
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
//var connDocker = builder.Configuration.GetConnectionString("DockerConnection");
// Add services to the container.

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(conn);
});

builder.Services.AddScoped<IMapper, Mapper>();

//builder.Services.AddDbContext<DataContext>(options => { options.UseInMemoryDatabase("database"); });

//builder.Services.AddDbContext<DataContext>(options =>
//{
//    options.UseMySql(connDocker, ServerVersion.AutoDetect(connDocker));
//});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// using (var serviceScope = app.Services.CreateScope())
// {
//     serviceScope.ServiceProvider.GetService<DataContext>().Database.Migrate();
// }

app.UseCors(builder =>
        builder
        .WithOrigins("https://localhost:44413", "https://localhost:3000", "http://localhost:3000", "http://localhost:3002",
        "http://localhost:5173", "https://localhost:5173")
        .AllowAnyMethod()
        .AllowAnyHeader());

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
