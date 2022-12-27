using AspMongoApi.Models;
using AspMongoApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<StudentsDbSettings>(
    builder.Configuration.GetSection("StudentsDbSettings"));

builder.Services.AddSingleton<StudentsService>();

builder.Services.Configure<RestaurantsDbSetting>(
    builder.Configuration.GetSection("RestaurantsDbSettings"));

builder.Services.AddSingleton<RestaurantsServices>();

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
