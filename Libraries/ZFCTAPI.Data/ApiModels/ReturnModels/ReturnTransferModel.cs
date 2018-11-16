using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.ReturnModels
{
    public class RTansferAbstract
    {
            /// <summary>
            /// 标的id
            /// </summary>
            public int LoanId { get; set; }
            /// <summary>
            /// 转让id
            /// </summary>
            public int TransferId { get; set; }

            /// <summary>
            /// 项目名称
            /// </summary>
            public string LoanName { get; set; }

            /// <summary>
            /// 年华收益率
            /// </summary>
            public decimal YearRate { get; set; }

            /// <summary>
            /// 投资期限
            /// </summary>
            public int RemainPeriod { get; set; }

            /// <summary>
            /// 还款方式
            /// </summary>-
            public string RepaymentType { get; set; }

            /// <summary>
            /// 剩余可投金额
            /// </summary>
            public decimal SurplusMoney { get; set; }

            /// <summary>
            /// 项目状态
            /// </summary>
            public string TransferState { get; set; }
            /// <summary>
            /// 项目状态id
            /// </summary>
            public int TransferStateId { get; set; }

            /// <summary>
            /// 还款方式
            /// </summary>
            public string RepeymentTypeCode { get; set; }

            /// <summary>
            /// 原始项目类型
            /// </summary>
            public string ProductType { get; set; }

            public int RemainDay { get; set; }

            public string NextPayDay { get; set; }
    }

    public class RTansferDetail : RTansferAbstract
        {
            public string TransferName { get; set; }

            public string TransferUserName { get; set; }

            public string TransferUserPhone { get; set; }

            public string TransferUserCardId { get; set; }

            public decimal InvestMoney { get; set; }

            public int Period { get; set; }

            public decimal LoanRate { get; set; }

        }
}
