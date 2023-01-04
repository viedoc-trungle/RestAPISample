using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using TeaBreakApi;
using System.Reflection;
using TeaBreakApi.Controllers.TeaBreaks.v3;
using TeaBreakApi.Data;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using TeaBreakApi.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers(options =>
    {
        options.Filters.Add(new ProducesAttribute("application/json"));
        //options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());
    })
    .AddNewtonsoftJson()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = ModelStateValidator.ValidateModelState;
    })
    .AddFluentValidation(options =>
    {
        options.RegisterValidatorsFromAssemblyContaining<TeaBreakRequestValidator>();
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Set Title and version from config
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Teabreak API", Version = "1.0", Description = "Sample teakbreak API" });

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddSingleton<TeabreakRepository>();
builder.Services.AddSingleton<ProviderRepository>();

builder.Services.AddHATEOAS(options =>
{
    options.AddLink<TeaBreak>("self",
      "GetTeaBreakV2",
      HttpMethod.Post,
      (x) => new { id = x.Id });

    options.AddLink<TeaBreak>("replace",
      "ReplaceTeaBreakV2",
      HttpMethod.Put,
      (x) => new { id = x.Id });

    options.AddLink<TeaBreak>("update",
      "EditTeaBreakV2",
      HttpMethod.Patch,
      (x) => new { id = x.Id });

    options.AddLink<TeaBreak>("addorder",
      "AddOrderV2",
      HttpMethod.Post,
      (x) => new { id = x.Id });
});

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


public static class MyJPIF
{
    public static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
    {
        var builder = new ServiceCollection()
            .AddLogging()
            .AddMvc()
            .AddNewtonsoftJson()
            .Services.BuildServiceProvider();

        return builder
            .GetRequiredService<IOptions<MvcOptions>>()
            .Value
            .InputFormatters
            .OfType<NewtonsoftJsonPatchInputFormatter>()
            .First();
    }
}