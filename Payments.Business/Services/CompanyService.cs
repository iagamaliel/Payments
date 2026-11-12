using Newtonsoft.Json;
using Payments.Business.Exceptions;
using Payments.Business.Interfaces;
using Payments.Business.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Payments.Business.Services
{
    public class CompanyService : ICompanyService
    {
        private static readonly HttpClient Http = CreateHttpClient();

        private static HttpClient CreateHttpClient()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            return new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
        }

        public async Task<IList<Company>> GetCompaniesAsync(
            ChannelConfiguration configuration)
        {
            if (configuration == null ||
                string.IsNullOrWhiteSpace(configuration.ServiceUrl) ||
                string.IsNullOrWhiteSpace(configuration.ApiKey) ||
                string.IsNullOrWhiteSpace(configuration.ChannelCode))
            {
                throw new CompanyServiceException(
                    "The company service configuration is invalid.");
            }

            try
            {
                var separator = configuration.ServiceUrl.Contains("?")
                    ? "&"
                    : "?";

                var url = configuration.ServiceUrl + separator +
                          "channel=" +
                          Uri.EscapeDataString(configuration.ChannelCode);

                using (var request = new HttpRequestMessage(
                    HttpMethod.Get, url))
                {
                    request.Headers.Add(
                        "X-Api-Key",
                        configuration.ApiKey);

                    request.Headers.Accept.Add(
                        new MediaTypeWithQualityHeaderValue(
                            "application/json"));

                    using (var response = await Http.SendAsync(request)
                        .ConfigureAwait(false))
                    {
                        var responseContent =
                            await response.Content.ReadAsStringAsync()
                                .ConfigureAwait(false);

                        if (response.StatusCode == HttpStatusCode.Unauthorized ||
                            response.StatusCode == HttpStatusCode.Forbidden)
                        {
                            throw new CompanyServiceException(
                                "Authentication with the company service failed. " +
                                "HTTP Status: " +
                                (int)response.StatusCode);
                        }

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new CompanyServiceException(
                                "Company service request failed. " +
                                "HTTP Status: " +
                                (int)response.StatusCode +
                                " (" + response.StatusCode + "). " +
                                "Response: " + responseContent);
                        }

                        var companies =
                            JsonConvert.DeserializeObject<List<Company>>(
                                responseContent);

                        return companies ?? new List<Company>();
                    }
                }
            }
            catch (TaskCanceledException exception)
            {
                throw new CompanyServiceException(
                    "The request to the company service timed out.",
                    exception);
            }
            catch (HttpRequestException exception)
            {
                throw new CompanyServiceException(
                    "Unable to connect to the company service.",
                    exception);
            }
        }
    }
}
