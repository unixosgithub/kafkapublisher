namespace kafkapublisher.Kafka
{
    public class producerSettings : IProducerSettings
    {        
        public string BootstrapServers { get; set; }
        public string SecurityProtocol { get; set; }
        public string SaslMechanisms { get; set; }
        public string SaslUsername { get; set; }
        public string SaslPassword { get; set; }
        public string Topic { get; set; }

        public producerSettings() { }  
    }
}
