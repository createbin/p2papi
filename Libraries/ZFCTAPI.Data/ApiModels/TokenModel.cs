using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels
{
    /// <summary>
    /// 令牌模型
    /// </summary>
    public class TokenModel
    {
        public TokenModel()
        {

        }
        public TokenModel(int userid)
        {
            StratDate = DateTime.Now.Ticks;
            EndDate = DateTime.Now.AddMinutes(20 * 60).Ticks;
            UserId = userid;
        }
        /// <summary>
        /// 开始日期
        /// </summary>
        public long StratDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public long EndDate { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }
    }
}
