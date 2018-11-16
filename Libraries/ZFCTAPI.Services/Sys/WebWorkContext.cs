using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Data.CST;

namespace ZFCTAPI.Services.SYS
{
    /// <summary>
    /// Work context
    /// </summary>
    public interface IWorkContext
    {
        /// <summary>
        /// 用户登录信息（网贷CST_user_info）
        /// </summary>
        CST_user_info CstUserInfo { get; set; }

    }
    public partial class WebWorkContext : IWorkContext
    {
        public CST_user_info CstUserInfo { get; set; }
    }
}
