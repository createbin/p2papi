using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    public class SBHQueryMerchantTrans : SBHBaseModel
    {
        public SBHQueryMerchantTransBody SvcBody { get; set; }=new SBHQueryMerchantTransBody();
    }

    public class SBHQueryMerchantTransBody
    {
        public string startDate { get; set; }
        /// <summary>
        /// yyyyMMdd， 起始日期与结束日期最大间隔为 7 日（如 20170101-20170107)
        /// </summary>
        public string endDate { get; set; }
    }
}
