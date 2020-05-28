using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartServer.Common.Models;
using SmartServer.Repositories.Abstraction;

namespace SmartServer.Controllers
{
  [Route("api/temperature")]
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

    [HttpGet("devices")]
    [ProducesResponseType(typeof(List<SmartTemperatureClient>), 200)]
    public IActionResult Get()
    {
      return Ok(_temperatureRepository.GetAllDevices());
    }

    [HttpPut("device")]
    [ProducesResponseType(typeof(SmartTemperatureClient), 200)]
    public IActionResult UpdateSmartTemperatureClient([FromBody]SmartTemperatureClient smartTemperatureClient)
    {
      var res = _temperatureRepository.UpdateSmartTemperatureClient(smartTemperatureClient);
      if (res == null)
      {
        return NotFound();
      }
      return Ok(res);
    }
  }
}