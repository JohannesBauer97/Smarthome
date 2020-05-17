using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SmartServer.Worker;
using SmartServer.Worker.Abstraction;

namespace SmartServer
{
  public class Startup
  {
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

      services.AddControllers();
      services.AddHostedService<MqttBrokerService>();
      services.AddHostedService<AutodiscoverService>();
      services.AddSingleton<IMqttClientService, MqttClientService>();
      services.AddSingleton<IAutodiscoverService, AutodiscoverService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.RoutePrefix = "swagger";
        c.SwaggerEndpoint($"/swagger/{Constants.VERSION.ToString()}/swagger.json",
          $"SmartServer v{Constants.VERSION.ToString()}");
      });
      app.UseRouting();
      app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
  }
}