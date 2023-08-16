using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace kafkapublisher.Controllers
{
    
    [ApiController]    
    [Route("[controller]")]
    [Consumes("application/json","application/vnd.api+json")]
    [Produces("application/json", "application/vnd.api+json")]
    public class ProducerController : ControllerBase
    {        
        private readonly ILogger<ProducerController> _logger;
        private readonly IProducer _producer;

        public ProducerController(ILogger<ProducerController> logger, IProducer producer)
        {
            _logger = logger;
            _producer = producer;
        }

        [HttpPost("publish")]
        public async Task<IActionResult> PublishMessage([FromBody] RequestMessage request)
        {
            try
            {
                Message<string,string> message = new Message<string, string>() { Key = request.Key, Value = request.Value };
                var response = _producer?.PublishMessage(message);
                return (response?.Sucess == true) ? StatusCode(Convert.ToInt32(HttpStatusCode.OK), response) :
                    StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError));
            }
            catch (Exception)
            {
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError));
            }
        }

        [HttpGet("getconfig")]
        public IActionResult Get()
        {
            if (_producer != null) 
            {
                var settings = _producer?.GetConfigSettings();
                if (settings != null) 
                {
                    return Ok(settings.SaslPassword);
                }
            }
            return BadRequest("Failed to get config settings");
        }
    }
}
