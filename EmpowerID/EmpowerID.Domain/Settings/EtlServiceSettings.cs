namespace EmpowerID.Domain.Settings
{
    public class EtlServiceSettings
    {
        public string SubscriptionId { get; set; }
        public string ResourceGroupName { get; set; }
        public string FactoryName { get; set; }
        public string PipelineName { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TenantId { get; set; }
    }
}