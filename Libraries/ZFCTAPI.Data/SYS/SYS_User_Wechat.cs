using Dapper.Contrib.Extensions;
using ZFCTAPI.Data;

namespace ZFCTAPI.Data.SYS
{
    [Table("SYS_User_Wechat")]
    public partial class SYS_User_Wechat : BaseEntity
    {
        /// <summary>
        /// Customer id
        /// </summary>
        public int? cst_customer_id { get; set; }

        /// <summary>
        /// 微信昵称
        /// </summary>
        public string cst_user_name { get; set; }

        /// <summary>
        /// openid
        /// </summary>
        public string cst_user_uopenid { get; set; }

        /// <summary>
        /// unionid
        /// </summary>
        public string cst_user_unionid { get; set; }

        /// <summary>
        /// 微信头像
        /// </summary>
        public string cst_user_weurl { get; set; }

        /// <summary>
        /// 关注状态
        /// </summary>
        public bool cst_we_subscribe { get; set; }

        /// <summary>
        /// 关注时间
        /// </summary>
        public string cst_we_subscribe_time { get; set; }

        /// <summary>
        /// 邀请码
        /// </summary>
        public string cst_invitation_code { get; set; }
    }
}