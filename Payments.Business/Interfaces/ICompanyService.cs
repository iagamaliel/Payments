using Payments.Business.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Payments.Business.Interfaces
{
    public interface ICompanyService
    {
        Task<IList<Company>> GetCompaniesAsync(ChannelConfiguration configuration);
    }
}
