using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.Interest
{
    public partial class CalculateCreditorRequest
    {
        /// <summary>
        /// 贷款金额
        /// </summary>
        public decimal LoanAmount { set; get; }

        /// <summary>
        /// 贷款利率（年利率）
        /// </summary>
        public decimal LoanRate { set; get; }


        /// <summary>
        /// 贷款期限类型 ？ 月 日
        /// </summary>
        public int InterestBearing { set; get; }

        /// <summary>
        /// 贷款期限 ？ 月 日
        /// </summary>
        public int LoanDurTime { set; get; }

        /// <summary>
        /// 账单日
        /// </summary>
        public int BillDay { set; get; }

        /// <summary>
        /// 还款期数
        /// </summary>
        public int RepaymentPeriods { set; get; }

        /// <summary>
        /// 结算方式  (非)固定还款日
        /// </summary>
        public int SettlementWay { set; get; }

        /// <summary>
        /// 收费值
        /// </summary>
        public decimal ManagementFee { set; get; }

        /// <summary>
        /// 收费方式  笔/比例
        /// </summary>
        public int ChargeWay { set; get; }

        /// <summary>
        /// 项目类型
        /// </summary>
        public int ProjectTypes { set; get; }

        /// <summary>
        /// 还款方式
        /// </summary>
        public string RepaymentType { get; set; }

    }

    public partial class InterestWayShow
    {
        /// <summary>
        /// 计息名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 计息代码
        /// </summary>
        public string Code { get; set; }
    }
}
