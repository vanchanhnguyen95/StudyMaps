using Elastic02.Models;
using Elastic02.Services;
using Nest;

var builder = WebApplication.CreateBuilder(args);

var appSettings = new AppSettings();

var elasticClient = new ElasticClient(new Uri("http://localhost:9200"));

builder.Services.AddSingleton(elasticClient);
builder.Services.AddScoped(typeof(IElasticRepository<>), typeof(ElasticRepository<>));

builder.Services.AddScoped(typeof(IElasticService<>), typeof(ElasticService<>));

// Add services to the container.

builder.Services.AddControllers();
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
