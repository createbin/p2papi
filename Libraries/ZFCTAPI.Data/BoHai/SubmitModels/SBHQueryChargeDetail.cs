using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    public class SBHQueryChargeDetail:SBHBaseModel
    {
        public SBHQueryChargeDetail()
        {
            SvcBody=new SBHQueryChargeDetailBody();
        }

        public SBHQueryChargeDetailBody SvcBody { get; set; }
    }
    public class SBHQueryChargeDetailBody
    {
        /// <summary>
        /// 存管平台客户号或对公账号根据 accountTyp 取不同值1-账户存管平台客户号2-对公账号
        /// </summary>
        public string accountNo { get; set; }
        /// <summary>
        ///查询方式 1-历史记录查询 2-当前记录查询
        /// </summary>
        public string queryTyp { get; set; }
        /// <summary>
        /// 开始日期 queryTyp-1 必输。 yyyyMMdd
        /// </summary>
        public string startDate { get; set; }
        /// <summary>
        /// 结束日期 
        /// queryTyp-1 必输。 yyyyMMdd。起始日期与结束日期最大间隔为 7 日（如 20170101-20170107） 且结束日最大为 T-2 日
        /// </summary>
        public string endDate { get; set; }
        /// <summary>
        /// 账户存管平台流水号 queryTyp-2 必输
        /// 存管平台返回两个自然日内此流水号至
        /// 最新一笔流水间所有记录（若无历史流水送 0，则返回两天内所有记录
        /// </summary>
        public string transId { get; set; }
        /// <summary>
        /// 页数 当前查询页数（首次查询送 1）
        /// </summary>
        public string pageNo { get; set; }
    }
}
