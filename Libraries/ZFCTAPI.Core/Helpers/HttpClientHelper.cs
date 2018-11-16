using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ZFCTAPI.Core.Configuration;

namespace ZFCTAPI.Core.Helpers
{
    public class HttpClientHelper
    {
        private static HttpClient _httpClient;
        private static readonly string ApiAddress = ApiEngineToConfiguration.GetBoHaiApi();
        static HttpClientHelper()
        {
            try
            {
                var handler = new HttpClientHandler {Proxy = null};
                _httpClient = new HttpClient(handler) { BaseAddress = new Uri(ApiAddress) };
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.9));
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/webp"));
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*", 0.8));
                _httpClient.Timeout = TimeSpan.FromSeconds(1000);
                _httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
                //帮HttpClient热身
                //_httpClient.SendAsync(new HttpRequestMessage
                //{
                //    Method = new HttpMethod("HEAD"),
                //    RequestUri = new Uri(ApiAddress)
                //}).Result.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                LogsHelper.WriteLog("链接处理时发生错误 错误信息：" + ex.Message);
                throw;
            }
        }

        public static Task<HttpResponseMessage> PostAsync(string postUrl, string postJson)
        {
            try
            {
                HttpContent httpContent = new StringContent(postJson, System.Text.Encoding.UTF8, "application/json");
                return (_httpClient ?? GetDefaultClient()).PostAsync(postUrl, httpContent);
            }
            catch (Exception ex)
            {
                LogsHelper.WriteLog("链接处理时发生错误 错误信息：" + ex.Message);
                throw;
            }
        }

        public static string GetAsync(string getUrl)
        {
            var responseJson = "";
            try
            {
                responseJson = (_httpClient ?? GetDefaultClient()).GetAsync(getUrl).Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception)
            {
                throw;
            }
            return responseJson;
        }

        private static HttpClient GetDefaultClient()
        {
            if (_httpClient != null) return _httpClient;
            LogsHelper.WriteLog("重新生成httpclient");
            _httpClient = new HttpClient();
            var handler = new HttpClientHandler();
            handler.Proxy = null;
            _httpClient = new HttpClient(handler) { BaseAddress = new Uri(ApiAddress) };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
            _httpClient.Timeout = TimeSpan.FromMinutes(30);
            _httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
            //帮HttpClient热身
            _httpClient.SendAsync(new HttpRequestMessage
            {
                Method = new HttpMethod("HEAD"),
                RequestUri = new Uri(ApiAddress + "/")
            }).Result.EnsureSuccessStatusCode();
            return _httpClient;
        }
    }
}
