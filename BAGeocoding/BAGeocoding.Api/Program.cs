using BAGeocoding.Api;
using BAGeocoding.Api.Models;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Nest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<GeoDatabaseSettings>(
    builder.Configuration.GetSection("GeoDatabaseSettings"));

var elasticSettings = new ElasticSettings();
builder.Configuration.GetSection("Elastic").Bind(elasticSettings);

//var urls = new Urls();
//builder.Configuration.GetSection("Urls").Bind(urls);
string uri = elasticSettings.EsEndPoint;
//var pool = new SingleNodeConnectionPool(new Uri(uri));
var pool = new SingleNodeConnectionPool(new Uri("http://10.0.10.146:9200"));
//var pool = new SingleNodeConnectionPool(new Uri("http://elastic:Mz*k3+jzBMPOgZMFhPPB@localhost:9200/"));
//var pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));
var settings = new ConnectionSettings(pool);


var elasticClient = new ElasticClient(settings);

// Add services to the container.
builder.Services.AddSingleton(elasticClient);

builder.Services.RegisterService();

builder.Services.AddHostedService<BackgroundWorkerService>();

// Add services to the container.
//builder.Services.AddControllers();
builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                    new HeaderApiVersionReader("x-api-version"),
                                                    new MediaTypeApiVersionReader("x-api-version"));
});

// Add ApiExplorer to discover versions
builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//if (builder.Environment.IsProduction())
//{
//    builder.Services.AddHttpsRedirection(options =>
//    {
//        options.RedirectStatusCode = (int)HttpStatusCode.PermanentRedirect;
//        options.HttpsPort = 5000;
//    });
//}

var app = builder.Build();
if(app.Environment.IsProduction())
{
    //app.Urls.Add("http://10.0.20.228:5000");// Windows IP
    //app.Urls.Add("http://172.19.214.232:5000");// WSL IP
    //app.Urls.Add("http://10.0.10.146:5000");// Ubuntu IP
    app.Urls.Add("http://10.0.10.146:5001");// Ubuntu IP Test
    //app.Urls.Add("http://localhost:5000");// 127.0.0.1 IP
}    


var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
