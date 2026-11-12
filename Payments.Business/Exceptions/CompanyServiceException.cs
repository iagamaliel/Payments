using System;

namespace Payments.Business.Exceptions
{
    public class CompanyServiceException : Exception
    {
        public CompanyServiceException(string message, Exception inner = null)
            : base(message, inner)
        {
        }
    }
}
