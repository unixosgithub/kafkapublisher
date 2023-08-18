using Confluent.Kafka;
using kafkapublisher.Kafka;
using kafkapublisher.Crypt;

namespace kafkapublisher
{
    public interface IProducer
    {
        ResponseMesage PublishMessage(Message<string, string> message);
        ICryptoSettings /*IProducerSettings*/ GetConfigSettings();
    }
}
