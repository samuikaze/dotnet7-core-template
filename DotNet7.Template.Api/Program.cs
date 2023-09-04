using DotNet7.Template.Api.ServiceProviders;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
}); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

SwaggerDefinitionServiceProvider.ConfigureSwagger(builder.Services);
ServiceMapperProvider.GetServiceProvider(builder.Services);
DatabaseServiceProvider.AddDatabaseContext(builder.Services, builder.Configuration);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(config =>
    {
        string? path = app.Configuration.GetValue<string>("Swagger:RoutePrefix");
        if (!string.IsNullOrEmpty(path))
        {
            config.PreSerializeFilters.Add((swaggerDoc, httpRequest) =>
            {
                string httpScheme = (app.Environment.IsDevelopment()) ? httpRequest.Scheme : "https";
                swaggerDoc.Servers = new List<OpenApiServer> {
                    new OpenApiServer { Url = $"{httpScheme}://{httpRequest.Host.Value}{path}" }
                };
            });
        }
    });
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
