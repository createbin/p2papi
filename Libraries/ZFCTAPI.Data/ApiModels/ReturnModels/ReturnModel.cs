using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.ReturnModels
{
    /// <summary>
    ///返回数据类型
    /// </summary>
    /// <typeparam name="T">返回数据主体类型</typeparam>
    /// <typeparam name="TS">Token类型</typeparam>
    public class ReturnModel<T,TS>
    {
        //返回码
        public int ReturnCode { get; set; }
        //返回数据
        public T ReturnData { get; set; }
        //token
        public TS Token { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Signature { get; set; }
        //提示信息
        public string Message { get; set; }
        //额外字段
        public string Extra1 { get; set; }
        //额外字段
        public string Extra2 { get; set; }
    }

    public class RToPage
    {
        /// <summary>
        /// 跳转链接
        /// </summary>
        public string Url { get; set; }
    }

    public class RToInvestResult
    {
        public string ErrorInfo { get; set; }

        public string ErrorCode { get; set; }
    }
}
