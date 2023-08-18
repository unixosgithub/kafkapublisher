using Confluent.Kafka;
using kafkapublisher.Kafka;

namespace kafkapublisher
{
    public interface IProducer
    {
        ResponseMesage PublishMessage(Message<string, string> message);
        ICryptoSettings /*IProducerSettings*/ GetConfigSettings();
    }
}
