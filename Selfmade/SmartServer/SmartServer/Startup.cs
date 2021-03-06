using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SmartServer.Ef;
using SmartServer.Ef.Abstraction;
using SmartServer.Repositories;
using SmartServer.Repositories.Abstraction;
using SmartServer.Worker;
using SmartServer.Worker.Abstraction;
using EnvironmentName = Microsoft.AspNetCore.Hosting.EnvironmentName;

namespace SmartServer
{
  public class Startup
  {
    private readonly IWebHostEnvironment _webHostEnvironment;

    public Startup(IWebHostEnvironment webHostEnvironment)
    {
      _webHostEnvironment = webHostEnvironment;
    }
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc(Constants.VERSION.ToString(), new OpenApiInfo
        {
          Title = "SmartServer",
          Version = Constants.VERSION.ToString(),
          Description = "Made with <3 by Johannes Bauer"
        });
        c.IncludeXmlComments($"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
      });

      if (_webHostEnvironment.IsDevelopment())
      {
        services.AddCors(o => o.AddDefaultPolicy(builder =>
        {
          builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
        }));
      }

      services.AddSpaStaticFiles(options =>
      {
        options.RootPath = "wwwroot";
      });

      services.AddDbContext<SmartServerContext>(opt => opt.UseSqlite(Constants.SQLITE_CONNECTION_STRING), ServiceLifetime.Transient);
      services.AddControllers();
      services.AddHostedService<MqttBrokerService>();
      services.AddSingleton<IMqttTemperatureClientService, MqttTemperatureTemperatureClientService>();
      services.AddSingleton<IAutodiscoverService, AutodiscoverService>();
      services.AddScoped<ITemperatureRepository, TemperatureRepository>();
      services.AddSingleton<ITemperatureDataSource, TemperatureDataSource>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
      app.UseDefaultFiles();
      app.UseSpaStaticFiles();
      app.UseRouting();
      app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
      app.UseSwaggerUI(c =>
      {
        c.RoutePrefix = "swagger";
        c.SwaggerEndpoint($"/swagger/{Constants.VERSION.ToString()}/swagger.json",
          $"SmartServer v{Constants.VERSION.ToString()}");
      });
      app.UseSwagger();
      app.UseSpa(builder => {});
      app.UseCors();
    }
  }
}