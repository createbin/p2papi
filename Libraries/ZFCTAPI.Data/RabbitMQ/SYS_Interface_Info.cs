using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.RabbitMQ
{
    /// <summary>
    /// 接口请求日志
    /// </summary>
    public partial class SYS_Interface_Info : BasePusblishModel
    {
        /// <summary>
        ///  请求类型
        /// </summary>
        public string ITF_info_type { get; set; }

        /// <summary>
        ///  请求参数
        /// </summary>
        public string ITF_req_parameters { get; set; }

        /// <summary>
        ///  返回参数
        /// </summary>
        public string ITF_ret_parameters { get; set; }

        /// <summary>
        ///  是否应答
        /// </summary>
        public string ITF_Info_answer { get; set; }

        /// <summary>
        ///  应答数据
        /// </summary>
        public string ITF_answer_info { get; set; }

        /// <summary>
        ///  添加时间
        /// </summary>
        public DateTime? ITF_Info_addtime { get; set; }

        /// <summary>
        ///  添加人
        /// </summary>
        public string ITF_Info_adduser { get; set; }

        /// <summary>
        ///  cmdid
        /// </summary>
        public string cmdid { get; set; }

        /// <summary>
        ///  回调码
        /// </summary>
        public string respcode { get; set; }

        /// <summary>
        ///  回调描述
        /// </summary>
        public string respdesc { get; set; }

        /// <summary>
        ///  区别接口类别0发送接口参数1接口回调返回信息
        /// </summary>
        public bool Category { get; set; }
    }
}