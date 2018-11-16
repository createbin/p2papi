using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{


    public class RBHRechargeSyn : RBHBaseModel
    {
        public RBHRechargeSynBody SvcBody { get; set; }
    }

    public class RBHRechargeSynBody
    {
        /// <summary>
        /// 资金种类
        /// N
        /// 1投资  2融资
        /// </summary>
        public string capTyp { get; set; }

        /// <summary>
        /// 可用余额 
        /// N
        /// </summary>
        public string avlBal { get; set; }

    }
}
