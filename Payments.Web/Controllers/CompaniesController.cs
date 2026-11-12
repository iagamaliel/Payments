using Payments.Business.Models;
using Payments.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace Payments.Web.Controllers
{
    [System.Web.Http.RoutePrefix("api/v1/companies")]
    public class CompaniesController : ApiController
    {
        private readonly CompanyRepository _companyRepository;
        private readonly ChannelRepository _channelRepository;

        public CompaniesController()
        {
            _companyRepository = new CompanyRepository();
            _channelRepository = new ChannelRepository();
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetCompanies()
        {
            try
            {
                string channel = Request.GetQueryNameValuePairs()
                .FirstOrDefault(x =>
                    string.Equals(
                        x.Key,
                        "channel",
                        StringComparison.OrdinalIgnoreCase))
                .Value;

                if (string.IsNullOrWhiteSpace(channel))
                {
                    return BadRequest("Channel is required.");
                }

                channel = channel.Trim();

                ChannelConfiguration configuration =
                    _channelRepository.GetConfiguration(channel);

                if (configuration == null)
                {
                    return NotFound();
                }

                IEnumerable<string> apiKeyHeaders;

                if (!Request.Headers.TryGetValues(
                    "X-Api-Key",
                    out apiKeyHeaders))
                {
                    return Unauthorized();
                }

                string providedApiKey = apiKeyHeaders.FirstOrDefault();

                if (string.IsNullOrWhiteSpace(providedApiKey) ||
                    string.IsNullOrWhiteSpace(configuration.ApiKey) ||
                    !string.Equals(
                        providedApiKey,
                        configuration.ApiKey,
                        StringComparison.Ordinal))
                {
                    return Unauthorized();
                }

                var companies = _companyRepository.GetAll();

                return Ok(companies);
            }
            catch (Exception exception)
            {
                System.Diagnostics.Trace.TraceError(
                    "Error retrieving companies: {0}",
                    exception);

                return InternalServerError();
            }
        }
    }
}