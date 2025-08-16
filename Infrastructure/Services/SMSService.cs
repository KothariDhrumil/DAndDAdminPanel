using Application.Abstractions.Authentication;
using Application.Communication;
using Application.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace Infrastructure.Services
{
    public class SMSService : ISMSService
    {
        public SMSConfiguration setting { get; }
        public ILogger<SMTPMailService> _logger { get; }

        private readonly HttpClient httpClient;


        public SMSService(IOptions<SMSConfiguration> settingConfig, ILogger<SMTPMailService> logger, IHttpClientFactory httpClientFactory)
        {
            setting = settingConfig.Value;
            _logger = logger;
            httpClient = httpClientFactory.CreateClient();
        }

        public async Task<string> SendOTPAsync(SMSRequestDTO smsRequest)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, string.Format(setting.OTPUrlFormat, setting.ApiKey, $"+91{smsRequest.To}", smsRequest.Body, smsRequest.Template));
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return response.StatusCode.ToString();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return ex.Message;
            }
        }

        public async Task<string> SendTransactionalSMSASync(SMSRequestDTO request)
        {
            try
            {

                // Prepare the request URL with the necessary parameters
                var values = new List<KeyValuePair<string, string>>
                {
                    new("module", "TRANS_SMS"),
                    new("apikey", setting.ApiKey),
                    new("to",$"+91{request.To}"),
                    new("from", "DELUX"),
                    new("msg", request.Body)
                };

                using (var content = new FormUrlEncodedContent(values))
                {
                    content.Headers.Clear();
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                    HttpResponseMessage response = await httpClient.PostAsync(setting.TransBaseURL, content);

                    var result = await response.Content.ReadAsStringAsync();

                    if (!result.Contains("Success"))
                    {
                        _logger.LogError("Error in sms", result);
                    }
                    response.EnsureSuccessStatusCode();
                    // Return the status code as a string
                    return response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return ex.Message;
            }

        }
    }
}