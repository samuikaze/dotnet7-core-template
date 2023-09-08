namespace DotNet7.Template.Api.Models.ServiceModels
{
    public class HttpClientBaseResponseServiceModel<T>
    {
        /// <summary>
        /// HTTP 狀態碼
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// 回應資料
        /// </summary>
        public T? Data { get; set; }
    }
}
