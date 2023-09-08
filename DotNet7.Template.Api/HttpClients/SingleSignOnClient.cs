namespace DotNet7.Template.Api.HttpClients
{
    public class SingleSignOnClient
    {
        private HttpClient _client { get; set; }

        public SingleSignOnClient(HttpClient httpClient, IConfiguration config)
        {
            httpClient.BaseAddress = new Uri(config.GetValue<string>("Services:SingleSignOn") ?? "http://127.0.0.1/");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _client = httpClient;
        }

        /// <summary>
        /// 設定請求標頭
        /// </summary>
        /// <param name="key">標頭名稱</param>
        /// <param name="value">標頭值</param>
        public void AddHeaders(string key, string value)
        {
            _client.DefaultRequestHeaders.Add(key, value);
        }

        /// <summary>
        /// 取得 HttpClient
        /// </summary>
        /// <returns></returns>
        public HttpClient GetClient()
        {
            return _client;
        }

        /// <summary>
        /// 驗證登入狀態，並取得帳號資訊
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResponseMessage> VerifyAuthorization()
        {
            return await _client.GetAsync("/api/v1/user");
        }
    }
}
