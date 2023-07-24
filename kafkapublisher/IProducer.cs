using Confluent.Kafka;

namespace kafkapublisher
{
    public interface IProducer
    {
        ResponseMesage PublishMessage(Message<string, string> message);
    }
}