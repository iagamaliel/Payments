using Payments.Business.Models;
using System.Collections.Generic;

namespace Payments.Business.Interfaces
{
    public interface ICompanyRepository
    {
        List<Company> GetAll();
    }
}
