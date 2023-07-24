namespace kafkapublisher.Kafka
{
    public interface IProducerSettings
    {
        string BootstrapServers { get; set; }
        string SecurityProtocol { get; set; }
        string SaslMechanisms { get; set; }
        string SaslUsername { get; set; }
        string SaslPassword { get; set; }
        string Topic { get; set; }
    }
}