using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.ReturnModels
{
    /// <summary>
    /// 用户红包
    /// </summary>
    public class RRedEnvelopesModel
    {
        /// <summary>
        /// 红包编号
        /// </summary>
        public int RedId { get; set; }

        /// <summary>
        /// 红包金额
        /// </summary>
        public decimal RedMoney { get; set; }

        /// <summary>
        /// 红包开始时间
        /// </summary>
        public DateTime? RedStartDate { get; set; }

        /// <summary>
        /// 红包截至时间
        /// </summary>
        public DateTime? RedEndDate { get; set; }

        /// <summary>
        /// 红包名称
        /// </summary>
        public string RedName { get; set; }

        /// <summary>
        ///  红包类型
        /// </summary>
        public string RedType { get; set; }

        /// <summary>
        /// 使用说明
        /// </summary>
        public string RedInstructions { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public string RedValidity { get; set; }

        /// <summary>
        ///  红包状态
        /// </summary>
        public string RedUseState { get; set; }

        /// <summary>
        ///  使用的标编号
        /// </summary>
        public int? RedLoanId { get; set; }
    }
}