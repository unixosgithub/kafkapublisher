using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace kafkapublisher.Controllers
{
    [Route("/sh")]
    [ApiController]
    [Route("[controller]")]
    [Consumes("application/json","application/vnd.api+json")]
    [Produces("application/json", "application/vnd.api+json")]
    public class ProducerController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ProducerController> _logger;
        private readonly IProducer _producer;

        public ProducerController(ILogger<ProducerController> logger, IProducer producer)
        {
            _logger = logger;
            _producer = producer;
        }

        //[HttpGet(Name = "GetWeatherForecast")]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

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
    }
}