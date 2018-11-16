using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Core.Configuration.DataBase
{
    /// <summary>
    /// 借款项目的全局设置
    /// </summary>
    public class ProjectSettings : ISettings
    {
        /// <summary>
        /// 获取或设置 起始最低信用额度
        /// </summary>
        public decimal sys_min_credit { get; set; }

        /// <summary>
        /// 获取或设置 罚息利率
        /// </summary>
        public decimal sys_over_rate { get; set; }

        /// <summary>
        /// 获取或设置 债权是否允许部分转让
        /// </summary>
        public bool sys_is_transfer { get; set; }

        /// <summary>
        /// 获取或设置 转让满标是否需要审核
        /// </summary>
        public bool sys_is_fullAudit { get; set; }

        /// <summary>
        /// 转让标是否支持部分认购
        /// </summary>
        ///
        public bool sys_is_partInvest { get; set; }

        /// <summary>
        /// 获取或设置 债权转让次数
        /// </summary>
        public int? sys_transfer_count { get; set; }

        /// <summary>
        /// 距离最近一次还款日流标的天数
        /// </summary>
        public int sys_min_payDates { get; set; }

        /// <summary>
        ///满标或待划转的债权距离最近一次还款日天数
        /// </summary>
        ///
        public int full_min_payDates { get; set; }

        /// <summary>
        /// 获取或设置 最低转让折扣
        /// </summary>
        public decimal sys_min_transfer_rate { get; set; }

        /// <summary>
        /// 获取或设置 最高转让折扣
        /// </summary>
        public decimal sys_max_transfer_rate { get; set; }

        /// <summary>
        /// 获取或设置 转让手续费率
        /// </summary>
        public decimal sys_transfer_rate { get; set; }

        /// <summary>
        /// 获取或设置 首次债权转让距放款周期
        /// </summary>
        public int sys_transfer_cycle { get; set; }

        /// <summary>
        /// 获取或设置 首次债权转让距放款周期类型
        /// </summary>
        public int sys_transfer_type { get; set; }

        /// <summary>
        /// 获取或设置 个体每日融资项目数
        /// </summary>
        public int sys_borrow_count { get; set; }

        /// <summary>
        /// 获取或设置 上期代还未收回是否继续代还
        /// </summary>
        public bool sys_repay_limit { get; set; }

        /// <summary>
        /// 获取或设置 可提前还款天数
        /// </summary>
        public int sys_prepayment_days { get; set; }

        /// <summary>
        /// 支付订单有效时间
        /// </summary>
        public int sys_orderValidateTime { get; set; }

        /// <summary>
        /// 到期提醒天数
        /// </summary>
        public int sys_expire_day { get; set; }

        /// <summary>
        /// 获取或设置 用户名
        /// </summary>
        public string sys_user_name { get; set; }

        /// <summary>
        /// 获取或设置 密码
        /// </summary>
        public string sys_user_password { get; set; }

        /// <summary>
        /// 获取或设置 编号模板
        /// </summary>
        public string sys_template_no { get; set; }

        /// <summary>
        ///  获取或设置 提成规则编号模板
        /// </summary>
        public string rule_template_no { get; set; }

        /// <summary>
        /// 获取或设置 编号模板最大编号
        /// </summary>
        public int? sys_maxId { get; set; }

        /// <summary>
        /// 获取或设置  提成规则最大编号
        /// </summary>
        public int? rule_maxId { get; set; }

        /// <summary>
        /// 是否验证
        /// </summary>
        public bool isValidator { get; set; }

        /// <summary>
        /// 投资转让列表显示条数
        /// </summary>
        public int? invertPageSize { get; set; }
    }
}