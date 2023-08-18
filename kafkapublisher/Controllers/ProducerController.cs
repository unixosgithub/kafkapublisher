using Confluent.Kafka;
using kafkapublisher.Crypt;
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
        private readonly IDecryptAsymmetric _decryptAsymmetric;

        public ProducerController(ILogger<ProducerController> logger, IProducer producer, IDecryptAsymmetric decryptAsymmetric)
        {
            _logger = logger;
            _producer = producer;
            _decryptAsymmetric = decryptAsymmetric;
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
                //var settings = _producer?.GetConfigSettings();

                var settings = _decryptAsymmetric?.GetConfigSettings();
                return Ok(settings.LocationId);
                /*
                var cryptoSettings = _decryptAsymmetric?.GetConfigSettings();
                if ((settings != null) && (cryptoSettings != null))
                {
                    try
                    {
                        byte[] cipherText = Convert.FromBase64String(settings.SaslPassword);
                        if (cipherText?.Length > 0)
                        {
                            var decryptedString = _decryptAsymmetric?.DecryptAsymmetricString(cipherText);
                            return Ok(decryptedString);                         
                        }
                        return BadRequest("cipher text is incorect");
                    }
                    catch(Exception ex)
                    { 
                        return BadRequest(ex);
                    }
                }
                */
            }
            return BadRequest("Failed to get the config settings");
        }
    }
}
