using Dapper.Contrib.Extensions;

namespace ZFCTAPI.Data.WeChat
{
    [Table("tbWechat")]
    public partial class tbWechat : BaseEntity
    {
        public string Token { get; set; }
        public string EncodingAESKey { get; set; }
        public string AppID { get; set; }
        public string AppSecret { get; set; }
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string jsapi_ticket { get; set; }
        public string jsapi_ticket_expires_in { get; set; }
    }
}