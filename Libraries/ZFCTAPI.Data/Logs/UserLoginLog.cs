using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Data.RabbitMQ;

namespace ZFCTAPI.Data.Logs
{
    public class UserLoginLog:BasePusblishModel
    {
        /// <summary>
        /// 操作人员编号
        /// </summary>
        public int Operator { get; set; }

        /// <summary>
        /// 操作人员名称
        /// </summary>
        public string OperatorName { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperatingTime { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 操作返回信息
        /// </summary>
        public string ReturnInfo { get; set; }

        /// <summary>
        /// 设备来源
        /// </summary>
        public int Source { get; set; }

        /// <summary>
        /// 设备地址
        /// </summary>
        public string DeviceAddress { get; set; }

        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public string DeviceType { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string VersionNum { get; set; }

        /// <summary>
        /// BY1
        /// </summary>
        public string BY1 { get; set; }

        /// <summary>
        /// BY2
        /// </summary>
        public string BY2 { get; set; }

    }
}
