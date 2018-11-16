using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{

    /// <summary>
    ///批量用户注册请求实体
    /// </summary>
    public class RBHRegisterApplication
    {

        public RBHUserRegister RspSvcHeader { get; set; }
    }

    public class RBHUserRegister
    {
        public string tranTime { get; set; }

        public string returnCode { get; set; }

        public string tranDate { get; set; }

        public string globalSeqNo { get; set; }

        public string returnMsg { get; set; }

        public string backendSysId { get; set; }

        public string RespCode { get; set; }
    }

    /// <summary>
    /// 存量用户注册 异步放回参数Model
    /// </summary>
    public class UserRegisterAsyncRecieve
    {
        public string partner_id { get; set; }

        public string version_no { get; set; }

        public string biz_type { get; set; }

        public string sign_type { get; set; }

        public string BatchNo { get; set; }

        public string MerBillNo { get; set; }

        public string RespCode { get; set; }

        public string RespDesc { get; set; }

        public string mac { get; set; }

    }




}
