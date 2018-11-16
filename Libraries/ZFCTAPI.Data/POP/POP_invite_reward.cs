using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZFCTAPI.Data.CST;

namespace ZFCTAPI.Data.POP
{
    /// <summary>
    /// 数据表POP_invite_reward的数据库实体类
    /// </summary>
    [Table("POP_invite_reward")]
    public partial class POP_invite_reward : BaseEntity
    {
        /// <summary>
        /// 邀请人
        /// </summary>
        public virtual CST_user_info InviteUserInfo { get; set; }
        /// <summary>
        /// 被邀请人
        /// </summary>
        public virtual CST_user_info PopUserInfo { get; set; }
        /// <summary>
        ///  邀请人Id 
        /// </summary>	
        public int? pop_invite_userId { get; set; }
        /// <summary>
        ///  被邀请人Id 
        /// </summary>	
        public int? pop_user_Id { get; set; }
        /// <summary>
        ///  1：注册成功 2：账户未开户 3：账户已开户 4：已充值 5：已投资 6：已放款 
        /// </summary>	
        public int? pop_was_userId { get; set; }
        /// <summary>
        ///  是否已生效 
        /// </summary>	
        public int? pop_is_effect { get; set; }
    }
}
