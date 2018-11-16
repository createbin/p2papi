using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.ReturnModels
{
    public class RMerchantToUserMoney
    {
        public int SuccessCount { get; set; }

        public int ErrorCount { get; set; }

        public List<TransResult> TransResults { get; set; }=new List<TransResult>();
    }

    public class TransResult{

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string UserPhone { get; set; }

        public string Success { get; set; }

        public string ErrorInfo { get; set; }
    }


    public class RefundRecond
    {
       /// <summary>
       /// 交易流水号
       /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// 营销活动编号
        /// </summary>
        public string ProjectCode { get; set; }

        /// <summary>
        /// 营销活动信息
        /// </summary>
        public string ProjectInfo { get; set; }

        /// <summary>
        /// 操作日期
        /// </summary>
        public string OperateDate { get; set; }
        /// <summary>
        /// 转账手机号
        /// </summary>
        public string UserPhone { get; set; }
        /// <summary>
        /// 转账用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 操作金额
        /// </summary>
        public string OperateMoney { get; set; }
        /// <summary>
        /// 操作状态
        /// </summary>
        public string OperateState { get; set; }
        /// <summary>
        /// 操作状态描述
        /// </summary>
        public string OperateStateDesc { get; set; }
        /// <summary>
        /// 失败原因
        /// </summary>
        public string FailReason { get; set; }
    }
}
