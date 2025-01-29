using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<GameAPIRestDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GameAPIRestDbContext") ?? throw new InvalidOperationException("Connection string 'GameAPIRestDbContext' not found.")));

// Add services to the container
builder.Services
    .AddControllers()
    .AddNewtonsoftJson(
        options =>
        options.SerializerSettings.ReferenceLoopHandling
          = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
