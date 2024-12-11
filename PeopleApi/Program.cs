using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using PeopleApi.Di;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PeopleApi.Core.Misc;
namespace PeopleApi;
/*
 builder.Services.AddCors siehe ExtensionMethods
 */
public class Program {
   
   private static void Main(string[] args){

      // path for PeopleApi images
      var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
      var wwwroot = Path.Combine(home, "PeopleApi", "v1");

      //
      // Configure API
      //
      var builder = WebApplication.CreateBuilder(args);
      builder.Environment.WebRootPath = wwwroot; // path to images;
      
      // Add Logging Services to DI-Container
      builder.Logging.ClearProviders();
      builder.Logging.AddConsole();
      builder.Logging.AddDebug();
      // Write Logging to Debug into a file
      // var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); 
      // var tracePath = Path.Join(path, $"Log_WebApi_{DateTime.Now.ToString("yyyy_MM_dd-HHmmss")}.txt");
      // Trace.Listeners.Add(new TextWriterTraceListener(File.Create(tracePath)));
      // Trace.AutoFlush = true;
      
      // Configure DI-Container aka builder.Services:IServiceCollection
      // ---------------------------------------------------------------------
      // add http logging 
      builder.Services.AddHttpLogging(opts =>
         opts.LoggingFields = HttpLoggingFields.All);
      
      // add controllers
      builder.Services.AddControllers()
         .AddJsonOptions(opt => {
            opt.JsonSerializerOptions.Converters.Add(new IsoDateTimeConverter());
            opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            opt.JsonSerializerOptions.PropertyNamingPolicy = new LowerCaseNamingPolicy();
         });
      
      // add core
      builder.Services.AddCore();
      
      // add Persistence
      builder.Services.AddPersistence(builder.Configuration);

      // add Error handling
      builder.Services.AddProblemDetails();
      
      // add API versioning
      var apiVersionReader = ApiVersionReader.Combine(
         new UrlSegmentApiVersionReader(),
         new HeaderApiVersionReader("x-api-version")
         // new MediaTypeApiVersionReader("x-api-version"),
         // new QueryStringApiVersionReader("api-version")
      );
      builder.Services.AddApiVersioning(opt=> {
            opt.DefaultApiVersion = new ApiVersion(1, 0);
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.ReportApiVersions = true;
            //          opt.ApiVersionReader = new UrlSegmentApiVersionReader();
            opt.ApiVersionReader = apiVersionReader;
         })
         .AddMvc()
         .AddApiExplorer(options => {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
         });
      
      // add OpenApi/Swagger
      builder.Services.AddSwaggerGen(opt => {
         var dir = new DirectoryInfo(AppContext.BaseDirectory);
         // combine WebApi.Controllers.xml and WebApi.Core.xml
         foreach (var file in dir.EnumerateFiles("*.xml")) {
            opt.IncludeXmlComments(file.FullName);
         }
      });
      builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
      
      // limit imageFile max size
      builder.Services.Configure<FormOptions>(options => {
         options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10 MB limit
      });
      
      
      // add Cors 
      builder.Services.AddCors(options => {
         options.AddDefaultPolicy(policy => { policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
         });
      // builder.Services.AddCors(options => {
      //    options.AddDefaultPolicy(policy => {
      //       policy.WithOrigins(
      //      //    "http://localhost", 
      //            "http://localhost:5010"
      //      //    "https://localhost:7010", 
      //      //    "http://localhost:63342"
      //       ).AllowAnyMethod().AllowAnyHeader();
      //    });
      });

      var app = builder.Build();
      
      //
      // Configure Http Pipeline
      //

      // use middleware to handle errors
      if (app.Environment.IsDevelopment())
         app.UseExceptionHandler("/people/v1/error-development");
      else
         app.UseExceptionHandler("/peopleapi/v1/error");

      // API Versioning, OpenAPI/Swagger documentation
      var provider =
         app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
      if(app.Environment.IsDevelopment()){
         app.UseSwagger();
         app.UseSwaggerUI(options => {
            foreach(var description in provider.ApiVersionDescriptions){
               options.SwaggerEndpoint(
                  $"/swagger/{description.GroupName}/swagger.json",
                  description.GroupName.ToUpperInvariant());
            }
         });
      }

      // app.UseSwaggerUI(c =>
      // {
      //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
      // });
      
      app.UseCors();
      
      app.UseRouting();
      // app.UseAuthentication();
      // app.UseAuthorization();
      app.MapControllers();
      app.Run();
   }
}

public class LowerCaseNamingPolicy : JsonNamingPolicy {
   public override string ConvertName(string name) =>
      name.ToLower();
}
