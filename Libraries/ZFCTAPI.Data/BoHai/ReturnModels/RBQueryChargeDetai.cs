using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBQueryChargeDetail
    {
        public RBQueryChargeDetail()
        {
            items = new List<ChargeDetai>();
        }

        /// <summary>
        /// 总页数(当前条件可查询数据总页数（PageNo 为1 时返回)
        /// </summary>
        public string totalPage { get; set; }

        /// <summary>
        /// 总笔数（PageNo 为 1 时返回）
        /// </summary>
        public string totalNum { get; set; }

        /// <summary>
        /// 明细数据
        /// </summary>
        public List<ChargeDetai> items { get; set; }
    }

    public class ChargeDetai
    {
        /// <summary>
        /// 记账日期
        /// </summary>
        public string acdate { get; set; }

        /// <summary>
        /// 账户存管平台流水号
        /// </summary>
        public string transId { get; set; }

        /// <summary>
        /// 交易金额
        /// </summary>
        public string transAmt { get; set; }

        /// <summary>
        /// 手续费
        /// </summary>
        public string feeAmt { get; set; }

        /// <summary>
        /// 交易状态
        /// S1 交易成功,已清分
        /// F1 交易失败, 未清分
        /// W2 请求处理中
        /// W3 系统受理中
        /// W4 银行受理中
        /// S2 撤标解冻成功
        /// S3 放款解冻成功
        /// B1 部分成功, 部分冻结
        /// R9 审批拒绝
        /// F2 撤标解冻失败
        /// </summary>
        public string transStat { get; set; }

        /// <summary>
        /// 失败原因
        /// </summary>
        public string falRsn { get; set; }
    }
}
