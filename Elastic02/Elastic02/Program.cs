using Elastic02.Models;
using Elastic02.Services;
using Elastic02.Services.Test;
using Nest;

var builder = WebApplication.CreateBuilder(args);

var appSettings = new AppSettings();

string uri = @"http://localhost:9200";//local
//string uri = @"http://10.0.10.146:9200";//server

//if(builder.Build().Environment.IsProduction())
//{
//    uri = @"http://10.0.10.146:9200";
//}    

var elasticClient = new ElasticClient(new Uri(uri));

builder.Services.AddSingleton(elasticClient);
builder.Services.AddScoped(typeof(IElasticRepository<>), typeof(ElasticRepository<>));

builder.Services.AddScoped(typeof(IElasticService<>), typeof(ElasticService<>));

builder.Services.AddScoped(typeof(IElasticGeoRepository<>), typeof(ElasticGeoRepository<>));
//builder.Services.AddScoped(typeof(IGeoService), typeof(GeoService));
builder.Services.AddScoped(typeof(IHaNoiRoadService), typeof(HaNoiRoadService));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsProduction())
{
    app.Urls.Add("http://10.1.13.67:6000");// Windows IP
    ////app.Urls.Add("http://192.168.145.70:6000");// WSL IP
    //app.Urls.Add("http://10.0.10.146:6000");// Ubuntu IP
    app.Urls.Add("http://localhost:6000");// 127.0.0.1 IP
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
