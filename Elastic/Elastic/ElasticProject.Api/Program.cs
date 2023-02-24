using ElasticProject.Data.Interface;
using ElasticProject.Data.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IElasticsearchService, ElasticsearchService>();
builder.Services.AddScoped<IRoadService, RoadService>();
builder.Services.AddScoped<IElsGeopointService, ElsGeopointService>();
builder.Services.AddScoped<IElsGeoshapeService, ElsGeoshapeService>();
builder.Services.AddScoped<IHealthCareService, HealthCareService>();
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
