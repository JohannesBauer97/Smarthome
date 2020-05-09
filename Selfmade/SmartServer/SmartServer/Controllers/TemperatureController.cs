using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartServer.Repositories.Abstraction;

namespace SmartServer.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TemperatureController : ControllerBase
  {
    private readonly ILogger<TemperatureController> _logger;
    private readonly ITemperatureRepository _temperatureRepository;

    public TemperatureController(ILogger<TemperatureController> logger, ITemperatureRepository temperatureRepository)
    {
      _logger = logger;
      _temperatureRepository = temperatureRepository;
    }

    [HttpGet]
    public string Get()
    {
      return "";
    }
  }
}