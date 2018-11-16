using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHQueryMerchantAccts : RBHBaseModel
    {
        public RBHQueryMerchantAcctsBody SvcBody { get; set; }
    }

    public class RBHQueryMerchantAcctsBody
    {
        public RBHQueryMerchantAcctsBody()
        {
            items=new List<Items>();
        }

        public List<Items> items { get; set; }
    }

    public class Items
    {
        /// <summary>
        /// 账户类型 800：手续费收入账户
        /// 810：营销账户
        /// 820：预付费账户 （手续费支出） 
        /// 830：现金账户 
        /// 840：信用账户
        /// </summary>
        public string acTyp { get; set; }
        /// <summary>
        /// 可用余额
        /// </summary>
        public string avlBal { get; set; }
        /// <summary>
        /// 账户余额
        /// </summary>
        public string actBal { get; set; }
        /// <summary>
        /// 冻结余额
        /// </summary>
        public string frzBal { get; set; }
    }

    public class MerchantMoney
    {
        /// <summary>
        /// 可用余额
        /// </summary>
        public string AvlBal { get; set; }

        /// <summary>
        /// 账户余额
        /// </summary>
        public string ActBal { get; set; }

        /// <summary>
        /// 冻结金额
        /// </summary>
        public string FrzBal { get; set; }
    }
}
