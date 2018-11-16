using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Core.Configuration;
using ZFCTAPI.Core.Helpers;

namespace ZFCTAPI.Data.BoHai.SubmitModels
{
    public class SBHBaseModel
    {
        public SBHBaseModel()
        {
            ReqSvcHeader=new SBHBaseHeaderModel();
        }

        /// <summary>
        /// 接口名
        /// </summary>
        public string serviceName { get; set; }

        /// <summary>
        /// 公共请求基类
        /// </summary>
        public SBHBaseHeaderModel ReqSvcHeader { get; set; }
    }

    public class SBHBaseHeaderModel
    {
        //public SBHBaseHeaderModel()
        //{
        //    evidenceCust = new List<EvidenceCust>();
        //}

        /// <summary>
        /// 不用传值 系统日期，YYYYMMDD
        /// </summary>
        public string tranDate = DateTime.Now.ToString("yyyyMMdd");

        /// <summary>
        /// 不用传值 各平台按日切时间点，确定的平台交易日，YYYYMMDD
        /// </summary>
        public string tranDay = DateTime.Now.ToString("yyyyMMdd");

        /// <summary>
        ///不用传值 交易时间，单位毫秒，样式：hhmmssSSS。平台侧交易实际发生时间
        /// </summary>
        public string tranTime = DateTime.Now.ToString("HHmmssfff");

        /// <summary>
        /// 互金平台交易流水号
        /// </summary>
        public string tranSeqNo { get; set; } = CommonHelper.GetMchntTxnSsn();

        /// <summary>
        /// 清算系统分配给互金平台的系统编码
        /// </summary>
        public string consumerId { get; set; } = BoHaiApiEngineToConfiguration.ConsumerId();

        /// <summary>
        /// 互金平台的交易发起IP地址
        /// </summary>
        public string ip { get; set; } = ZfctWebEngineToConfiguration.GetIPAddress();

        /// <summary>
        /// 报文签名数据
        /// </summary>
        public string signature { get; set; }

        /// <summary>
        /// 签名数字证书标识(浙商银行必填)
        /// </summary>
        public string certId { get; set; }

        /// <summary>
        /// 银行分配给平台或者结算系统的唯一标识
        /// </summary>
        public string instId { get; set; } = BoHaiApiEngineToConfiguration.InstId();

        /// <summary>
        /// 预留字段
        /// </summary>
        public string extension { get; set; }

        /// <summary>
        /// 是否存证(0 否 1是)
        /// </summary>
        public string evidFlag { get; set; }

        /// <summary>
        /// 当事人列表(如果evidFlag=1，则必填)
        /// </summary>
        //public List<EvidenceCust> evidenceCust { get; set; }
    }

    public class EvidenceCust
    {
        /// <summary>
        /// 当事人姓名/名称
        /// </summary>
        public string custName { get; set; }

        /// <summary>
        /// 当事人手机号
        /// </summary>
        public string custMobile { get; set; }

        /// <summary>
        /// 当事人证件类型代码，详见证件类型列表
        /// </summary>
        public string custIdType { get; set; }

        /// <summary>
        /// 当事人证件号码
        /// </summary>
        public string custIdNo { get; set; }

        /// <summary>
        /// 当事人互金平台账号
        /// </summary>
        public string account { get; set; }

        /// <summary>
        /// 当事人互金平台账号注册时间，格式：yyyyMMddHHmmss
        /// </summary>
        public string registerTime { get; set; }
    }
}