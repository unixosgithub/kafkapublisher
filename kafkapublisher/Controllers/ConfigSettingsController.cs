using kafkapublisher.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace kafkapublisher.Controllers
{
    [Route("/settings")]
    [ApiController]    
    [Consumes("application/json", "application/vnd.api+json")]
    [Produces("application/json", "application/vnd.api+json")]
    public class ConfigSettingsController : ControllerBase
    {
        private readonly IConfiguration _configSettings;

        public ConfigSettingsController(IConfiguration config)
        {
            _configSettings = config;            
        }

        [HttpGet(Name = "configsettings")]
        public IActionResult Get()
        {
            return Ok(_configSettings.GetSection("KafkaSettings").GetChildren());
        }
    }
}
