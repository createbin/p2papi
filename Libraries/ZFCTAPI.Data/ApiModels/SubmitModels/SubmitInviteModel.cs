using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.SubmitModels
{
    public class SMonthInvester : BaseSubmitModel
    {
        /// <summary>
        /// 邀请年份
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 邀请月份
        /// </summary>
        public int Month { get; set; }
    }
}
