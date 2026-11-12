using Payments.Business.Models;

namespace Payments.Business.Interfaces
{
    public interface IChannelRepository
    {
        ChannelConfiguration GetConfiguration(string channelCode);
    }
}
