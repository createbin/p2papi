using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.Customers
{
    public static partial class SystemCustomerRoleNames
    {
        public static string Administrators => "Administrators";

        public static string Registered => "Registered";

        public static string Guests => "Guests";

        public static string Xiaodai => "Xiaodai";

        public static string Danbao => "Danbao";

        public static string Caiwu => "Caiwu";
        public static string Kefu => "Kefu";

        /// <summary>
        /// 理财加盟商
        /// </summary>
        public static string Alb => "Alb";

        /// <summary>
        /// 理财加盟商员工
        /// </summary>
        public static string AlbEmp => "AlbEmp";

        /// <summary>
        /// 中融理财经理
        /// </summary>
        public static string ZrLiCai => "ZrLiCai";

        #region 众筹相关

        /// <summary>
        /// 平台客服
        /// </summary>
        public static string RaiseCustomerService => "RaiseCustomerService";

        /// <summary>
        /// 平台财务
        /// </summary>
        public static string RaiseFinance => "RaiseFinance";

        /// <summary>
        /// 平台风控
        /// </summary>
        public static string RaiseControl => "RaiseControl";

        #endregion

    }
}
