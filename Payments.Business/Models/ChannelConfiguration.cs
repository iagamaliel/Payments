namespace Payments.Business.Models
{
    public  class ChannelConfiguration
    {
        public string ChannelCode { get; set; }
        public string ServiceUrl { get; set; }
        public string ApiKey { get; set; }
        public bool ShowMenu { get; set; }
        public string DestinationPage { get; set; }
    }
}
