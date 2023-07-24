using Confluent.Kafka;
using static Confluent.Kafka.ConfigPropertyNames;
using kafkapublisher.Kafka;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using System.Net;

namespace kafkapublisher
{    
    public class Producer : IProducer
    {
        private readonly ClientConfig _clientConfig;
        private readonly ProducerBuilder<string,string> _producerBuilder;
        private readonly Confluent.Kafka.IProducer<string, string> _producer;
        private readonly string _topic;
        

        public Producer(IProducerSettings producerSettings)
        {
            _clientConfig = new ClientConfig()
            {
                BootstrapServers = producerSettings.BootstrapServers,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = producerSettings.SaslUsername,
                SaslPassword = producerSettings.SaslPassword,                
            };
            
            _topic = producerSettings.Topic;
            _producerBuilder = new ProducerBuilder<string, string>(_clientConfig);
            _producer = _producerBuilder.Build();
        }
        

        public ResponseMesage PublishMessage(Message<string,string> message)
        {
            var response = new ResponseMesage();
            try
            {
                _producer?.Produce(_topic, message);
                response.Sucess = true;
                response.Error = string.Empty;
            }
            catch (Exception ex)
            {
                response.Sucess = false;
                response.Error = ex.Message;
            }

            return response;
        }
    }

    
    
    
}
