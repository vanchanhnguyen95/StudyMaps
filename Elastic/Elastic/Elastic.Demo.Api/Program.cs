using Elastic.Demo.Api;
using Elastic.Demo.Api.Interface;
using Elastic.Demo.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddElasticsearch(builder.Configuration);
builder.Services.AddScoped(typeof(IElasticRepository<>), typeof(ElasticRepository<>));

builder.Services.AddScoped(typeof(IElasticService<>), typeof(ElasticService<>));

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
