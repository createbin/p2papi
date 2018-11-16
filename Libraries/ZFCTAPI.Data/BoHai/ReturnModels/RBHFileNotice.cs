using System;
using System.Collections.Generic;
using System.Text;

namespace ZFCTAPI.Data.BoHai.ReturnModels
{
    public class RBHFileNotice:RBHBaseModel
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string fileName { get; set; }
        /// <summary>
        /// 文件类型
        /// 1:批量扣款对账文件；2:批量还款对账文件；3:开销户对账文件；4:商户出入账对账文件；5:营销活动对账文件。
        /// </summary>
        public string fileType { get; set; }
        /// <summary>
        /// 消息扩展
        /// </summary>
        public string extension { get; set; }
    }
}
