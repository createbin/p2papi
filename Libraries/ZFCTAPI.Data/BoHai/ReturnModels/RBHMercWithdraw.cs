using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHMercWithdraw : RBHBaseModel
    {
        public RBHMercWithdraw()
        {
            SvcBody=new RBHMercWithdrawBody();
        }

        public RBHMercWithdrawBody SvcBody { get; set; }
    }

    public class RBHMercWithdrawBody
    {
        /// <summary>
        /// 银行流水号
        /// </summary>
        public string bankSerialNo { get; set; }
        /// <summary>
        /// 消息扩展
        /// </summary>
        public string extension { get; set; }
    }
}
