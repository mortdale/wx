using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WooliesX.Models;

namespace WooliesX.Services
{
    public abstract class BaseRequest
    {
        protected Settings Settings;
        private readonly IHttpClientFactory _clientFactory;

        protected BaseRequest(IOptions<Settings> option, IHttpClientFactory clientFactory)
        {
            Settings = option.Value;
            _clientFactory = clientFactory;
        }

        protected virtual StringContent GenerateJsonContent(object requestPayload)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestPayload,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        Converters = new JsonConverter[] { new Newtonsoft.Json.Converters.StringEnumConverter() },
                        ContractResolver =
                            new DefaultContractResolver
                            {
                                NamingStrategy = new CamelCaseNamingStrategy(false, true)
                            }
                    }), Encoding.UTF8,
                "application/json");

            return jsonContent;
        }

        private void SetAuthenticationToken(HttpRequestMessage request)
        {
            request.RequestUri = new Uri(request.RequestUri, $"?token={Settings.Token}");
        }

        protected async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            string reason = null;
            try
            {
                SetAuthenticationToken(request);

                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    reason = response.ReasonPhrase;
                }

                response.EnsureSuccessStatusCode();

                return response;
            }
            catch (Exception e)
            {
                if (!string.IsNullOrEmpty(reason))
                    e.Data.Add("ErrorResponse", reason);

                throw new HttpRequestException($"{request.Method} {request.RequestUri} failed : {reason}");
            }
            finally
            {

            }
        }
    }
}
