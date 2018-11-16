using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZFCTAPI.Data.ApiModels.SubmitModels
{
    /// <summary>
    /// 基请求模型
    /// </summary>
    public class BaseSubmitModel
    {
        /// <summary>
        /// 签名
        /// </summary>
        public string Signature { get; set; }
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public long TimeStamp { get; set; }
        /// <summary>
        /// 请求方的ip
        /// </summary>
        public string IPAddress { get; set; }
        /// <summary>
        /// 请求来源
        /// </summary>
        public int RequestSource { get; set; }
        /// <summary>
        /// 接口名称
        /// </summary>
        public string ApiName { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public string FacilityType { get; set; }
    }

    /// <summary>
    /// 分页请求基模型
    /// </summary>
    public class BasePageModel : BaseSubmitModel
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// 条数
        /// </summary>
        public int PageSize { get; set; }
    }


    public class BaseCountModel : BaseSubmitModel
    {
        /// <summary>
        ///获取数量 
        /// </summary>
        public int Count { get; set; }
    }
}
