using Confluent.Kafka;
using static Confluent.Kafka.ConfigPropertyNames;
using kafkapublisher.Kafka;
using kafkapublisher.Crypt;
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
        private readonly IProducerSettings producerSettings;
        private readonly IDecryptAsymmetric _decryptAsymmetric;


        public Producer(IConfiguration config, IDecryptAsymmetric decryptAsymmetric)
        {
            producerSettings = config?.GetSection("KafkaSettings")?.Get<ProducerSettings>();
            _decryptAsymmetric = decryptAsymmetric;
            _clientConfig = new ClientConfig()
            {
                BootstrapServers = producerSettings?.BootstrapServers,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = producerSettings?.SaslUsername,
                //SaslPassword = producerSettings?.SaslPassword,                
            };

            // Decrypt the password 
            var cryptoSettings = _decryptAsymmetric?.GetConfigSettings();
            if ((cryptoSettings != null))
            {
                //try
                //{
                    byte[] cipherText = Convert.FromBase64String(producerSettings?.SaslPassword);
                    if (cipherText?.Length > 0)
                    {
                        _clientConfig.SaslPassword = _decryptAsymmetric?.DecryptAsymmetricString(cipherText);                        
                    }                    
                //}
                //catch(Exception ex)
                //{ 
                //    throw new Excepteion(ex);
                //}
            }
            
            _topic = producerSettings.Topic;
            _producerBuilder = new ProducerBuilder<string, string>(_clientConfig);
            _producer = _producerBuilder.Build();
        }
        
        public IProducerSettings GetConfigSettings()
        {
            return producerSettings;
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
