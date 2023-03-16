using Elastic02.Models;
using Elastic02.Services;
using Elastic02.Services.Test;
using Elasticsearch.Net;
using Nest;

var builder = WebApplication.CreateBuilder(args);

//string uri = @"http://localhost:9200";//local
//string uri = @"http://10.0.10.146:9200";//server
var elasticSettings = new ElasticSettings();
builder.Configuration.GetSection("Elastic").Bind(elasticSettings);

var urls = new Urls();
builder.Configuration.GetSection("Urls").Bind(urls);
string uri = elasticSettings.EsEndPoint;
//var pool = new SingleNodeConnectionPool(new Uri(uri));
//var pool = new SingleNodeConnectionPool(new Uri("http://10.0.10.146:9200"));
//var pool = new SingleNodeConnectionPool(new Uri("http://elastic:Mz*k3+jzBMPOgZMFhPPB@localhost:9200/"));
var pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));
var settings = new ConnectionSettings(pool);
    //.CertificateFingerprint("FINGERPRINT")
    //.BasicAuthentication("elastic", "password")
    //.EnableHttpPipelining()
    //.DisableDirectStreaming()
    //.EnableApiVersioningHeader();
    //.EnableHttpPipelining()
    //.DisableDirectStreaming()
    //.EnableApiVersioningHeader()
    //.EnableDebugMode();

var elasticClient = new ElasticClient(settings);

// Add services to the container.
builder.Services.AddSingleton(elasticClient);

builder.Services.AddScoped(typeof(IElasticRepository<>), typeof(ElasticRepository<>));

builder.Services.AddScoped(typeof(IElasticService<>), typeof(ElasticService<>));

builder.Services.AddScoped(typeof(IElasticGeoRepository<>), typeof(ElasticGeoRepository<>));
//builder.Services.AddScoped(typeof(IGeoService), typeof(GeoService));
builder.Services.AddScoped(typeof(IHaNoiRoadService), typeof(HaNoiRoadService));
builder.Services.AddScoped(typeof(IHaNoiShapeService), typeof(HaNoiShapeService));
builder.Services.AddScoped(typeof(IVietNamShapeService), typeof(VietNameShapeService));
builder.Services.AddScoped(typeof(IRoadNameService), typeof(RoadNameService));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsProduction())
{
    //app.Urls.Add(urls.WindowsIP);// Windows IP
    //app.Urls.Add(urls.WSLIP);// WSL IPhttp://172.20.0.1:6000
    //app.Urls.Add("http://172.20.2.181:6000");// WSL IPhttp://172.20.0.1:6000
    app.Urls.Add(urls.UbuntuIP);// Ubuntu IP
    app.Urls.Add(urls.LocalIP);// 127.0.0.1 IP
    //app.Urls.Add("http://localhost:6000");// 127.0.0.1 IP
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//           Path.Combine(builder.Environment.ContentRootPath, "MyStaticFiles")),
//    RequestPath = "/StaticFiles"
//});

app.MapControllers();

app.Run();
