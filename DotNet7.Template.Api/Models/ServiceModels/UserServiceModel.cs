namespace DotNet7.Template.Api.Models.ServiceModels
{
    public class UserServiceModel
    {
        /// <summary>
        /// 使用者帳號 PK
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 使用者帳號
        /// </summary>
        public string? Account { get; set; }
        /// <summary>
        /// 暱稱
        /// </summary>
        public string? Username { get; set; }
        /// <summary>
        /// 連絡電話
        /// </summary>
        public string? Phone { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public List<string>? Roles { get; set; }
        /// <summary>
        /// 權限
        /// </summary>
        public List<string>? Abilities { get; set; }
        /// <summary>
        /// 註冊時間
        /// </summary>
        public DateTime? RegisteredAt { get; set; }
        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}
